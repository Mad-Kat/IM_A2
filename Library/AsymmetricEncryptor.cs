using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// RSA. Exercise C3.
/// </summary>
public class AsymmetricEncryptor
{
    // TODO: implement
    private readonly RSA keyPair = RSA.Create();

    public string Encrypt(string plaintext)
    {
        var bytes = Encoding.UTF8.GetBytes(plaintext);

        // TODO: implement
        byte[] encrypted = [];

        return Convert.ToHexStringLower(encrypted);
    }

    public string Decrypt(string hex)
    {
        var encrypted = Convert.FromHexString(hex);

        // TODO: implement
        byte[] decrypted = [];

        return Encoding.UTF8.GetString(decrypted);
    }
}
