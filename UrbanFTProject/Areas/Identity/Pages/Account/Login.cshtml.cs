// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable


using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Common;
using UrbanFTProject.ToDoList.Data;

namespace UrbanFTProject.ToDoList.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UrbanFTAssignmentDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger, UserManager<IdentityUser> userManager, UrbanFTAssignmentDbContext dbContext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = dbContext;
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;

            var url = Url.ActionLink(action: "Login", controller: "Account", values: new
            {
                returnURL = returnUrl
            }, protocol: Request.Scheme);

            if (returnUrl.Contains("/api/"))
            {
                Redirect(url);
            }
            
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                var user = await _userManager.FindByEmailAsync(Input.Email);

                var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, false);


                var token = _userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");
                var url = Url.ActionLink(action: "Login", controller: "LoginRedirect", values: new
                {
                    Token = token.Result,
                    Email = Input.Email,
                    ReturnUrl = returnUrl
                }, protocol: Request.Scheme);


                if (result.Succeeded)
                {
                    _logger.LogInformation($"User: {user.UserName} logged in.");
                    return Redirect(url);
                }
                else
                {
                    if (user is null)
                    {
                        ModelState.AddModelError("invalidUser", "User does not exists");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                    return Unauthorized();
                }
                //if (user != null)
                //{
                //    var token = _userManager.GenerateUserTokenAsync(user, "Default", "passwordless-auth");
                //    var url = Url.ActionLink(action: "Login", controller: "LoginRedirect", values: new
                //    {
                //        Token = token.Result,
                //        Email = Input.Email,
                //        ReturnUrl = returnUrl
                //    }, protocol: Request.Scheme);

                //    return Redirect(url);
                //    //return Ok(url);
                //}
                //return Unauthorized();

            }
            return Page();
        }
    }
}

