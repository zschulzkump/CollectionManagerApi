namespace Collection_Manager_Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api/v{version:ApiVersion}";

    public static class V1
    {
        public static class Accounts
        {
            private const string Base = $"{ApiBase}/accounts";
            public const string Create = $"{Base}/register";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string RefreshToken = $"{Base}/{{id:guid}}/refresh-token";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";

        }
        
        public static class Authentication
        {
            private const string Base = $"{ApiBase}/authentication";
            public const string Login = $"{Base}/login";
            public const string Logout = $"{Base}/logout";
        }
    }
}