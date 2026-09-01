using System.Diagnostics;
using System.Security.Cryptography;
using Xunit.Abstractions;

namespace Library.Tests;

public class PasswordSpeedTests(ITestOutputHelper output)
{
    private readonly Hasher hasher = new();

    [Fact]
    public void HowManyPasswordsPerSecondWithPlainSha256()
    {
        const int Count = 1_000_000;

        var watch = Stopwatch.StartNew();
        for (var i = 0; i < Count; i++)
        {
            this.hasher.Hash("password" + i);
        }

        watch.Stop();
        this.Report("SHA-256, blank", Count, watch);
    }

    [Fact]
    public void HowManyPasswordsPerSecondWithPbkdf2()
    {
        const int Count = 100;
        const int Iterations = 100_000;

        var salt = RandomNumberGenerator.GetBytes(16);

        var watch = Stopwatch.StartNew();
        for (var i = 0; i < Count; i++)
        {
            Rfc2898DeriveBytes.Pbkdf2(
                "password" + i,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                32
            );
        }

        watch.Stop();
        this.Report($"PBKDF2, {Iterations:N0} Runden", Count, watch);
    }

    private void Report(string what, int count, Stopwatch watch)
    {
        var perSecond = count / watch.Elapsed.TotalSeconds;
        output.WriteLine(
            $"{what}: {count:N0} Passwoerter in {watch.ElapsedMilliseconds} ms = {perSecond:N0} pro Sekunde"
        );
    }
}
