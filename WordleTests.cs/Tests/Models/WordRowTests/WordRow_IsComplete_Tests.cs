using NUnit.Framework;
using Wordle.Lib.Models;

namespace Wordle.Lib.Tests.Tests.Models.WordRowTests;

[TestFixture]
public class WordRow_IsComplete_Tests : BaseWordRowTests
{
    [Test]
    public void WhenTextIsZeroLengthString_ThenIsCompleteIsFalse()
    {
        // Arrange/Act
        var wordRow = new WordRow(3);

        // Assert
        Assert.IsFalse(wordRow.IsComplete);
    }

    [Test]
    public void WhenWordRowIsEmpty_ThenIsCompleteIsFalse()
    {
        // Arrange/Act
        var wordRow = new WordRow();

        // Assert
        Assert.IsFalse(wordRow.IsComplete);
    }

    [Test]
    [TestCase(5, "")]
    [TestCase(5, null)]
    [TestCase(5, "T")]
    [TestCase(5, "Te")]
    [TestCase(5, "Tes")]
    [TestCase(5, "Test")]
    public void WhenWordRowTextIsShorterThanMaxRowLength_ThenIsCompleteIsFalse(int maxRowLength, string text)
    {
        // Arrange/Act
        var wordRow = new WordRow(text, maxRowLength);

        // Assert
        Assert.IsFalse(wordRow.IsComplete);
    }

    [Test]
    [TestCase(1, "T")]
    [TestCase(2, "Te")]
    [TestCase(3, "Tes")]
    [TestCase(4, "Test")]
    [TestCase(5, "Tests")]
    [TestCase(8, "Complete")]
    public void WhenWordRowTextLengthIsSameAsThanMaxRowLength_ThenIsCompleteIsTrue(int maxRowLength, string text)
    {
        // Arrange/Act
        var wordRow = new WordRow(text, maxRowLength);

        // Assert
        Assert.IsTrue(wordRow.IsComplete);
    }
}