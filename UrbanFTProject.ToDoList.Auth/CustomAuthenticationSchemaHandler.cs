
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Nimbly_Core.CustomAPI.Helpers;
//using Nimbly_Core.CustomAPI.Interfaces;
//using System.Security.Claims;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;
//using UrbanFTProject.ToDoList.Auth;

//namespace Nimbly_Core.CustomAPI.Services
//{
//    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
//    {
//        private readonly ICustomAuthenticationService _authenticationService;

//        private const int MIN_ENCODEDKEY_LEGAL_KEY_SIZE = 16;

//        public CustomAuthenticationHandler(
//            UrlEncoder encoder,
//            IOptionsMonitor<CustomAuthenticationOptions> options,
//            ILoggerFactory logger,
//            ISystemClock clock,
//            ICustomAuthenticationService authenticationService)
//            : base(options, logger, encoder, clock)
//        {
//            _authenticationService = authenticationService;
//        }

//        /// <summary>
//        /// Overridden method of abstract class AuthenticationHandler to handle authentication logic
//        /// </summary>
//        /// <returns></returns>
//        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            string authenticationKey;
//            string invalid_auth_message = "Could not read the Authorization header";
//            if (!Request.Headers.ContainsKey(CustomAuthenticationDefaults.AUTHENTICATION_HEADER))
//            {
//                bool isOtherHeaderExists = Request.Headers.ContainsKey("Authorization") || Request.Headers.ContainsKey("Jwt");
//                if (isOtherHeaderExists)
//                {
//                    authenticationKey = (Request.Headers.ContainsKey("Authorization") ?
//                                        Request.Headers["Authorization"] : Request.Headers["Jwt"]);

//                    //as the obtained key should be JWT token key, so removing 'Bearer' from the beginning of the token.

//                    authenticationKey = (authenticationKey.StartsWith("Bearer") ?
//                        authenticationKey.Replace("Bearer", string.Empty).Trim() : authenticationKey);

//                    string result = _authenticationService.IsAuthenticatedUserJWT(authenticationKey);
//                    _ = await _authenticationService.RefreshCache(result);
//                    AuthenticateResult authenticationResult = (!string.IsNullOrEmpty(result) ?
//                        CreateAuthenticationTicket(result) :
//                        AuthenticateResult.Fail(invalid_auth_message));

//                    return authenticationResult;
//                }

//                return AuthenticateResult.Fail(invalid_auth_message);
//            }

//            authenticationKey = Request.Headers[CustomAuthenticationDefaults.AUTHENTICATION_HEADER];

//            if (string.IsNullOrEmpty(authenticationKey))
//            {
//                return AuthenticateResult.Fail("Auth key cannot be empty");
//            }
//            if (authenticationKey.Length < MIN_ENCODEDKEY_LEGAL_KEY_SIZE)
//            {
//                return AuthenticateResult.Fail("Invalid auth key.");
//            }

//            string isValidUser = await _authenticationService.IsAuthenticatedUser(authenticationKey);

//            if (string.IsNullOrEmpty(isValidUser))
//            {
//                return AuthenticateResult.Fail("Failed API KEY Authentication");
//            }

//            AuthenticateResult ticket = CreateAuthenticationTicket(isValidUser);
//            return ticket;
//        }
//        private AuthenticateResult CreateAuthenticationTicket(string User)
//        {
//            var claims = new[] { new Claim(ClaimTypes.Name, User) };
//            var identity = new ClaimsIdentity(claims, Scheme.Name);
//            var principal = new ClaimsPrincipal(identity);
//            var ticket = new AuthenticationTicket(principal, Scheme.Name);

//            return AuthenticateResult.Success(ticket);
//        }
//    }
//}
