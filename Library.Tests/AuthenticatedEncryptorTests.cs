using System.Security.Cryptography;
using Xunit.Abstractions;

namespace Library.Tests;

public class AuthenticatedEncryptorTests(ITestOutputHelper output)
{
    private const string Plaintext = "Ueberweise 100 Franken an Bob....und noch mehr Text";

    [Fact]
    public void WhatGoesInComesOut()
    {
        var testee = new AuthenticatedEncryptor();

        Assert.Equal(Plaintext, testee.Decrypt(testee.Encrypt(Plaintext)));
    }

    [Fact]
    public void CbcAcceptsAManipulatedCiphertext()
    {
        var cbc = new SymmetricEncryptor();
        var raw = Convert.FromHexString(cbc.Encrypt(Plaintext));

        raw[11] ^= (byte)('1' ^ '9');

        var tampered = cbc.Decrypt(Convert.ToHexStringLower(raw));
        output.WriteLine("CBC nach der Manipulation: " + tampered);

        Assert.Equal("Ueberweise 900 Franken an Bob....und noch mehr Text", tampered);
    }

    [Fact]
    public void GcmRefusesAManipulatedCiphertext()
    {
        var gcm = new AuthenticatedEncryptor();
        var raw = Convert.FromHexString(gcm.Encrypt(Plaintext));

        raw[12] ^= 0x01;

        Assert.Throws<AuthenticationTagMismatchException>(() =>
            gcm.Decrypt(Convert.ToHexStringLower(raw))
        );
    }
}
