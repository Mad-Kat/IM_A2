using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// AES in GCM mode. Exercise C2.
/// </summary>
public class AuthenticatedEncryptor
{
    private const int NonceLength = 12;
    private const int TagLength = 16;

    private readonly byte[] key = Convert.FromHexString(
        "eb213655ce7ac25591def2aad983e4f0c261ec890577a3f5babeea0670d6d110"
    );

    public string Encrypt(string plaintext)
    {
        var nonce = RandomNumberGenerator.GetBytes(NonceLength);
        var bytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = new byte[bytes.Length];
        var tag = new byte[TagLength];

        // TODO: implement

        return Convert.ToHexStringLower([.. nonce, .. ciphertext, .. tag]);
    }

    public string Decrypt(string hex)
    {
        var raw = Convert.FromHexString(hex);
        var nonce = raw[..NonceLength];
        var ciphertext = raw[NonceLength..^TagLength];
        var tag = raw[^TagLength..];
        var plaintext = new byte[ciphertext.Length];

        // TODO: implement

        return Encoding.UTF8.GetString(plaintext);
    }
}
