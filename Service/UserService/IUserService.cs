using learnApi.Models;

namespace learnApi.Service.UserService
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();
        User GetSingleUser(int id);

        List<User> AddUser(User user);

        List<User>? UpdateUser(int id, User request);
    }
}