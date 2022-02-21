using System.Collections.Generic;
using Wordle.Lib.Models;

namespace Wordle.Lib.Tests.Tests.Models.WordRowTests;

public abstract class BaseWordRowTests
{
    #region Test Data 

    public static IEnumerable<object[]> GetValidSetTestData()
    {
        yield return new object[] { 5, "T" };
        yield return new object[] { 5, "Te" };
        yield return new object[] { 5, "Tes" };
        yield return new object[] { 5, "Test" };
        yield return new object[] { 5, "Tests" };
    }

    public static IEnumerable<object?[]> GetEmptySetTestData()
    {
        yield return new object?[] { 5, "" };
        yield return new object?[] { 5, null };
    }

    #endregion

    protected WordRow GetWordRowWithText(int maxLength, string text)
    {
        var wordRow = new WordRow(maxLength);
        wordRow.Set(text);
        return wordRow;
    }
}
