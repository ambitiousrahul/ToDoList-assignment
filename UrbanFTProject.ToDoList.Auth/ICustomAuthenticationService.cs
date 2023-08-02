
namespace UrbanFTProject.ToDoList.Auth
{
    internal interface ICustomAuthenticationService
    {
        Task<string> ValidateUserToken(string authToken);
    }
}
