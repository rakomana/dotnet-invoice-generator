using learnApi.Models;
using learnApi.Data;

namespace learnApi.Service.UserService
{
    public class UserService : IUserService
    {
        private static List<User> users = new List<User> {
            new User {
                Id = 1,
                FirstName = "Prince",
                LastName = "Rakomana"
            },
            new User {
                Id = 2,
                FirstName = "Sydney",
                LastName = "Rakomana"
            }
        };

        private readonly DataContextEF _context;

        public UserService(DataContextEF context)
        {
            _context = context;
        }

        public DataContextEF Context => _context;

        public DataContextEF Context1 => _context;

        public List<User> AddUser(User user)
        {
            users.Add(user);

            return users;
        }

        public User GetSingleUser(int id)
        {
            var user = users.Find(x => x.Id == id);

            if(user is null)
                return null;
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await Context.users.ToListAsync();

            return users;
        }

        public List<User>? UpdateUser(int id, User request)
        {
            var user = users.Find(x => x.Id == id);

            if(user is null)
                return null;
                
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            return users;
        }
    }
}