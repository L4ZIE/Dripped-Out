using System;
using NUnit.Framework;
using infrastructure;
using infrastructure.Entities;
using infrastructure.Repositories;


namespace Tests.RepoTesting
{
    public class UserRepositoryTests
    {

        [Test]
        public void TestCreateUser()
        {
            var dbConnection = new DBConnection();
            var userRepository = new UserRepository(dbConnection);
            var user = new Users
            {
                Email = "test@email.com",
                IsAdmin = true
            };
            
            userRepository.Create(user);
            
            Console.WriteLine("User hase ben created!!");
        }

        [Test]
        public void TestDeleteUser()
        {
            var dbConnection = new DBConnection();
            var userRepository = new UserRepository(dbConnection);
            var user = new Users
            {
                UserId = 1
            };
            userRepository.Delete(user.UserId);
        }

        [Test]
        public void TestUpdateUser()
        {
            var dbConnection = new DBConnection();
            var userRepository = new UserRepository(dbConnection);
            var user = new Users
            {
                UserId = 2,
                Email = "Updated@Email.com",
                IsAdmin = true
            };
            userRepository.Update(user);
        }

        [Test]
        public void TestReadUser()
        {
            var dbConnection = new DBConnection();
            var userRepository = new UserRepository(dbConnection);
            const int userId = 2;

            var readUser = userRepository.Read(userId);

            Console.WriteLine($"User ID: {readUser.UserId}");
            Console.WriteLine($"User Email: {readUser.Email}");
            Console.WriteLine($"User Type: {readUser.IsAdmin}");
            
            
        }
    }
}