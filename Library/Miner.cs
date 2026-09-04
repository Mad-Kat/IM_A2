namespace Library;

/// <summary>
/// Proof of work. Exercise B2.
/// </summary>
// What this exercise was about.
//
// There is no clever way to find a hash that starts with zeros. A hash cannot be run
// backwards, so the only option is to change the nonce and try again, on average 16
// times more often for every extra zero. MiningEffortTests puts numbers on it, and it
// also shows the spread: one block is found after a handful of tries, the next one
// takes three times the average. That is why Bitcoin needs two thousand blocks to
// judge whether the difficulty is right.
//
// This loop is what B1 was missing. Rewriting a block now costs the same search again,
// for that block and for every block after it, and the honest network keeps adding
// blocks at the tip while the attacker is still busy. Week 9 does the arithmetic.
public class Miner
{
    public string Mine(Block block, int difficulty)
    {
        var target = new string('0', difficulty);

        var hash = block.ComputeHash();
        while (!hash.StartsWith(target, StringComparison.Ordinal))
        {
            block.Nonce++;
            hash = block.ComputeHash();
        }

        return hash;
    }
}
