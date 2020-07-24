using Auth.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Model;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;

namespace SampleAuthJWT.Filter
{
    public class CustomAuthorize : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;
        private readonly JWTModule _module;
        private readonly ValidateModel _validateModel;

        public CustomAuthorize(IConfiguration configuration, JWTModule module, ValidateModel validateModel)
        {
            _configuration = configuration;
            _module = module;
            _validateModel = validateModel;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string secrect = _configuration.GetSection("AuthJWT").GetSection("Secrect").Value;
            var requestToken = context.HttpContext.Request.Headers["AuthJWT"].FirstOrDefault();

            //Below code must be from DB or config.
            _validateModel.Issuer = "authjwt_team";
            _validateModel.UserId = "U1324322";
            _validateModel.User = "test_user";
            _validateModel.Role = "Admin";
            _validateModel.Audience = "authjwt_app";
            _validateModel.JwtId = "J4433421";
            _validateModel.Subject = "authjwt_subject";
            _validateModel.CustomProperty.Add("CustomField1", "auth_custom1");
            _validateModel.CustomProperty.Add("CustomField2", "auth_custom2");

            var validateResult = _module.VerifyToken(requestToken, secrect, _validateModel);

            if (validateResult.Status.Trim().ToUpper() == "FAILED")
            {
                context.Result = new CustomUnauthorizedResult(validateResult.Content);
            }

            base.OnActionExecuting(context);

        }

    }

    public class CustomUnauthorizedResult : JsonResult
    {
        public CustomUnauthorizedResult(string message)
            : base(new CustomError(message))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }

    public class CustomError
    {
        public string Error { get; }

        public CustomError(string message)
        {
            Error = message;
        }
    }
}
