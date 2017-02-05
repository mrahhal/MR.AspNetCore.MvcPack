using System.Collections.Generic;
using System.Linq;
using Basic.Models;

namespace Basic.Services
{
	public class UserRepository : IUserRepository
	{
		private List<AppUser> _users = new List<AppUser>
		{
			new AppUser
			{
				Id = 1,
				Name = "user 1"
			},
			new AppUser
			{
				Id = 42,
				Name = "user 42"
			}
		};

		public AppUser FindById(int id)
		{
			return _users.FirstOrDefault(u => u.Id == id);
		}
	}
}
