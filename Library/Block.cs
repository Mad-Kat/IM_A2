using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// One block of a chain. Exercise B1.
/// </summary>
// What this exercise was about.
//
// The hash is the fingerprint of the whole block: its position, its time, its data, its
// nonce and the fingerprint of the block before it. Change one character anywhere and
// the fingerprint changes completely, that is the avalanche effect from week 6 applied
// to a list. The nonce stays at zero for now, B2 gives it a job.
//
// ChangingABlockBreaksTheLinkToItsSuccessor is the point. The next block still carries
// the old fingerprint, so a tampered block is visible from the block after it, and
// rehashing the tampered block does not help, the successor keeps pointing at the old
// value. To hide the change you would have to rewrite every block that follows. Nothing
// in this class makes that expensive yet. Week 9 does, and that is what turns a linked
// list into a blockchain.
public class Block
{
    public Block(int index, long timestamp, string previousHash, string data)
    {
        this.Index = index;
        this.Timestamp = timestamp;
        this.PreviousHash = previousHash;
        this.Data = data;
    }

    public int Index { get; }

    public long Timestamp { get; }

    public string PreviousHash { get; }

    public long Nonce { get; set; }

    public string Data { get; set; }

    public string ComputeHash()
    {
        var content = $"{this.Index}|{this.Timestamp}|{this.PreviousHash}|{this.Nonce}|{this.Data}";
        var bytes = Encoding.UTF8.GetBytes(content);

        return Convert.ToHexStringLower(SHA256.HashData(bytes));
    }
}
