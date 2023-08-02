namespace UrbanFTProject.ToDoList.Data
{
    public interface IUserRepository
    {
        Task<AspNetUsers> GetUserByEmail(string email);
    }
}
