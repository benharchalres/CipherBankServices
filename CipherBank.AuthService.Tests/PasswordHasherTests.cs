

namespace CipherBank.AuthService.Tests
{
    using CipherBank.AuthService.Identity.Password;
    public class Tests
    {
        public BcryptPasswordHasher hasher;
        public string Password;

        [SetUp]
        public void Setup()
        {
            //Arrange
            hasher = new BcryptPasswordHasher();
            Password = "Demo@123";
        }

        [Test]
        public void HashReturnNonEmptyString()
        {
            // Act
            string hash = hasher.Hash(Password);
            
            //Assert
            Assert.IsNotNull(hash);
            Assert.IsNotEmpty(hash);
        }

        [Test]
        public void HashDoesNotContainsPlainText()
        {
            // Act
            string hash = hasher.Hash(Password);
            Assert.IsFalse(hash.Contains(Password), "Hash should not contain the original password.");
        }

        [Test]
        public void HashExpectedFormat()
        {
            // Act
            string hash = hasher.Hash(Password);
            Assert.IsTrue(hash.StartsWith("$2"), "Hash should start with $2 indicating BCrypt format.");
        }

        [Test]
        public void VerifyCorrectPasswordReturnsTrue()
        {
            //Act
            string hash = hasher.Hash(Password);

            bool result = hasher.VerifyHash(Password, hash);

            Assert.IsTrue(result, "Verify should return true for the correct password.");

        }
        [Test]
        public void VerifyCorrectPasswordReturnsFalse()
        {
            string hash = hasher.Hash(Password);

            bool result = hasher.VerifyHash("Wrong@123",hash);

            Assert.IsFalse(result, "Verify should return false for the incorrect password.");
        }
    }
}