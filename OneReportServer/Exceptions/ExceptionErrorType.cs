namespace OneReportServer.Exceptions
{
    public enum ExceptionErrorType
    {
        EmailIsNotValid = 4002,
        OTPTokenIsNotValid = 4003,
        OTPCodeIsNotValid = 4004,
        LoginTokenExpired = 4007,
        EmailAlreadyExist = 4008,
        FailureCreate = 4009,
        InvalidMessageType = 4010,
        MessageContentRequired = 4011,
        InvalidDateFormat = 4012, 
        NotFound = 4013, //TODO: For Itay
        EventNotFound = 4014, 
        UserNotFound = 4015, 
        FileMissingOrEmpty = 4016,
        UnsupportedFile = 4017,
        LoginOTPTokenDoesntFound = 4018,
        LoginOTPCodeDoesntMatch = 4019,

    }
}
