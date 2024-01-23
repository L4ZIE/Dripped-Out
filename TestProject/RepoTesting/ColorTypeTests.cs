using infrastructure.DatabaseManager;
using infrastructure.Entities;
using infrastructure.Repositories;
using NUnit.Framework;

namespace TestProject.RepoTesting;

public class ColorTypeTests
{
    [TestFixture]
    public class UserTest
    {
        private DBConnection _dbConnection;
        private ColorTypeRepository _repository;

        [SetUp]
        public void Setup()
        {
            Environment.SetEnvironmentVariable("IsTestMode", "true");
            _dbConnection = new DBConnection();
            _dbConnection.TriggerRebuild();
            _repository = new ColorTypeRepository(_dbConnection);
        }

        [Test]
        public void CreateColorType()
        {
            var colorType = new ColorType
            {
                Color = "Grey"
            };

            var createdColor = _repository.Create(colorType);
            
            Assert.That(createdColor.Color, Is.EqualTo(colorType.Color));
        }

        [Test]
        public void ReadColorType()
        {
            var expectedBlack = "Black";
            var expectedWhite = "White";
            var expectedBlue = "Blue";

            var shouldNotMatch = "not a color";

            var retrievedBlack = _repository.Read(1);
            var retrievedWhite = _repository.Read(2);
            var retrievedBlue = _repository.Read(3);
            
            Assert.That(expectedBlack, Is.EqualTo(retrievedBlack.Color));
            Assert.That(expectedWhite, Is.EqualTo(retrievedWhite.Color));
            Assert.That(expectedBlue, Is.EqualTo(retrievedBlue.Color));
            Assert.That(shouldNotMatch, Is.Not.EqualTo(retrievedBlack.Color));
        }
        
        [TearDown]
        public void CleanUp()
        {
            Environment.SetEnvironmentVariable("IsTestMode", "false");
        }
    }
}