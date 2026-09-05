using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// A chain of blocks. Exercise B4.
/// </summary>
// What this exercise was about.
//
// A block has no stored hash, and that is deliberate. IsValid recomputes every hash
// from the data and compares it with what the next block claims as its predecessor.
// A stored hash could be edited along with the data, a recomputed one cannot. The
// tampered block itself still looks fine, the break shows up one block later.
//
// The Merkle root is the same idea one level down. Hash the transactions, hash the
// pairs, keep going until one value is left, and put that into the block. Any change
// to any transaction changes the root and therefore the block hash and therefore
// every block after it. Pairwise instead of all at once, because a proof for a single
// transaction then needs only one hash per level, week 8.
//
// What is still missing is the part that makes rewriting expensive: nothing here
// checks the leading zeros from B2, and nothing checks the signatures from B3. B5
// puts the pieces together.
public class Chain
{
    private readonly List<Block> blocks = [new Block(0, 0, new string('0', 64), "Genesis")];

    public IReadOnlyList<Block> Blocks => this.blocks;

    public Block Tip => this.blocks[^1];

    public Block Append(string data, long timestamp)
    {
        var block = new Block(this.Tip.Index + 1, timestamp, this.Tip.ComputeHash(), data);
        this.blocks.Add(block);

        return block;
    }

    public bool IsValid()
    {
        for (var i = 1; i < this.blocks.Count; i++)
        {
            var block = this.blocks[i];
            if (block.Index != i || block.PreviousHash != this.blocks[i - 1].ComputeHash())
            {
                return false;
            }
        }

        return true;
    }

    public static string MerkleRoot(IReadOnlyList<string> transactions)
    {
        var level = transactions.Select(Hash).ToList();

        while (level.Count > 1)
        {
            if (level.Count % 2 == 1)
            {
                level.Add(level[^1]);
            }

            level = level.Chunk(2).Select(pair => Hash(pair[0] + pair[1])).ToList();
        }

        return level[0];
    }

    private static string Hash(string text) =>
        Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(text)));
}
