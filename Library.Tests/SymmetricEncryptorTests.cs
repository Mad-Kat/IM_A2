namespace Library.Tests;

public class SymmetricEncryptorTests
{
    private const string Plaintext = "Don't tell anybody: I'm encrypted";

    private readonly SymmetricEncryptor testee = new();

    [Fact]
    public void WhatGoesInComesOut()
    {
        var encrypted = this.testee.Encrypt(Plaintext);

        Assert.NotEqual(Plaintext, encrypted);
        Assert.Equal(Plaintext, this.testee.Decrypt(encrypted));
    }

    [Fact]
    public void SamePlaintextGivesDifferentCiphertexts()
    {
        var first = this.testee.Encrypt(Plaintext);
        var second = this.testee.Encrypt(Plaintext);

        Assert.NotEqual(first, second);
        Assert.Equal(Plaintext, this.testee.Decrypt(first));
        Assert.Equal(Plaintext, this.testee.Decrypt(second));
    }
}
