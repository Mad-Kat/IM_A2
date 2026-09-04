namespace Library.Tests;

public class BlockTests
{
    private const long Timestamp = 1_700_000_000;

    private static readonly string NoPredecessor = new('0', 64);

    [Fact]
    public void TheHashIsSixtyFourHexCharacters()
    {
        var hash = new Block(0, Timestamp, NoPredecessor, "Genesis").ComputeHash();

        Assert.Equal(64, hash.Length);
        Assert.All(hash, c => Assert.Contains(c, "0123456789abcdef"));
    }

    [Fact]
    public void TheHashIsCorrect()
    {
        var block = new Block(1, Timestamp, NoPredecessor, "Alice->Bob 0.5");

        Assert.Equal(
            "d1d7f0331c4c6d8484ad38cf66d66fa6d450ec1c0284d2e67f1dd12afadb7b78",
            block.ComputeHash()
        );
    }

    [Fact]
    public void EveryFieldIsPartOfTheHash()
    {
        var reference = new Block(1, Timestamp, NoPredecessor, "Alice->Bob 0.5").ComputeHash();
        Block[] variants =
        [
            new(2, Timestamp, NoPredecessor, "Alice->Bob 0.5"),
            new(1, Timestamp + 1, NoPredecessor, "Alice->Bob 0.5"),
            new(1, Timestamp, new string('1', 64), "Alice->Bob 0.5"),
            new(1, Timestamp, NoPredecessor, "Alice->Bob 5.0"),
            new(1, Timestamp, NoPredecessor, "Alice->Bob 0.5") { Nonce = 1 },
        ];

        Assert.All(variants, variant => Assert.NotEqual(reference, variant.ComputeHash()));
    }

    [Fact]
    public void ChangingABlockBreaksTheLinkToItsSuccessor()
    {
        var first = new Block(1, Timestamp, NoPredecessor, "Alice->Bob 0.5");
        var second = new Block(2, Timestamp + 600, first.ComputeHash(), "Bob->Carol 0.2");
        Assert.Equal(first.ComputeHash(), second.PreviousHash);

        first.Data = "Alice->Bob 5.0";

        Assert.NotEqual(first.ComputeHash(), second.PreviousHash);
    }
}
