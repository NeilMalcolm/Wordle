using NUnit.Framework;
using Wordle.Lib.Models;

namespace Wordle.Lib.Tests.Tests.Models.WordRowTests;

[TestFixture]
public class WordRow_CompareTo_Tests : BaseWordRowTests
{
    [Test]
    [TestCase(4, "Code")]
    [TestCase(5, "Tests")]
    [TestCase(5, "")]
    [TestCase(5, null)]
    public void WhenCompareToIsCalledWithADifferentWord_ThenCompareToReturnsFalse(int maxRowLength, string text)
    {
        // Arrange
        var wordRow = new WordRow(text, maxRowLength);

        // Act
        var isCorrect = wordRow.CompareTo("Test");

        // Assert
        Assert.IsFalse(isCorrect);
    }
}