namespace CollectionManagerApi;

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
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
        
        public static class Authentication
        {
            private const string Base = $"{ApiBase}/authentication";
            public const string Login = $"{Base}/login";
            public const string Logout = $"{Base}/logout";
            public const string RefreshToken = $"{ApiBase}/accounts/{{id:guid}}/refresh-token";
        }
        public static class Collections
        {
            private const string Base = $"{ApiBase}/collections";
            public const string Create = $"{Base}/";
            public const string GetAll = $"{Base}/";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string GetByName = $"{Base}/{{name:string}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
        public static class Storages
        {
            private const string Base = $"{ApiBase}/storages";
            public const string Create = $"{Base}/";
            public const string GetAll = $"{Base}/";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string GetByName = $"{Base}/{{name:string}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
        
        public static class CollectionItems
        {
            private const string Base = $"{ApiBase}/collection-items";
            public const string Create = $"{ApiBase}/collections/{{id:guid}}/collection-items";
            public const string GetAll = $"{Base}/";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string GetByName = $"{Base}/{{name:string}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
        public static class CustomAttributes
        {
            private const string Base = $"{ApiBase}/custom-attributes";
            public const string Create = $"{Base}/";
            public const string GetAll = $"{Base}/";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string GetByName = $"{Base}/{{name:string}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
        public static class CollectionItemAttributes
        {
            private const string Base = $"{ApiBase}/collection-item-attributes";
            public const string Create = $"{ApiBase}/collection-items/{{id:guid}}/attributes";
            public const string GetAll = $"{ApiBase}/collection-items/{{id:guid}}/attributes";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
        public static class CollectionItemImages
        {
            private const string Base = $"{ApiBase}/collection-item-images";
            public const string Create = $"{ApiBase}/collection-items/{{id:guid}}/images";
            public const string GetAll = $"{ApiBase}/collection-items/{{id:guid}}/images";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
        public static class CollectionItemDocuments
        {
            private const string Base = $"{ApiBase}/collection-item-documents";
            public const string Create = $"{ApiBase}/collection-items/{{id:guid}}/documents";
            public const string GetAll = $"{ApiBase}/collection-items/{{id:guid}}/documents";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
        
        public static class CollectionItemTags
        {
            private const string Base = $"{ApiBase}/collection-item-tags";
            public const string Create = $"{ApiBase}/collection-items/{{id:guid}}/tags";
            public const string GetAllByCollectionId = $"{ApiBase}/collection-items/{{id:guid}}/tags";
            public const string GetAll = $"{Base}/";
            public const string GetById = $"{Base}/{{id:guid}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }
    }
}