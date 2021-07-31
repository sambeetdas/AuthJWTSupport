using Auth.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAuthJWT.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWTModule _module;
        private readonly IConfiguration _configuration;
        private readonly ValidateModel _validateModel;

        public AuthJwtMiddleware(RequestDelegate next, JWTModule module, IConfiguration configuration, ValidateModel validateModel)
        {
            _next = next;
            _module = module;
            _configuration = configuration;
            _validateModel = validateModel;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            if (httpContext.Request.Path.Equals("/getdataauthmiddleware"))
            {
                string secrect = _configuration.GetSection("AuthJWT").GetSection("Secrect").Value;

                var request = httpContext.Request;
                var requestToken = httpContext.Request.Headers["AuthJWT"].FirstOrDefault();

                //Below code must be from DB or config.
                _validateModel.Issuer = "authjwt_team";
                _validateModel.UserId = "U1324322";
                _validateModel.User = "test_user";
                _validateModel.Role = "Admin";
                _validateModel.Audience = "authjwt_app";
                _validateModel.JwtId = "J4433421";
                _validateModel.Subject = "authjwt_subject";
                _validateModel.CustomProperty = new System.Collections.Generic.Dictionary<string, string>();
                _validateModel.CustomProperty.Add("CustomField1", "auth_custom1");
                _validateModel.CustomProperty.Add("CustomField2", "auth_custom2");

                var validateResult = _module.VerifyToken(requestToken, secrect, _validateModel);

                if (validateResult.Status.Trim().ToUpper() == "FAILED")
                {
                    await httpContext.Response.WriteAsync(validateResult.Content);
                    return;
                }
            }

                
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthJwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthJwtMiddleware>();
        }
    }
}
