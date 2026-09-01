using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// RSA signatures. Exercise C4.
/// </summary>
// What this exercise was about.
//
// ThePublicKeyVerifiesButCannotSign is the whole point. The public half carries the
// modulus and the public exponent, and that is enough to check a signature but not to
// produce one, which needs the private exponent d. Signing therefore uses your own
// private key and verifying uses the signer's public key, the exact opposite of
// encryption, where you take the recipient's public key.
//
// Get the two the wrong way round and you do not end up with a weak signature, you end
// up with an exception. In block 2 this same operation, on an elliptic curve instead of
// RSA, is what a Bitcoin transaction is.
public class Signer
{
    private readonly RSA keyPair = RSA.Create(2048);

    public string Sign(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var signature = this.keyPair.SignData(
            bytes,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );

        return Convert.ToHexStringLower(signature);
    }

    public bool Verify(string message, string signatureHex)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var signature = Convert.FromHexString(signatureHex);

        return this.keyPair.VerifyData(
            bytes,
            signature,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1
        );
    }

    public RSA ExportPublicKey()
    {
        var publicOnly = RSA.Create();
        publicOnly.ImportRSAPublicKey(this.keyPair.ExportRSAPublicKey(), out _);

        return publicOnly;
    }
}
