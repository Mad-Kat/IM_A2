using System.Diagnostics;
using Xunit.Abstractions;

namespace Library.Tests;

public class MiningEffortTests(ITestOutputHelper output)
{
    private readonly Miner miner = new();

    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void HowManyTriesPerBlock(int difficulty)
    {
        const int Blocks = 10;

        long tries = 0;
        var watch = Stopwatch.StartNew();
        for (var i = 0; i < Blocks; i++)
        {
            var block = new Block(i, 1_700_000_000, new string('0', 64), "block " + i);
            this.miner.Mine(block, difficulty);
            tries += block.Nonce + 1;
        }

        watch.Stop();
        output.WriteLine(
            $"{difficulty} Nullen: {tries / (double)Blocks:N0} Versuche pro Block im Schnitt, {watch.ElapsedMilliseconds} ms fuer {Blocks} Bloecke"
        );
    }
}
