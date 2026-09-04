namespace Library.Tests;

public class TransactionTests
{
    private readonly Wallet alice = new();

    private readonly Wallet mallory = new();

    [Fact]
    public void ASignedTransactionIsValid()
    {
        var transaction = this.alice.Send("Bob", 0.5m);

        Assert.Equal(this.alice.PublicKey, transaction.Sender);
        Assert.NotEmpty(transaction.Signature);
        Assert.True(transaction.IsValid());
    }

    [Fact]
    public void AChangedAmountInvalidatesTheSignature()
    {
        var transaction = this.alice.Send("Bob", 0.5m);
        Assert.True(transaction.IsValid());

        transaction.Amount = 5.0m;

        Assert.False(transaction.IsValid());
    }

    [Fact]
    public void AChangedRecipientInvalidatesTheSignature()
    {
        var transaction = this.alice.Send("Bob", 0.5m);
        Assert.True(transaction.IsValid());

        transaction.Recipient = "Mallory";

        Assert.False(transaction.IsValid());
    }

    [Fact]
    public void OnlyTheOwnerOfTheKeyCanSign()
    {
        var genuine = this.alice.Send("Bob", 0.5m);
        Assert.True(genuine.IsValid());

        var forged = new Transaction(this.alice.PublicKey, "Mallory", 0.5m)
        {
            Signature = this.mallory.Send("Mallory", 0.5m).Signature,
        };

        Assert.False(forged.IsValid());
    }
}
