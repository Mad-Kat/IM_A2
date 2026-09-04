namespace Library.Tests;

public class MinerTests
{
    private const long Timestamp = 1_700_000_000;

    private static readonly string NoPredecessor = new('0', 64);

    private readonly Miner testee = new();

    [Fact]
    public void TheMinedHashStartsWithTheRequiredZeros()
    {
        var block = SampleBlock();

        var hash = this.testee.Mine(block, 3);

        Assert.StartsWith("000", hash);
        Assert.Equal(hash, block.ComputeHash());
    }

    [Fact]
    public void MiningChangesOnlyTheNonce()
    {
        var block = SampleBlock();

        this.testee.Mine(block, 2);

        Assert.Equal(1, block.Index);
        Assert.Equal(Timestamp, block.Timestamp);
        Assert.Equal(NoPredecessor, block.PreviousHash);
        Assert.Equal("Alice->Bob 0.5", block.Data);
        Assert.NotEqual(0, block.Nonce);
    }

    [Fact]
    public void TheNonceIsCorrect()
    {
        var block = SampleBlock();

        var hash = this.testee.Mine(block, 3);

        Assert.Equal(11204, block.Nonce);
        Assert.Equal("000441d2355037507f5a8c92cf38fd83ac875a862f2eb461170218c192478315", hash);
    }

    [Fact]
    public void AHigherDifficultyNeedsMoreTries()
    {
        var easy = this.AverageNonce(2);
        var hard = this.AverageNonce(3);

        Assert.True(hard > easy * 4, $"expected far more tries, got {easy} against {hard}");
    }

    private static Block SampleBlock(string data = "Alice->Bob 0.5") =>
        new(1, Timestamp, NoPredecessor, data);

    private double AverageNonce(int difficulty)
    {
        const int Blocks = 8;
        long total = 0;
        for (var i = 0; i < Blocks; i++)
        {
            var block = SampleBlock("block " + i);
            this.testee.Mine(block, difficulty);
            total += block.Nonce;
        }

        return total / (double)Blocks;
    }
}
