using Microsoft.AspNetCore.Authorization;

namespace Setting.Mvc.Authorize
{
    public class DefaultAuthorizeAttribute : AuthorizeAttribute
    {
        public const string DefaultAuthenticationScheme = "DefaultAuthenticationScheme";
        public DefaultAuthorizeAttribute()
        {
            this.AuthenticationSchemes = DefaultAuthenticationScheme;
        }
    }
}