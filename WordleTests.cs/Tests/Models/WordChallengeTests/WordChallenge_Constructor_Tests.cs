using NUnit.Framework;
using Wordle.Lib.Models;

namespace Wordle.Lib.Tests.Tests.Models.WordChallengeTests;

[TestFixture]
public class WordChallenge_Constructor_Tests : BaseWordChallengeTests
{
    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    [TestCase(7)]
    public void WhenWordChallengeIsInitialized_ThenWordRowsLengthIsEqualToNumRowsParameter(int numRows)
    {
        var challenge = new WordChallenge("Crest", numRows);

        // Assert
        Assert.AreEqual(numRows, challenge.WordRows.Length);
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    [TestCase(7)]
    public void WhenWordChallengeIsInitialized_ThenEachWordRowCharactersArrayLengthIsEqualToRowLengthParameter(int rowLength)
    {
        var challenge = new WordChallenge("Crest", 6, rowLength);

        // Assert
        foreach (var wordRow in challenge.WordRows)
        {
            Assert.AreEqual(rowLength, wordRow.Characters.Length);
        }
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    [TestCase(7)]
    public void WhenWordChallengeIsInitialized_ThenEnteredCharactersArrayLengthIsEqualToRowLengthParameter(int rowLength)
    {
        var challenge = new WordChallenge("Crest", 6, rowLength);

        // Assert
        Assert.AreEqual(rowLength, challenge.EnteredCharacters.Length);
    }
}