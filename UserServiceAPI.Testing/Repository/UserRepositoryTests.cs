using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserServiceAPI.Domain.Models;
using UserServiceAPI.Infrastructure;
using UserServiceAPI.Infrastructure.Repository;

namespace UserServiceAPI.Testing.Repository
{
    public class UserRepositoryTests
    {
        private DbContextOptions<UserDbContext> _db;
        private UserDbContext _userDbContext;
        private UserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _db = new DbContextOptionsBuilder<UserDbContext>().UseInMemoryDatabase("test").Options;
            _userDbContext = new UserDbContext(_db);
            _repository = new UserRepository(_userDbContext);
        }

        [Test(Description = "Tests Add() Method on user resository")]
        public async Task Add_ShouldCreateANewUser()
        {
            _userDbContext.Database.EnsureDeleted();

            var repository = new UserRepository(_userDbContext);

            var user = new User
            {
                Id = new Guid("584b8acf-2da0-4179-9321-9def780d3b22"),
                Name = "test 1",
                Birthdate = new DateTime(1990, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                Active = true
            };

            await repository.Add(user);
            await _userDbContext.SaveChangesAsync();

            var users = await repository.GetAllActive();
            var newUser = users.FirstOrDefault();

            Assert.IsNotNull(newUser);
            Assert.That(newUser.Name, Is.EqualTo("test 1"));
            Assert.That(newUser.Birthdate, Is.EqualTo(new DateTime(1990, 2, 10, 0, 0, 0, DateTimeKind.Utc)));
            Assert.IsInstanceOf<User>(newUser);
        }

        [Test(Description = "Tests GetAllActive() Method on user resository")]
        public async Task GetAllActive_ShouldReturnActiveUsers()
        {
            _userDbContext.Database.EnsureDeleted();

            var repository = new UserRepository(_userDbContext);

            _userDbContext.User.AddRange(new List<User>()
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
                },
                new User
                {
                    Id = new Guid("08ee9c87-fcca-45db-a855-ff773a59d939"),
                    Name = "test 3",
                    Birthdate = new DateTime(1985, 12, 01, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                }
            });

            _userDbContext.SaveChanges();

            var result = await repository.GetAllActive();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToList().Count);
        }

        [Test(Description = "Tests Remove() Method on user resository")]
        public async Task Remove_ShouldRemoveRecordFromDatabase()
        {
            _userDbContext.Database.EnsureDeleted();

            var repository = new UserRepository(_userDbContext);

            _userDbContext.User.AddRange(new List<User>()
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
                },
                new User
                {
                    Id = new Guid("08ee9c87-fcca-45db-a855-ff773a59d939"),
                    Name = "test 3",
                    Birthdate = new DateTime(1985, 12, 01, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                }
            });

            _userDbContext.SaveChanges();

            var result = await repository.Remove(new Guid("584b8acf-2da0-4179-9321-9def780d3b22"));

            _userDbContext.SaveChanges();

            var deletedProd = _userDbContext.User.Find(new Guid("584b8acf-2da0-4179-9321-9def780d3b22"));

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.IsNull(deletedProd);
        }

        [Test(Description = "Tests UpdateActive() Method on user resository")]
        public async Task UpdateActive_ShouldUpdateActiveStatusToOppositteOfCurrent()
        {
            _userDbContext.Database.EnsureDeleted();

            var repository = new UserRepository(_userDbContext);
            
            Guid id = new Guid("584b8acf-2da0-4179-9321-9def780d3b22");

            _userDbContext.User.AddRange(new List<User>()
            {
                 new User
                {
                    Id = id,
                    Name = "test 1",
                    Birthdate = new DateTime(1990, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                    Active = true
                }
            });

            _userDbContext.SaveChanges();

            var result = await repository.UpdateActive(id);
            var users = await repository.GetAllActive();
            var updatedUser = users.FirstOrDefault();

            Assert.IsNotNull(updatedUser);
            Assert.IsFalse(updatedUser.Active);
        }
    }
}
