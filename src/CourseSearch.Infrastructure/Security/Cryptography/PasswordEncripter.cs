using CourseSearch.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CourseSearch.Infrastructure.Security.Cryptography;
internal class PasswordEncripter : IPasswordEncripter
{
    public string Encript(string password)
    {
        string passwordHash = BC.HashPassword(password);


        return passwordHash;
    }

    public bool Verify(string password, string passwordHash)
    {
        return BC.Verify(password, passwordHash);
    }
}
