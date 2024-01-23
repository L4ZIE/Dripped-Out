using infrastructure.DatabaseManager;
using infrastructure.Entities;
using infrastructure.Repositories;
using NUnit.Framework;

namespace TestProject.RepoTesting
{
    [TestFixture]
    public class UserTest
    {
        private DBConnection _dbConnection;
        private UserRepository userRepository;
        
        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable("IsTestMode", "true");
            _dbConnection = new DBConnection();
            _dbConnection.TriggerRebuild();
            userRepository = new UserRepository(_dbConnection);
        }


        [Test]
        public void TestDeleteUser()
        {
            const int deleteId = 1;
            
            userRepository.Delete(deleteId);
            try
            {
                var nullUser = userRepository.Read(deleteId);
            }
            catch (Exception e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void TestUpdateUser()
        {
            var user = new Users
            {
                UserId = 2,
                Email = "Updated@Email.com",
                IsAdmin = true
            };
            userRepository.Update(user);
            var updatedUser = userRepository.Read(2);
            
            Assert.That(user.UserId, Is.EqualTo(updatedUser.UserId));
            Assert.That(user.Email, Is.EqualTo(updatedUser.Email));
            Assert.That(user.IsAdmin, Is.EqualTo(updatedUser.IsAdmin));
        }

        [Test]
        public void TestReadUser()
        {
            const int userId = 2;
            const int expectedId = 2;
            const string expectedEmail = "test2@dapper.com";
            const bool expectedAdmin = false;
            
            var readUser = userRepository.Read(userId);
            
            Assert.That(readUser.UserId, Is.EqualTo(expectedId));
            Assert.That(readUser.Email, Is.EqualTo(expectedEmail));
            Assert.That(readUser.IsAdmin, Is.EqualTo(expectedAdmin));
            
        }

        [Test]
        public void TestCreateUser()
        {
            var createdUser = new Users
            {
                Email = "Updated@Email.com",
                IsAdmin = true
            };

            var expectedUser = userRepository.Create(createdUser);
            
            Assert.That(expectedUser.UserId, Is.EqualTo(4));
            Assert.That(expectedUser.Email, Is.EqualTo(createdUser.Email));
            Assert.That(expectedUser.IsAdmin, Is.EqualTo(createdUser.IsAdmin));
        }
        
        [TearDown]
        public void CleanUp()
        {
            Environment.SetEnvironmentVariable("IsTestMode", "false");
        }
    }
}