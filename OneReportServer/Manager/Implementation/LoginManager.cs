using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OneReportServer.Client.Interface;
using OneReportServer.Contract.Request;
using OneReportServer.Contract.Response;
using OneReportServer.DB;
using OneReportServer.DB.Redis;
using OneReportServer.Exceptions;
using OneReportServer.Helper;
using OneReportServer.Manager.Interface;
using OneReportServer.Model;
using OneReportServer.Model.Enum;

namespace OneReportServer.Manager.Implementation
{
    public class LoginManager : ILoginManager
    {
        private readonly ILogger<LoginManager> _logger;
        private readonly AppDBContext _dbContext;
        private readonly IRedisClient _redisClient;
        private readonly IEmailClient _emailClient;


        public LoginManager(ILogger<LoginManager> logger, AppDBContext dbContext, IRedisClient redisClient, IEmailClient emailClient)
        {
            _logger = logger;
            _dbContext = dbContext;
            _redisClient = redisClient;
            _emailClient = emailClient;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            ValidationLogin(request);
            var otpModel = await ValidationOTP(request);
            return await CreateJWTForUserAfterUserLogedIn(request.Email);
        }
        public async Task<UserAvailableResponse> IsUserAvailable(UserAvailableRequest request)
        {
            ValidateUserAvailableRequest(request);
            var existingUser = await _dbContext.UserDetails.FirstOrDefaultAsync(user => user.Email == request.Email);
            if (existingUser == null)
            {
                return new UserAvailableResponse
                {
                    IsUserAvailable = false
                };
            }

            var otpModel = new OTPModel
            {
                Email = request.Email,
                AvailableToken = Guid.NewGuid().ToString(),
                OTPToken = Guid.NewGuid().ToString(),
                OTPCode = GeneralHelper.CreateRandomNumber(6),
            };

            await _redisClient.SetOTPAvailableToken(otpModel);
            await _redisClient.SetOTPToken(otpModel);
            _emailClient.SendMessage($"Hi, You got message for Login, Your OTP code is: {otpModel.OTPCode}", otpModel.Email );

            return new UserAvailableResponse
            {
                AvailableToken = otpModel.AvailableToken,
                IsUserAvailable = true,
                OTPToken = otpModel.OTPToken,
            };
        }
        
        private async Task<LoginResponse> CreateJWTForUserAfterUserLogedIn(string email)
        {
            var existingUser = await _dbContext.UserDetails.FirstOrDefaultAsync(user => user.Email == email);

            //TODO: Update login succeeded
            var userDetails = new UserDetails
            {
                UserID = existingUser.Id,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                Email = existingUser.Email,
                Role = (eRoleType)existingUser.Role,
            };
            return new LoginResponse
            {
                JWT = CreateJWTDetails(userDetails)
            };
        }

        private string CreateJWTDetails(UserDetails userDetails)
        {
            var jwt = "";
            List<Claim> claims = new List<Claim>()
            {
                new Claim(nameof(ClaimTypes.NameIdentifier), userDetails.Email),
                new Claim("sub", userDetails.UserID.ToString()),
                new Claim(nameof(ClaimTypes.Role), ((int)userDetails.Role).ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsDetails.JWTTokenPrivateKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(SettingsDetails.JWTExpireIn),
                signingCredentials: cred);
            jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        
        private void ValidationLogin(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || !GeneralHelper.IsValidPhone(request.Email))
            {
                _logger.LogInformation($"Email is not valid: [{request.Email}]");
                throw new UnauthorizedException(ExceptionErrorType.EmailIsNotValid);
            }

            if (string.IsNullOrEmpty(request.OTPToken))
            {
                _logger.LogInformation($"OTPToken is not valid: [{request.OTPToken}]");
                throw new UnauthorizedException(ExceptionErrorType.OTPTokenIsNotValid);
            }

            if (string.IsNullOrEmpty(request.OTPCode))
            {
                _logger.LogInformation($"OTPCode is not valid: [{request.OTPCode}]");
                throw new UnauthorizedException(ExceptionErrorType.OTPCodeIsNotValid);
            }
        }

        private async Task<OTPModel> ValidationOTP(LoginRequest request)
        {
            var otpToken = await _redisClient.GetOTPToken(request.OTPToken);

            if (request.Email != otpToken.Email)
            {
                _logger.LogWarning($"OTP failed. The OTP Email in redis not match for same Token. KeyToken: [{request.OTPToken}] Redis phone: [{otpToken.Email}], The request is: [{request.Email}]");
                throw new UnauthorizedException(ExceptionErrorType.LoginOTPCodeDoesntMatch);
            }

            if (otpToken == null || string.IsNullOrEmpty(otpToken?.OTPCode))
            {
                _logger.LogInformation($"OTP failed. The OTPkey in redis doesnt found. KeyToken: [{request.OTPToken}] for phone: [{request.Email}]");
                throw new UnauthorizedException(ExceptionErrorType.LoginOTPTokenDoesntFound);
            }

            if (request.OTPCode != otpToken.OTPCode)
            {
                _logger.LogWarning($"OTP failed. The OTP code in redis not match. KeyToken: [{request.OTPToken}] Phone: [{otpToken.Email}], The request code is: [{request.OTPCode}], The Redis code is: [{otpToken.OTPCode}]");
                throw new UnauthorizedException(ExceptionErrorType.LoginOTPCodeDoesntMatch);
            }

            if (!string.IsNullOrEmpty(otpToken.AvailableToken) && otpToken.AvailableToken != request.AvailableToken)
            {
                _logger.LogWarning($"OTP failed. The OTP code in redis not match. AvailableToken: [{request.AvailableToken}] Phone: [{request.Email}], The request token is: [{request.AvailableToken}], The Redis token is: [{otpToken.AvailableToken}]");
                throw new UnauthorizedException(ExceptionErrorType.LoginOTPTokenDoesntFound);
            }

            otpToken.IsUserPhoneIsValid = true;
            return otpToken;
        }
        private void ValidateUserAvailableRequest(UserAvailableRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || !GeneralHelper.IsValidPhone(request.Email))
            {
                _logger.LogInformation($"Email is not valid: [{request.Email}]");
                throw new BadRequestException(ExceptionErrorType.EmailIsNotValid);
            }
        }
    }
}