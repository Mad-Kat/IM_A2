using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// A chain of blocks. Exercise B4, mining since B5.
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
//
// B5 puts them together. Mine refuses a transaction whose signature does not verify,
// writes the Merkle root of the accepted ones into the block, and searches the nonce.
// IsValid with a difficulty then checks all four things at once: the links from B4,
// the leading zeros from B2, the signatures from B3 and the root over the contents.
// RewritingOneBlockIsNotEnough shows why this is a blockchain and B1 was a list: the
// attacker redoes the work for one block and is still caught by the next one.
//
// What the chain still does not know is whether Alice owns the 0.5 she sends. There
// are no balances here. That is the UTXO set of week 10, or the account state of
// week 11, and it is the difference between a signed message and money.
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

    public Block Mine(IReadOnlyList<Transaction> transactions, int difficulty, long timestamp)
    {
        if (transactions.Any(transaction => !transaction.IsValid()))
        {
            throw new ArgumentException(
                "A transaction is not validly signed.",
                nameof(transactions)
            );
        }

        var root = MerkleRoot(transactions.Select(transaction => transaction.Content).ToList());
        var block = new Block(this.Tip.Index + 1, timestamp, this.Tip.ComputeHash(), root)
        {
            Transactions = transactions,
        };
        new Miner().Mine(block, difficulty);
        this.blocks.Add(block);

        return block;
    }

    public bool IsValid(int difficulty)
    {
        var target = new string('0', difficulty);

        if (!this.IsValid())
        {
            return false;
        }

        foreach (var block in this.blocks.Skip(1))
        {
            if (!block.ComputeHash().StartsWith(target, StringComparison.Ordinal))
            {
                return false;
            }

            if (block.Transactions.Any(transaction => !transaction.IsValid()))
            {
                return false;
            }

            var contents = block.Transactions.Select(transaction => transaction.Content).ToList();
            if (contents.Count > 0 && block.Data != MerkleRoot(contents))
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
