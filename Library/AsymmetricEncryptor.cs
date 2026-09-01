using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// RSA. Exercise C3.
/// </summary>
// What this exercise was about.
//
// RSA does not encrypt data, it encrypts a number smaller than the modulus. A 2048 bit
// key gives 256 bytes, and every padding spends some of them. OAEP with SHA-256 spends
// two hashes and a separator, so 256 - 2 * 32 - 2 = 190 bytes are left, PKCS#1 v1.5 gets
// by with 11 bytes and leaves 245. The tests pin no padding, only that 32 bytes fit and
// 256 do not.
//
// A 4 GB file therefore never goes through RSA. You draw a random AES key, encrypt the
// file with that, and encrypt only the 32 byte key with RSA. That is hybrid encryption,
// and it is why RSA in practice only ever wraps session keys.
public class AsymmetricEncryptor
{
    private readonly RSA keyPair = RSA.Create(2048);

    public string Encrypt(string plaintext)
    {
        var bytes = Encoding.UTF8.GetBytes(plaintext);
        return Convert.ToHexStringLower(
            this.keyPair.Encrypt(bytes, RSAEncryptionPadding.OaepSHA256)
        );
    }

    public string Decrypt(string hex)
    {
        var encrypted = Convert.FromHexString(hex);
        return Encoding.UTF8.GetString(
            this.keyPair.Decrypt(encrypted, RSAEncryptionPadding.OaepSHA256)
        );
    }
}
