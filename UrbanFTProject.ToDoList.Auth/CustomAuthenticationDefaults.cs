
namespace UrbanFTProject.ToDoList.Auth
{
    internal struct CustomAuthenticationDefaults
    {        
        public const string AUTHENTICATION_SCHEME = "Bearer";
        public const string AUTHORISATION_HEADER = "Authorization";

        public static string[] GetHeaderTypes()
        {
            return new string[] { AUTHENTICATION_SCHEME, AUTHORISATION_HEADER };
        }
    }
}
