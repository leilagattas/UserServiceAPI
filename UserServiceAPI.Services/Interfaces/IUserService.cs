using UserServiceAPI.Domain.Models;
using UserServiceAPI.Services.DTO;

namespace UserServiceAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllActive();
        Task<UserDto> Create(UserCreateDto userCreateDto);
        Task<User> UpdateActive(Guid id);
        Task<bool> Delete(Guid id);
    }
}
