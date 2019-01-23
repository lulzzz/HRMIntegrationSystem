namespace Timereg.Api.Unimicro.Constants
{
    public static class Constants
    {
        public static class Mapping
        {
            public const string UnimicroUserMapKey = "Employee=>EmployeeId to Worker=>WorkedId";
            public const string UnimicroUnitMapKey = "Company=>CompanyId to Company=>CompanyKey";
        }

        public static class API
        {
            public const string API_HEADER_COMPANYKEY = "CompanyKey";
            public const string API_CLIENT_NAME = "UnimicroClient";
            public const string API_URL_CONFIG_KEY = "Unimicro.Api.Url";
            public const string API_COMPANYKEY_CONFIG_KEY = "Unimicro.Api.CompanyKey";
            public const string API_USERNAME_CONFIG_KEY = "Unimicro.Api.Username";
            public const string API_PASSWORD_CONFIG_KEY = "Unimicro.Api.Password";
        }
    }

}
