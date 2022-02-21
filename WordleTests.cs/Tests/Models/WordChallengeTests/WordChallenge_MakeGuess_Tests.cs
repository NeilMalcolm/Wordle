using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Wordle.Lib.Enums;
using Wordle.Lib.Models;

namespace Wordle.Lib.Tests.Tests.Models.WordChallengeTests;

[TestFixture]
public class WordChallenge_MakeGuess_Tests : BaseWordChallengeTests
{
    public static IEnumerable<object[]> GetUpdateCurrentGuessTextData()
    {
        yield return new object[]
        {
            "Crest", 6, new string[] { "Crewe", "Crabs", "Crews", "Creme", "Crepe", "Crest" }
        };
    }

    [Test]
    [TestCase("Crest", "Crest", 1, ChallengeState.Success)]
    [TestCase("Right", "Right", 5, ChallengeState.Success)]
    [TestCase("Crest", "Tried", 2, ChallengeState.Ongoing)]
    [TestCase("Crest", "Catch", 1, ChallengeState.Failure)]
    public void WhenMakeGuessIsCalled_ThenStateIsSetAppropriately(string word, string guess, int numGuesses, ChallengeState expectedState)
    {
        // Assert
        var challenge = new WordChallenge(word, numGuesses);

        // Act
        challenge.UpdateCurrentGuessText(guess);
        challenge.MakeGuess();

        // Assert
        Assert.AreEqual(expectedState, challenge.State);
    }

    [Test]
    [TestCase("Crest", "Crest", 1, true)]
    [TestCase("Crest", "Tried", 2, false)]
    [TestCase("Crest", "Catch", 1, false)]
    public void WhenMakeGuessIsCalled_ThenWordRowIsCorrectIsSetAppropriately(string word, string guess, int numGuesses, bool expectedCorrectValue)
    {
        // Assert
        var challenge = new WordChallenge(word, numGuesses);

        // Act
        challenge.UpdateCurrentGuessText(guess);
        challenge.MakeGuess();

        // Assert
        Assert.AreEqual(expectedCorrectValue, challenge.WordRows[0].IsCorrect);
    }

    [Test]
    [TestCase("Crest", "Crest", 1)]
    [TestCase("Crest", "Tried", 2)]
    [TestCase("Crest", "Catch", 1)]
    public void WhenMakeGuessIsCalled_ThenExtendedCharactersAreSetToLatestGuess(string word, string guess, int numGuesses)
    {
        // Arrange
        var challenge = new WordChallenge(word, numGuesses);

        // Act
        challenge.UpdateCurrentGuessText(guess);
        challenge.MakeGuess();

        // Assert
        Assert.AreEqual(challenge.EnteredCharacters, challenge.WordRows[0].Characters);
    }

    [Test]
    [TestCaseSource(nameof(GetUpdateCurrentGuessTextData))]
    public void WhenMakeGuessIsCalledInSuccession_ThenRowsAreComplete(string word, int maxNumGuesses, string[] guesses)
    {
        // Arrange
        var challenge = new WordChallenge(word, maxNumGuesses);

        // Act
        foreach (var guess in guesses)
        {
            challenge.UpdateCurrentGuessText(guess);
            challenge.MakeGuess();
        }

        // Assert
        Assert.IsTrue(challenge.WordRows.All(x => x.IsComplete));
    }

    [Test]
    [TestCaseSource(nameof(GetUpdateCurrentGuessTextData))]
    public void WhenMakeGuessIsCalledInSuccession_ThenLastGuessIsCorrect(string word, int maxNumGuesses, string[] guesses)
    {
        // Arrange
        var challenge = new WordChallenge(word, maxNumGuesses);

        // Act
        foreach (var guess in guesses)
        {
            challenge.UpdateCurrentGuessText(guess);
            challenge.MakeGuess();
        }

        // Assert
        var lastGuess = challenge.WordRows.Last();
        var otherGuesses = challenge.WordRows.Where(x => x != lastGuess);
        Assert.That(otherGuesses.All(x => x.IsCorrect == false));
        Assert.IsTrue(lastGuess.IsCorrect);
    }
}