using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// SHA-256. Exercise C5.
/// </summary>
public class Hasher
{
    public string Hash(string plaintext)
    {
        var bytes = Encoding.UTF8.GetBytes(plaintext);

        // TODO: implement
        byte[] hash = [];

        return Convert.ToHexStringLower(hash);
    }
}
