using Microsoft.IdentityModel.Tokens;

namespace CipherBank.AuthService.Tests
{

using CipherBank.AuthService.Application.Service.Contract;
using CipherBank.AuthService.Domain.Entities;
using CipherBank.AuthService.Identity.TokenService;
    [TestFixture]
public class JwtTokenServiceTests
{
   public ITokenService _tokenService;

    [OneTimeSetUp]
    public void Setup()
    {
            
    }

    [Test]
    public void GenerateTokenFor_Null_ThrowsArgumentNullException()
    {


        var ex = Assert.ThrowsAsync<ArgumentNullException>(async() => await _tokenService.GenerateAsync(null,_ct));

        StringAssert.Contains("User parameter not be null", ex.Message);

        Assert.AreEqual("user",ex.ParamName);
    }
}
}