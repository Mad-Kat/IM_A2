using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// A signed transfer from one key to a recipient. Exercise B3.
/// </summary>
// What this exercise was about.
//
// The sender of a transaction is not a name, it is a public key, and the transaction
// carries it. Anyone can check the signature with that key, no registry and no
// authority needed. The signature covers sender, recipient and amount together, so
// changing any of them, or signing with a different key, breaks it. That is
// hash-then-sign from week 5, ECDSA instead of RSA.
//
// The private key never leaves the wallet. Whoever holds it can spend, whoever
// loses it has lost the coins. Bitcoin does exactly this with the curve secp256k1
// and hashes the public key into an address, so the key itself stays hidden until
// the first spend.
public class Transaction
{
    public Transaction(string sender, string recipient, decimal amount)
    {
        this.Sender = sender;
        this.Recipient = recipient;
        this.Amount = amount;
    }

    public string Sender { get; }

    public string Recipient { get; set; }

    public decimal Amount { get; set; }

    public string Signature { get; set; } = "";

    public string Content =>
        string.Create(
            CultureInfo.InvariantCulture,
            $"{this.Sender}|{this.Recipient}|{this.Amount}"
        );

    public bool IsValid()
    {
        var bytes = Encoding.UTF8.GetBytes(this.Content);

        if (this.Signature.Length == 0)
        {
            return false;
        }

        using var publicKey = ECDsa.Create();
        publicKey.ImportSubjectPublicKeyInfo(Convert.FromHexString(this.Sender), out _);

        return publicKey.VerifyData(
            bytes,
            Convert.FromHexString(this.Signature),
            HashAlgorithmName.SHA256
        );
    }
}
