using NUnit.Framework;
using Wordle.Lib.Models;

namespace Wordle.Lib.Tests.Tests.Models.WordRowTests;

[TestFixture]
public class WordRow_IsCorrect_Tests : BaseWordRowTests
{
    [Test]
    [TestCase(4, "")]
    [TestCase(4, null)]
    [TestCase(4, "Test")]
    public void WhenMakeGuessHasNotBeenCalled_ThenIsCorrectIsFalse(int maxRowLength, string text)
    {
        var wordRow = new WordRow(text, maxRowLength);

        Assert.IsFalse(wordRow.IsCorrect);
    }

    [Test]
    [TestCase(5,"Tests", "Tests")]
    [TestCase(6, "Create", "Create")]
    public void WhenMakeGuessIsCalledWithMatchingText_ThenIsCorrectIsTrue(int maxRowLength, string text, string expectedText)
    {
        // Arrange
        var wordRow = new WordRow(text, maxRowLength);

        // Act
        wordRow.MakeGuess(expectedText);

        // Assert
        Assert.IsTrue(wordRow.IsCorrect);
    }

    [Test]
    [TestCase(5,"Tests", "Tests")]
    [TestCase(6, "Create", "Create")]
    public void WhenMakeGuessIsCalledWithMatchingText_ThenMakeGuessReturnsTrue(int maxRowLength, string text, string expectedText)
    {
        // Arrange
        var wordRow = new WordRow(text, maxRowLength);

        // Act/Assert
        Assert.IsTrue(wordRow.MakeGuess(expectedText));
    }


    [Test]
    [TestCase(5, "Tests", "")]
    [TestCase(6, "Create", null)]
    public void WhenMakeGuessIsCalledWithNullOrEmptyText_ThenMakeGuessReturnsFalse(int maxRowLength, string text, string expectedText)
    {
        // Arrange
        var wordRow = new WordRow(text, maxRowLength);

        // Act
        var result = wordRow.MakeGuess(expectedText);

        // Assert
        Assert.IsFalse(result);
    }
}