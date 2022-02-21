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
    public void WhenCompareToHasNotBeenCalled_ThenIsCorrectIsFalse(int maxRowLength, string text)
    {
        var wordRow = new WordRow(text, maxRowLength);

        Assert.IsFalse(wordRow.IsCorrect);
    }

    [Test]
    [TestCase(5,"Tests", "Tests")]
    [TestCase(6, "Create", "Create")]
    public void WhenCompareToIsCalledWithMatchingText_ThenIsCorrectIsTrue(int maxRowLength, string text, string expectedText)
    {
        // Arrange
        var wordRow = new WordRow(text, maxRowLength);

        // Act
        wordRow.CompareTo(expectedText);

        // Assert
        Assert.IsTrue(wordRow.IsCorrect);
    }

    [Test]
    [TestCase(5,"Tests", "Tests")]
    [TestCase(6, "Create", "Create")]
    public void WhenCompareToIsCalledWithMatchingText_ThenCompareToReturnsTrue(int maxRowLength, string text, string expectedText)
    {
        // Arrange
        var wordRow = new WordRow(text, maxRowLength);

        // Act/Assert
        Assert.IsTrue(wordRow.CompareTo(expectedText));
    }


    [Test]
    [TestCase(5, "Tests", "")]
    [TestCase(6, "Create", null)]
    public void WhenCompareToIsCalledWithNullOrEmptyText_ThenCompareToReturnsFalse(int maxRowLength, string text, string expectedText)
    {
        // Arrange
        var wordRow = new WordRow(text, maxRowLength);

        // Act
        var result = wordRow.CompareTo(expectedText);

        // Assert
        Assert.IsFalse(result);
    }
}