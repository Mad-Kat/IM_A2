using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// AES in CBC mode. Exercise C1.
/// </summary>
// What this exercise was about.
//
// The key stayed secret the whole time and the round trip always worked, so nothing
// looked broken. The weakness was the second constant in the constructor, a fixed IV:
// the same message always produced the same ciphertext. An eavesdropper who cannot read
// a single byte still learns that a message was repeated. Where the set of possible
// messages is small, yes or no, buy or sell, that is the whole content.
//
// WhatGoesInComesOut was green after the first line of the exercise and is still green
// now, with a completely different scheme underneath. A round-trip test cannot see this,
// because both directions make the same mistake. Green means you get back what you put
// in; it says nothing about what a third party can work out.
public class SymmetricEncryptor
{
    private const int IvLength = 16;

    private readonly Aes aes;

    public SymmetricEncryptor()
    {
        this.aes = Aes.Create();
        this.aes.Key = Convert.FromHexString(
            "eb213655ce7ac25591def2aad983e4f0c261ec890577a3f5babeea0670d6d110"
        );
    }

    public string Encrypt(string plaintext)
    {
        var iv = RandomNumberGenerator.GetBytes(IvLength);
        var encrypted = this.aes.EncryptCbc(Encoding.UTF8.GetBytes(plaintext), iv);

        // The IV is not secret. It travels with the ciphertext so the receiver can decrypt.
        return Convert.ToHexStringLower([.. iv, .. encrypted]);
    }

    public string Decrypt(string hex)
    {
        var raw = Convert.FromHexString(hex);
        var iv = raw[..IvLength];
        var encrypted = raw[IvLength..];

        return Encoding.UTF8.GetString(this.aes.DecryptCbc(encrypted, iv));
    }
}
