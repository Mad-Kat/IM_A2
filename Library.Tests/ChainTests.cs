namespace Library.Tests;

public class ChainTests
{
    private const long Timestamp = 1_700_000_000;

    private static readonly string[] Transactions =
    [
        "Alice->Bob 0.5",
        "Bob->Carol 0.2",
        "Carol->Dave 1.0",
        "Dave->Alice 0.3",
    ];

    [Fact]
    public void AFreshChainIsValid()
    {
        var chain = new Chain();

        Assert.Single(chain.Blocks);
        Assert.True(chain.IsValid());
    }

    [Fact]
    public void AppendedBlocksLinkToTheTip()
    {
        var chain = new Chain();

        var first = chain.Append("Alice->Bob 0.5", Timestamp);
        var second = chain.Append("Bob->Carol 0.2", Timestamp + 600);

        Assert.Equal(first.ComputeHash(), second.PreviousHash);
        Assert.Equal(2, second.Index);
        Assert.True(chain.IsValid());
    }

    [Fact]
    public void ChangingAnOldBlockInvalidatesTheChain()
    {
        var chain = new Chain();
        var first = chain.Append("Alice->Bob 0.5", Timestamp);
        chain.Append("Bob->Carol 0.2", Timestamp + 600);
        chain.Append("Carol->Dave 1.0", Timestamp + 1200);
        Assert.True(chain.IsValid());

        first.Data = "Alice->Bob 5.0";

        Assert.False(chain.IsValid());
    }

    [Fact]
    public void TheMerkleRootIsCorrect()
    {
        Assert.Equal(
            "15304f56ef75d25a14b51a1c05f520f8c9571555616e4d9190fdd7f4f81b16aa",
            Chain.MerkleRoot(Transactions)
        );
    }

    [Fact]
    public void AChangedTransactionChangesTheRoot()
    {
        var changed = Transactions.ToArray();
        changed[2] = "Carol->Dave 10.0";

        Assert.NotEmpty(Chain.MerkleRoot(Transactions));
        Assert.NotEqual(Chain.MerkleRoot(Transactions), Chain.MerkleRoot(changed));
    }

    [Fact]
    public void AnOddNumberOfTransactionsDuplicatesTheLast()
    {
        var three = Transactions[..3];
        string[] four = [.. three, three[^1]];

        Assert.NotEmpty(Chain.MerkleRoot(three));
        Assert.Equal(Chain.MerkleRoot(four), Chain.MerkleRoot(three));
    }
}
