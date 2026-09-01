using System.Security.Cryptography;
using System.Text;

namespace Library.Tests;

public class SignerTests
{
    private const string Message = "Ueberweise 100 Franken an Bob";

    private static readonly HashAlgorithmName[] Hashes =
    [
        HashAlgorithmName.SHA256,
        HashAlgorithmName.SHA384,
        HashAlgorithmName.SHA512,
    ];

    private static readonly RSASignaturePadding[] Paddings =
    [
        RSASignaturePadding.Pkcs1,
        RSASignaturePadding.Pss,
    ];

    private readonly Signer testee = new();

    [Fact]
    public void AnUntouchedMessageVerifies()
    {
        Assert.True(this.testee.Verify(Message, this.testee.Sign(Message)));
    }

    [Fact]
    public void AChangedMessageDoesNotVerify()
    {
        var signature = this.testee.Sign(Message);

        Assert.False(this.testee.Verify("Ueberweise 900 Franken an Bob", signature));
    }

    [Fact]
    public void AChangedSignatureDoesNotVerify()
    {
        var raw = Convert.FromHexString(this.testee.Sign(Message));
        raw[0] ^= 0x01;

        Assert.False(this.testee.Verify(Message, Convert.ToHexStringLower(raw)));
    }

    [Fact]
    public void ThePublicKeyVerifiesButCannotSign()
    {
        var bytes = Encoding.UTF8.GetBytes(Message);
        var signature = Convert.FromHexString(this.testee.Sign(Message));
        var publicOnly = this.testee.ExportPublicKey();

        Assert.True(
            Hashes.Any(hash =>
                Paddings.Any(padding => publicOnly.VerifyData(bytes, signature, hash, padding))
            )
        );

        Assert.ThrowsAny<CryptographicException>(() =>
            publicOnly.SignData(bytes, Hashes[0], Paddings[0])
        );
    }
}
