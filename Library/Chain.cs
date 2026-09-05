using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// A chain of blocks. Exercise B4.
/// </summary>
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
        // TODO: implement
        return false;
    }

    public static string MerkleRoot(IReadOnlyList<string> transactions)
    {
        var level = transactions.Select(Hash).ToList();

        // TODO: implement
        return "";
    }

    private static string Hash(string text) =>
        Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(text)));
}
