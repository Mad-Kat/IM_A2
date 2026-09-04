using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// A key pair that can spend. Exercise B3.
/// </summary>
public class Wallet
{
    private readonly ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP256);

    public string PublicKey => Convert.ToHexStringLower(this.key.ExportSubjectPublicKeyInfo());

    public Transaction Send(string recipient, decimal amount)
    {
        var transaction = new Transaction(this.PublicKey, recipient, amount);
        var bytes = Encoding.UTF8.GetBytes(transaction.Content);

        transaction.Signature = Convert.ToHexStringLower(
            this.key.SignData(bytes, HashAlgorithmName.SHA256)
        );

        return transaction;
    }
}
