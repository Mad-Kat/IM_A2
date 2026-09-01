using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// SHA-256. Exercise C5.
/// </summary>
// What this exercise was about.
//
// SHA-256 is built to be fast, and for passwords that is precisely the wrong property.
// PasswordSpeedTests puts numbers on it: a laptop does millions of plain hashes per
// second and a few dozen PBKDF2 derivations at 100'000 iterations. The gap is what a
// dictionary attack has to pay, and it is around four orders of magnitude.
//
// The iteration count is a security parameter, not a constant. Hardware gets faster, so
// it is raised over the years to keep that gap. bcrypt and Argon2 go further and make
// the work memory hungry as well, which hurts a GPU more than a slow loop does.
public class Hasher
{
    public string Hash(string plaintext)
    {
        var bytes = Encoding.UTF8.GetBytes(plaintext);

        return Convert.ToHexStringLower(SHA256.HashData(bytes));
    }
}
