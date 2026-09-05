namespace Library.Tests;

public class MinedChainTests
{
    private const long Timestamp = 1_700_000_000;

    private const int Difficulty = 2;

    private readonly Wallet alice = new();

    private readonly Wallet bob = new();

    private readonly Wallet mallory = new();

    [Fact]
    public void AMinedBlockCarriesItsTransactionsAndTheWork()
    {
        var chain = new Chain();
        Transaction[] transactions = [this.alice.Send("Bob", 0.5m), this.bob.Send("Carol", 0.2m)];

        var block = chain.Mine(transactions, Difficulty, Timestamp);

        Assert.Equal(transactions, block.Transactions);
        Assert.Equal(
            Chain.MerkleRoot([transactions[0].Content, transactions[1].Content]),
            block.Data
        );
        Assert.StartsWith("00", block.ComputeHash());
        Assert.True(chain.IsValid(Difficulty));
    }

    [Fact]
    public void AChangedAmountBreaksTheChain()
    {
        var chain = new Chain();
        var payment = this.alice.Send("Bob", 0.5m);
        chain.Mine([payment], Difficulty, Timestamp);
        chain.Mine([this.bob.Send("Carol", 0.2m)], Difficulty, Timestamp + 600);
        Assert.True(chain.IsValid(Difficulty));

        payment.Amount = 5.0m;

        Assert.False(chain.IsValid(Difficulty));
    }

    [Fact]
    public void TheMinerRefusesAForgedTransaction()
    {
        var chain = new Chain();
        var forged = new Transaction(this.alice.PublicKey, "Mallory", 0.5m)
        {
            Signature = this.mallory.Send("Mallory", 0.5m).Signature,
        };

        Assert.ThrowsAny<ArgumentException>(() => chain.Mine([forged], Difficulty, Timestamp));
        Assert.Single(chain.Blocks);
    }

    [Fact]
    public void RewritingOneBlockIsNotEnough()
    {
        var chain = new Chain();
        var first = chain.Mine([this.alice.Send("Bob", 0.5m)], Difficulty, Timestamp);
        chain.Mine([this.bob.Send("Carol", 0.2m)], Difficulty, Timestamp + 600);
        chain.Mine([this.alice.Send("Dave", 0.1m)], Difficulty, Timestamp + 1200);
        Assert.True(chain.IsValid(Difficulty));

        first.Data = "Alice->Bob 5.0";
        new Miner().Mine(first, Difficulty);

        Assert.StartsWith("00", first.ComputeHash());
        Assert.False(chain.IsValid(Difficulty));
    }
}
