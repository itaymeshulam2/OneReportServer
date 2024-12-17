using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OneReportServer.Client.Implementation;
using OneReportServer.Client.Interface;
using OneReportServer.DB;
using OneReportServer.Helper;
using OneReportServer.Manager.Implementation;
using OneReportServer.Manager.Interface;
using OneReportServer.Middleware;
using OneReportServer.Model;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .SetBasePath(GeneralHelper.GetBasePathLocation(null))
    .AddEnvironmentVariables()
    .Build();

const string template =
    "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}]: {Message:lj} {NewLine}{Exception}";
var logger = new LoggerConfiguration()
    .WriteTo.File(Path.Combine("logs", "reportone-api", "ReportOneServer_.txt"), outputTemplate: template,
        rollingInterval: RollingInterval.Day, retainedFileCountLimit: 15, fileSizeLimitBytes: 1073741824, shared: true)
    .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.SystemConsoleTheme.Literate, outputTemplate: template);


Log.Logger = logger.CreateLogger();
Log.Information($"Starting up repot one");

//TODO: FIX env var
//TODO: Add Number of connection string limit
builder.Services.AddDbContextPool<AppDBContext>(options =>
    options.UseNpgsql(SettingsDetails.DBConnectionString), 300);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin() //TODO: Add specific origin
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers().AddJsonOptions(x =>
{
    // serialize enums as strings in api responses (e.g. Role)
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddSingleton<StackExchange.Redis.IDatabase>(cfg =>
{
    //Log.Information($"Redis connection: {SettingsDetails.RedisHost}:{SettingsDetails.RedisPort},password={SettingsDetails.RedisPassword?.Length}");
    //Log.Information("Redis connection code: ConnectionMultiplexer.Connect($\"{SettingsDetails.RedisHost}:{SettingsDetails.RedisPort},password={SettingsDetails.RedisPassword?.Length}\")");
    //await _cache.StringSetAsync(RedisKeys.LOCATION_CHARGE_POINTS, JsonConvert.SerializeObject(locationChargePoints), TimeSpan.FromMinutes(4));
    IConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect(
        $"{SettingsDetails.RedisHost}:{SettingsDetails.RedisPort},password={SettingsDetails.RedisPassword}");
    return multiplexer.GetDatabase();
});

builder.Services.AddScoped<ExceptionMiddleware>();
builder.Services.AddScoped<RequestBodyMiddleware>();
builder.Services.AddScoped<ILoginManager, LoginManager>();
builder.Services.AddScoped<IReportOne, ReportOneManager>();
builder.Services.AddSingleton<IRedisClient, RedisClient>();
builder.Services.AddSingleton<IEmailClient, EmailClient>();


builder.Services.AddHostedService<RedisClient>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = $"Report one API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// Add Mapping
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();


// Configure the HTTP request pipeline.

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();
app.UseGlobalRequestBodyMiddlewareHandler();
//app.UseAuthorzationMiddlewareHandler();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();
app.UseCors(MyAllowSpecificOrigins);

SettingsDetails.LoadAllSettings();

app.Run();