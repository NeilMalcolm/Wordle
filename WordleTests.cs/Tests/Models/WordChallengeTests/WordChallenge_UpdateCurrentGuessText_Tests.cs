using NUnit.Framework;
using Wordle.Lib.Models;

namespace Wordle.Lib.Tests.Tests.Models.WordChallengeTests;

[TestFixture]
public class UpdateCurrentGuessText : BaseWordChallengeTests
{
    [Test]
    [TestCase("Words", "Wordy")]
    public void WhenUpdateCurrentGuessTextIsCalled_ThenCurrentRowWordIsUpdated(string word, string guess)
    {
        // Arrange
        var challenge = new WordChallenge(word, 6);

        //Act
        challenge.UpdateCurrentGuessText(guess);

        // Assert
        Assert.AreEqual(guess, challenge.WordRows[0].Word);
    }
}