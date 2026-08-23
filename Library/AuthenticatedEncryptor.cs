using System.Security.Cryptography;
using System.Text;

namespace Library;

/// <summary>
/// AES in GCM mode. Exercise C2.
/// </summary>
// What this exercise was about.
//
// CBC decrypts the first block as P1 = D(C1) XOR IV. The IV enters the plaintext through
// nothing but that XOR, so flipping a bit in the IV flips exactly that bit of the
// plaintext. No key required; the attacker only has to know what stands at that spot.
// That is how CbcAcceptsAManipulatedCiphertext turns 100 into 900, and CBC hands the
// result over without complaint, because decryption is a total function with nothing to
// check against.
//
// GCM is not stronger encryption. Underneath it is CTR, where a flipped ciphertext bit
// flips the same plaintext bit even more directly. What stops the attack is the tag:
// GHASH over nonce and ciphertext, keyed with a subkey derived from the key, recomputed
// on decryption and compared before any plaintext is handed back. Forging it needs the
// key. Hence the rule: encryption without authentication is incomplete.
public class AuthenticatedEncryptor
{
    private const int NonceLength = 12;
    private const int TagLength = 16;

    private readonly byte[] key = Convert.FromHexString(
        "eb213655ce7ac25591def2aad983e4f0c261ec890577a3f5babeea0670d6d110"
    );

    public string Encrypt(string plaintext)
    {
        var nonce = RandomNumberGenerator.GetBytes(NonceLength);
        var bytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = new byte[bytes.Length];
        var tag = new byte[TagLength];

        using var gcm = new AesGcm(this.key, TagLength);
        gcm.Encrypt(nonce, bytes, ciphertext, tag);

        return Convert.ToHexStringLower([.. nonce, .. ciphertext, .. tag]);
    }

    public string Decrypt(string hex)
    {
        var raw = Convert.FromHexString(hex);
        var nonce = raw[..NonceLength];
        var ciphertext = raw[NonceLength..^TagLength];
        var tag = raw[^TagLength..];
        var plaintext = new byte[ciphertext.Length];

        using var gcm = new AesGcm(this.key, TagLength);

        // Throws AuthenticationTagMismatchException if anything was changed on the way.
        gcm.Decrypt(nonce, ciphertext, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }
}
