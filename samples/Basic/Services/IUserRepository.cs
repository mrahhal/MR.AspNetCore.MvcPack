using Basic.Models;

namespace Basic.Services
{
	public interface IUserRepository
	{
		AppUser FindById(int id);
	}
}
