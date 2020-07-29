using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.JWT;
using Auth.JWT.Common;
using Auth.JWT.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SampleAuthJWT.Model;

namespace SampleAuthJWT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JWTModule _module;
        private readonly TokenRequestModel _reqModel;
        private readonly IConfiguration _configuration;

        public AuthController(JWTModule module, TokenRequestModel reqModel, IConfiguration configuration)
        {
            _module = module;
            _reqModel = reqModel;
            _configuration = configuration;
        }

        [HttpPost("createtoken")]
        public IActionResult CreateToken([FromBody] UserModel user)
        {
            dynamic tokenResult = null;

            //Note: This is a Sample app to demonstrate Auth.JWT.
            //Hence authentication is done with static values.
            if (user.Username == "sambeet" && user.Password == "12345")
            {
                //Below code must be from DB or config.
                _reqModel.Issuer = "authjwt_team"; 
                _reqModel.ExpiryInSeconds = "18000"; 
                _reqModel.UserId = "U1324322"; 
                _reqModel.User = "test_user"; 
                _reqModel.Role = "Admin"; 
                _reqModel.Audience = "authjwt_app"; 
                _reqModel.JwtId = "J4433421"; 
                _reqModel.Subject = "authjwt_subject"; 
                _reqModel.CustomProperty.Add("CustomField1", "auth_custom1"); 
                _reqModel.CustomProperty.Add("CustomField2", "auth_custom2");

                //Secrect must be from DB or config.
                string secrect = _configuration.GetSection("AuthJWT").GetSection("Secrect").Value;

                //Below fuction would create the token.
                tokenResult = _module.CreateToken(_reqModel, secrect, AlgorithmType.SHA256);
            }
            

            return Ok(tokenResult);
        }
    }
}