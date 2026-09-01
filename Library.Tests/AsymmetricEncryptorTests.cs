using System.Security.Cryptography;

namespace Library.Tests;

public class AsymmetricEncryptorTests
{
    private const string Plaintext = "Don't tell anybody: I'm encrypted";

    private readonly AsymmetricEncryptor testee = new();

    [Fact]
    public void WhatGoesInComesOut()
    {
        var encrypted = this.testee.Encrypt(Plaintext);

        Assert.NotEqual(Plaintext, encrypted);
        Assert.Equal(Plaintext, this.testee.Decrypt(encrypted));
    }

    [Fact]
    public void RsaCannotEncryptMoreThanItsKeyAllows()
    {
        Assert.ThrowsAny<CryptographicException>(() => this.testee.Encrypt(new string('a', 500)));
    }

    [Theory]
    [InlineData(32, true)]
    [InlineData(256, false)]
    public void TheLimitIsTwoHundredAndFiftySixMinusPadding(int length, bool fits)
    {
        var plaintext = new string('a', length);

        if (fits)
        {
            this.testee.Encrypt(plaintext);
            return;
        }

        Assert.ThrowsAny<CryptographicException>(() => this.testee.Encrypt(plaintext));
    }
}
