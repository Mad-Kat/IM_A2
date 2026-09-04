using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// A signed transfer from one key to a recipient. Exercise B3.
/// </summary>
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

        // TODO: implement
        return false;
    }
}
