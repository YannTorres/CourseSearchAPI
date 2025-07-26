using CourseSearch.Domain.Entities;
using CourseSearch.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CourseSearch.Infrastructure.DataAcess.Repositories;
internal class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly CourseSearchDbContext _dbcontext;

    public UserRepository(CourseSearchDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    public async Task Add(User user)
    {
        await _dbcontext.Users.AddAsync(user);
    }

    public async Task Delete(User user)
    {
        var userToRemove = await _dbcontext.Users.FindAsync(user.Id);

        _dbcontext.Users.Remove(userToRemove!);
    }

    public Task<bool> ExistUserWithEmail(string email)
    {
        var exists = _dbcontext.Users.AnyAsync(u => u.Email == email);

        return exists;
    }

    public async Task<User> GetById(Guid id)
    {
        var user = await _dbcontext.Users.AsNoTracking().FirstAsync(u => u.Id == id);

        return user;
    }

    public Task<User?> GetUserByEmail(string email)
    {
        var user = _dbcontext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }

    public void Update(User user)
    {
        _dbcontext.Users.Update(user);
    }
}
