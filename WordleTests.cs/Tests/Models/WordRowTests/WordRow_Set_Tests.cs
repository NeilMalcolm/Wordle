using NUnit.Framework;
using System;
using System.Linq;
using Wordle.Lib.Models;

namespace Wordle.Lib.Tests.Tests.Models.WordRowTests;

[TestFixture]
public class WordRow_Set_Tests : BaseWordRowTests
{
    [Test]
    [TestCaseSource(nameof(GetValidSetTestData))]
    public void WhenSetIsCalledWithValidText_ThenWordPropertyIsSet(int maxLength, string word)
    {
        // Arrange
        var wordRow = new WordRow(maxLength);

        // Act
        wordRow.Set(word);

        // Assert
        Assert.AreEqual(wordRow.Word, word);
    }

    [Test]
    [TestCaseSource(nameof(GetValidSetTestData))]
    public void WhenSetIsCalledWithValidText_ThenCharactersPropertyIsSet(int maxLength, string word)
    {
        // Arrange
        var wordRow = new WordRow(maxLength);

        // Act
        wordRow.Set(word);

        // Assert
        var expectedChars = word.ToCharArray();
        for (int i = 0; i < expectedChars.Length; i++)
        {
            char expectedChar = expectedChars[i];
            Assert.AreEqual(expectedChar, wordRow.Characters[i].Character);
        }
    }

    [Test]
    [TestCaseSource(nameof(GetEmptySetTestData))]
    public void WhenSetIsCalledWithNullOrEmptyText_ThenWordPropertyIsEmptyString(int maxLength, string word)
    {
        // Arrange
        var wordRow = GetWordRowWithText(maxLength, "test");

        // Act
        wordRow.Set(word);

        // Assert
        Assert.AreEqual(string.Empty, wordRow.Word);
    }
    
    [Test]
    [TestCaseSource(nameof(GetEmptySetTestData))]
    public void WhenSetIsCalledWithNullOrEmptyText_ThenCharactersPropertyIsEmptyCharArrayWithlengthEqualToMaxLength(int maxLength, string word)
    {
        // Arrange
        var wordRow = GetWordRowWithText(maxLength, "test");

        // Act
        wordRow.Set(word);

        // Assert
        Assert.IsNotNull(wordRow.Characters);
        Assert.AreEqual(maxLength, wordRow.Characters.Length);
        Assert.That(wordRow.Characters.All(x => x.Character == default && x.CharacterStatus == Enums.CharacterCorrectStatus.Unknown));
    }

    [Test]
    [TestCase(5, "This is too long")]
    public void WhenSetIsCalledWithTextThatExceedsMaxLength_ThenArgumentOutOfRangeExceptionIsThrown(int maxLength, string word)
    {
        // Arrange
        var wordRow = new WordRow(maxLength);

        // Act/Assert
        Assert.Throws(typeof(ArgumentOutOfRangeException), () => wordRow.Set(word));
    }
}
