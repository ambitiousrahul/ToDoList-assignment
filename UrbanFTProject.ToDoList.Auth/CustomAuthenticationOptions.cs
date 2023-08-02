using Microsoft.AspNetCore.Authentication;

namespace UrbanFTProject.ToDoList.Auth
{
    public class CustomAuthenticationOptions:AuthenticationSchemeOptions
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string SignInKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
}