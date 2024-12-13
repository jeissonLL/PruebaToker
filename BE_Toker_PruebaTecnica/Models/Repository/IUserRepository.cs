namespace BE_Toker_PruebaTecnica.Models.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetListUser();
        Task<User?> GetUser(int id);
        Task deleteUser(User user);
        Task<User> AddUser(User user);
        Task UpdateUser(User user);
    }
}
