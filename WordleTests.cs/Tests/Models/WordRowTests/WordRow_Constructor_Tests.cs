using NUnit.Framework;
using Wordle.Lib.Models;
using Wordle.Lib.Constants;
using System;

namespace Wordle.Lib.Tests.Tests.Models.WordRowTests;

[TestFixture]
public class WordRow_Constructor_Tests : BaseWordRowTests
{
    [Test]
    public void WhenInitializedWithDefaultConstructor_ThenMaxLengthIsEqualToConstantsValue()
    {
        // Act
        var wordRow = new WordRow();

        // Assert
        Assert.AreEqual(Defaults.WordRowLength, wordRow.MaxLength);
    }

    [Test]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(10)]
    public void WhenInitializedWithIntegerParameter_ThenMaxLengthIsEqualToPassedInValue(int length)
    {
        // Act
        var wordRow = new WordRow(length);
            
        // Assert
        Assert.AreEqual(length, wordRow.MaxLength);
    }
    
    [Test]
    [TestCase(11)]
    [TestCase(20)]
    [TestCase(int.MaxValue)]
    public void WhenInitializedWithIntegerParameterLargerThanMaximum_ThenArgumentOutOfRangeExceptionIsThrown(int length)
    {
        // Assert
        Assert.Throws(typeof(ArgumentOutOfRangeException), () => new WordRow(length));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void WhenInitializedWithZeroOrNegativeIntegerParameter_ThenArgumentExceptionIsThrown(int length)
    {
        // Act
        // Assert
        Assert.Throws(typeof(ArgumentException), () => new WordRow(length));
    }

    [Test]
    [TestCase(5, "Chart")]
    public void WhenInitializedWithTextParameter_ThenWordPropertyIsSet(int maxLength, string text)
    {
        // Act
        var wordRow = new WordRow(text, maxLength);

        // Assert
        Assert.AreEqual(text, wordRow.Word);
    }

    [Test]
    [TestCase(5, "Chart")]
    public void WhenInitializedWithTextParameter_ThenCharactersPropertyIsSet(int maxLength, string text)
    {
        // Act
        var wordRow = new WordRow(text, maxLength);

        // Assert
        var expectedChars = text.ToCharArray();
        for (int i = 0; i < expectedChars.Length; i++)
        {
            char expectedChar = expectedChars[i];
            Assert.AreEqual(expectedChar, wordRow.Characters[i].Character);
        }
    }

    [Test]
    [TestCase(1, "Failure Example")]
    [TestCase(Defaults.WordRowLength, "Failure")]
    public void WhenInitializedWithTextParameter_AndTextLengthExceedsMaxLength_ThenArgumentExceptionIsThrown(int maxLength, string text)
    {
        // Act
        // Assert
        Assert.Throws(typeof(ArgumentException), () => new WordRow(text, maxLength));
    }
}