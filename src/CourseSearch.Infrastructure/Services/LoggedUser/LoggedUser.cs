using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Security.Tokens;
using CourseSearch.Domain.Services.LoggedUser;
using CourseSearch.Infrastructure.DataAcess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CourseSearch.Infrastructure.Services.LoggedUser;
internal class LoggedUser : ILoggedUser
{
    private readonly CourseSearchDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;
    public LoggedUser(CourseSearchDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }
    public async Task<User> Get()
    {
        string token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler(); 

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type.Equals(ClaimTypes.Sid)).Value;

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(users => users.Id.Equals(Guid.Parse(identifier)));
    }

}
