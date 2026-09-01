using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// RSA signatures. Exercise C4.
/// </summary>
public class Signer
{
    private readonly RSA keyPair = RSA.Create(2048);

    public string Sign(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);

        // TODO: implement
        byte[] signature = [];

        return Convert.ToHexStringLower(signature);
    }

    public bool Verify(string message, string signatureHex)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var signature = Convert.FromHexString(signatureHex);

        // TODO: implement
        return false;
    }

    public RSA ExportPublicKey()
    {
        var publicOnly = RSA.Create();
        publicOnly.ImportRSAPublicKey(this.keyPair.ExportRSAPublicKey(), out _);

        return publicOnly;
    }
}
