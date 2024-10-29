using AutoMapper;
using UserServiceAPI.Domain.Models;
using UserServiceAPI.Infrastructure.Interfaces;
using UserServiceAPI.Services.DTO;
using UserServiceAPI.Services.Interfaces;

namespace UserServiceAPI.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllActive()
        {
            var users = await _unitOfWork.Users.GetAllActive();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> Create(UserCreateDto userCreateDto)
        {
            User user = _mapper.Map<User>(userCreateDto);
            await _unitOfWork.Users.Add(user);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<User> UpdateActive(Guid id)
        {
            var result = await _unitOfWork.Users.UpdateActive(id);
            await _unitOfWork.CompleteAsync();
            return result;
        }
        public async Task<bool> Delete(Guid id)
        {
            var result = await _unitOfWork.Users.Remove(id);
            await _unitOfWork.CompleteAsync();
            return result;
        }
    }
}
