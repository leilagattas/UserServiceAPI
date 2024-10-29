using AutoMapper;
using Moq;
using NUnit.Framework;
using UserServiceAPI.Common.Models;
using UserServiceAPI.Domain.Models;
using UserServiceAPI.Infrastructure.Interfaces;
using UserServiceAPI.Services.DTO;
using UserServiceAPI.Services.Services;

namespace UserServiceAPI.Testing.Services
{
    public class UserServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private UserService _userService;


        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userService = new UserService(_unitOfWorkMock.Object,
                                           _mapperMock.Object);
        }

        [Test(Description = "Tests GetAllActive() Method on user service")]
        public void GetAllActive_ReturnsAllActiveUsers()
        {
            var activeUsers = MockedUsers().Where(u => u.Active).ToList();

            _unitOfWorkMock.Setup(x => x.Users.GetAllActive())
                            .ReturnsAsync(activeUsers);

            var mockedUserDtos = new List<UserDto>
            {
                new UserDto
                {
                    Id = new Guid("584b8acf-2da0-4179-9321-9def780d3b22"),
                    Name = "test 1",
                    Birthdate = new DateTime(1990, 2, 10, 0, 0, 0, DateTimeKind.Utc)
                }
            };

            _mapperMock.Setup(m => m.Map<IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                        .Returns(mockedUserDtos);

            var result = _userService.GetAllActive();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result.Count(), Is.EqualTo(1));
            Assert.That(result.Result.First().Id, Is.EqualTo(new Guid("584b8acf-2da0-4179-9321-9def780d3b22")));
            Assert.That(result.Result.First(), Is.InstanceOf<UserDto>());
        }

        [Test(Description = "Tests GetAllActive() Method on user service and throws exception")]
        public void GetAllActive_ThrowsAnException()
        {
            _unitOfWorkMock.Setup(x => x.Users.GetAllActive()).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(async () => await _userService.GetAllActive());
        }

        [Test(Description = "Tests Create() Method on user service")]
        public void Create_AddsAndReturnsANewUser()
        {
            var userCreateDto = new UserCreateDto
            {
                Name = "test user",
                Birthdate = new DateTime(1994, 9, 25, 0, 0, 0, DateTimeKind.Utc)
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = userCreateDto.Name,
                Birthdate = userCreateDto.Birthdate,
                Active = true
            };

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Birthdate = user.Birthdate
            };

            _mapperMock.Setup(m => m.Map<User>(userCreateDto)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

            _unitOfWorkMock.Setup(x => x.Users.Add(user)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var result = _userService.Create(userCreateDto);

            Assert.IsNotNull(result);
            Assert.That(result.Result.Id, Is.EqualTo(userDto.Id));
            Assert.That(result.Result.Name, Is.EqualTo(userDto.Name));
            Assert.That(result.Result.Birthdate, Is.EqualTo(userDto.Birthdate));

            _unitOfWorkMock.Verify(x => x.Users.Add(user), Times.Once);
            _unitOfWorkMock.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Test(Description = "Tests Create() Method when Add fails")]
        public void Create_ThrowsDBErrorException_WhenAddFails()
        {
            var userCreateDto = new UserCreateDto { Name = "test user" };
            var user = new User { Id = Guid.NewGuid(), Name = userCreateDto.Name };

            _mapperMock.Setup(m => m.Map<User>(userCreateDto)).Returns(user);

            _unitOfWorkMock.Setup(x => x.Users.Add(It.IsAny<User>())).ThrowsAsync(new DBErrorException("An error occurred while adding the user."));

            var ex = Assert.ThrowsAsync<DBErrorException>(async () => await _userService.Create(userCreateDto));
            Assert.That(ex.Message, Is.EqualTo("An error occurred while adding the user."));
        }

        [Test(Description = "Tests UpdateActive() Method on user service")]
        public void UpdateActive_TogglesActiveStatusFromUser()
        {
            var userIdToUpdate = new Guid("584b8acf-2da0-4179-9321-9def780d3b22");
            var userToUpdate = MockedUsers().First(u => u.Id == userIdToUpdate);
            userToUpdate.Active = !userToUpdate.Active;

            _unitOfWorkMock.Setup(x => x.Users.UpdateActive(userIdToUpdate))
                           .ReturnsAsync(userToUpdate);
            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            var result = _userService.UpdateActive(userIdToUpdate);

            Assert.That(result, Is.Not.Null);
            Assert.IsFalse(result.Result.Active);

            _unitOfWorkMock.Verify(x => x.Users.UpdateActive(userIdToUpdate), Times.Once);
            _unitOfWorkMock.Verify(x => x.CompleteAsync(), Times.Once);
        }
        [Test(Description = "Tests Delete() Method on user service")]
        public async Task Delete_RemovesUser()
        {
            var userIdToDelete = new Guid("584b8acf-2da0-4179-9321-9def780d3b22");
            var activeUsers = MockedUsers().Where(u => u.Active).ToList();

            _unitOfWorkMock.Setup(x => x.Users.GetAllActive())
                            .ReturnsAsync(activeUsers);

            _unitOfWorkMock.Setup(x => x.CompleteAsync()).ReturnsAsync(1);

            _unitOfWorkMock.Setup(x => x.Users.Remove(userIdToDelete)).ReturnsAsync(true);

            var result = await _userService.Delete(userIdToDelete);

            Assert.IsNotNull(result);
            Assert.IsTrue(result);

            _unitOfWorkMock.Verify(x => x.Users.Remove(userIdToDelete), Times.Once);
            _unitOfWorkMock.Verify(x => x.CompleteAsync(), Times.Once);

            var remainingUsers = await _userService.GetAllActive();
            Assert.That(remainingUsers, Is.Empty);
        }
        private static IEnumerable<User> MockedUsers()
        {
            IEnumerable<User> users = new List<User>()
                {
                    new User
                    {
                        Id = new Guid("584b8acf-2da0-4179-9321-9def780d3b22"),
                        Name = "test 1",
                        Birthdate = new DateTime(1990, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                        Active = true
                    },
                    new User
                    {
                        Id = new Guid("c21f0c03-6570-4209-8920-e476b9cc3b60"),
                        Name = "test 2",
                        Birthdate = new DateTime(1994, 9, 25, 0, 0, 0, DateTimeKind.Utc),
                        Active = false
                    }
                };

            return users;
        }
    }
}
