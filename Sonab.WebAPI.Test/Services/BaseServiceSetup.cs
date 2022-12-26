using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Sonab.WebAPI.Test.Services;

public abstract class BaseServiceSetup
{
    protected readonly Mock<HttpContext> _mockContext = new();
    protected readonly Mock<IHttpContextAccessor> _mockAccessor = new();

    protected BaseServiceSetup()
    {
        _mockAccessor.Setup(x => x.HttpContext).Returns(_mockContext.Object);
    }

    protected void SetUserId(string id)
    {
        _mockContext.Setup(x => x.User).Returns(new ClaimsPrincipal(new[]
        {
            new ClaimsIdentity(!string.IsNullOrEmpty(id) ? new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id)
            } : new Claim[0])
        }));
    }
}

