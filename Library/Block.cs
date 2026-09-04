using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// One block of a chain. Exercise B1.
/// </summary>
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

        // TODO: implement
        byte[] hash = [];

        return Convert.ToHexStringLower(hash);
    }
}
