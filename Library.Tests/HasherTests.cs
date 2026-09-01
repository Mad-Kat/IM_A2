namespace Library.Tests;

public class HasherTests
{
    private readonly Hasher testee = new();

    [Fact]
    public void TheSameInputGivesTheSameHash()
    {
        var first = this.testee.Hash("myplaintext");
        var second = this.testee.Hash("myplaintext");

        Assert.Equal(64, first.Length);
        Assert.Equal(first, second);
        Assert.NotEqual("myplaintext", first);
    }

    [Fact]
    public void TheHashIsCorrect()
    {
        Assert.Equal(
            "8cde7860d9453f6dc628615c0da5e981882878aefbac4d34573e95a17988d73b",
            this.testee.Hash("myplaintext")
        );
    }

    [Fact]
    public void TheOutputLengthDoesNotDependOnTheInputLength()
    {
        Assert.Equal(64, this.testee.Hash("a").Length);
        Assert.Equal(64, this.testee.Hash(new string('a', 100_000)).Length);
    }
}
