using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// AES in CBC mode. Exercise C1.
/// </summary>
public class SymmetricEncryptor
{
    private readonly Aes aes;

    public SymmetricEncryptor()
    {
        this.aes = Aes.Create();
        this.aes.Key = Convert.FromHexString(
            "eb213655ce7ac25591def2aad983e4f0c261ec890577a3f5babeea0670d6d110"
        );
        this.aes.IV = Convert.FromHexString("c8ef77b5f071d8d30c9a0592a98f00e9");
    }

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
        var decrypted = this.aes.DecryptCbc(encrypted, this.aes.IV);

        return Encoding.UTF8.GetString(decrypted);
    }
}
