
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;

//namespace UrbanFTProject.ToDoList.Auth
//{
//    /// <summary>
//    /// Overridden method of abstract class AuthenticationHandler to handle authentication logic
//    /// </summary>
//    /// <returns></returns>
//    internal class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
//    {
//        private readonly IOptionsMonitor<CustomAuthenticationOptions> signInUserOptions;
//        public CustomAuthenticationHandler(IOptionsMonitor<CustomAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
//        {
//            signInUserOptions = options;
//        }

//        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
//        {

//            var authHeaders = CustomAuthenticationDefaults.GetHeaderTypes();
            
//            if (!CustomAuthenticationDefaults.GetHeaderTypes().Any(header => Request.Headers.ContainsKey(header)))
//            {
//                // Redirect the user to the Login action of the AccountController
//                var authenticationProperties = new AuthenticationProperties
//                {
//                    RedirectUri = "/Account/Login" // Set the URL to the action you want to redirect to,
                   
//                };

//                return Task.FromResult(AuthenticateResult.Fail("UnAuthorized", authenticationProperties));

//            }
//            else
//            {
//                string? userName = null;
//                for (int index = 0; index < authHeaders.Length; index++)
//                {
//                    if (Request.Headers.ContainsKey(authHeaders[index]))
//                    {
//                        string authToken = Request.Headers[authHeaders[index]][1].Trim();
//                        userName= ValidateUserToken(authToken);
//                        if (!string.IsNullOrWhiteSpace(userName))
//                        {
//                            break;
//                        }
//                    }
//                }
//                return !string.IsNullOrWhiteSpace(userName) ? Task.FromResult(CreateAuthenticationTicket(userName)) : Task.FromResult(AuthenticateResult.Fail("Failed Authentication"));
//            }
//        }

//        private string? ValidateUserToken(string userToken)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();

//            var signinKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(signInUserOptions.CurrentValue.SignInKey)
//                );
//            if (tokenHandler.CanReadToken(userToken))
//            {
//                tokenHandler.ValidateToken(userToken, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = signinKey,

//                    ValidateIssuer = true,
//                    ValidIssuer = signInUserOptions.CurrentValue.Issuer,

//                    ValidateAudience = true,
//                    ValidAudience = signInUserOptions.CurrentValue.Audience

//                }, out SecurityToken validatedToken);

//                var userName = (validatedToken as JwtSecurityToken)?.Claims.First(claim => claim.Type == "name").Value;

//                return userName;
//            }
//           return default;
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
