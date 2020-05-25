using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SSO.Models.DatabaseModels;
using SSO.Models.Errors;
using SSO.Services.Abstract;

namespace SSO.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IGetToken getToken;
        private readonly ITokenCheck tokenCheck;
        private readonly ITokenRefresh tokenRefresh;

        public AuthController(IGetToken getToken, ITokenCheck tokenCheck, ITokenRefresh tokenRefresh)
        {
            this.getToken = getToken;
            this.tokenCheck = tokenCheck;
            this.tokenRefresh = tokenRefresh;
        }

        /// <summary>
        /// Method to validate LogIn.
        /// Calls method <see cref="IGetToken.Token(string, string)"/> to get token.
        /// </summary>
        /// <returns>
        /// Token in JSON-format.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null or token is invalid.</exception>
        /// <param name="user"><see cref="User"/> type model.</param>
        [AllowAnonymous]
        [HttpPost]
        [Route(Routes.CheckLogin)]
        public string LoginAuthorize([FromBody] User user)
        {
            var token = getToken.Token(user.Email, user.Password);
            if (token != null)
            {
                //maybe some token checking
                return token;
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to refresh token's expiration date.
        /// </summary>
        /// <returns>
        /// Token in JSON-format.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null or token is invalid.</exception>
        /// <param name="inputToken">A json-token in string format.</param>
        [HttpPost]
        [Route(Routes.RefreshToken)]
        public string RefreshToken([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                return tokenRefresh.TokenRefreshing(inputToken);
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Method to check if token is valid.
        /// </summary>
        /// <returns>
        /// Status code.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when parameter is null or token is invalid.</exception>
        /// <param name="inputToken">A json-token in string format.</param>
        [HttpPost]
        [Route(Routes.CheckToken)]
        public IActionResult CheckToken([FromHeader(Name = "Authorization")] string inputToken)
        {
            if (tokenCheck.TokenChecking(inputToken))
                throw new SuccessCustomException();
            throw new ArgumentNullException();
        }
    }
}