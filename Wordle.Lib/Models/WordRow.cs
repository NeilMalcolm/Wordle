﻿using Wordle.Lib.Constants;

namespace Wordle.Lib.Models;

public class WordRow : NotifyPropertyModel
{
    private int _maxLength;

    private string _word = string.Empty;
    private WordCharacter[] _characters = new WordCharacter[0];

    public WordRow(int rowLength)
    {
        if (rowLength <= 0)
        {
            throw new ArgumentException("Parameter must be a value greater than zero.", nameof(rowLength));
        }

        if (rowLength > 10)
        {
            throw new ArgumentOutOfRangeException("Parameter must not be greater than 10.", nameof(rowLength));
        }

        MaxLength = rowLength;
    }

    public WordRow(string text, int rowLength = Defaults.WordRowLength)
        : this(rowLength)
    {
        if ((text ??= string.Empty).Length > rowLength)
        {
            throw new ArgumentException($"Parameter {nameof(text)} length must not exceed {nameof(rowLength)} of {rowLength}", nameof(text));
        }

        SetWordAndCharactersArray(text);
    }

    public WordRow()
        : this(Defaults.WordRowLength)
    { }

    public int MaxLength
    {
        get => _maxLength;
        private set
        {
            _maxLength = value;
            SetWordAndCharactersArray(string.Empty);
        }
    }

    public string Word
    {
        get => _word;
        private set => SetProperty(ref _word, value);
    }

    public WordCharacter[] Characters
    {
        get => _characters;
        private set => SetProperty(ref _characters, value);
    }

    public bool IsCorrect => 
        Characters?.All(x => x.CharacterStatus == Enums.CharacterCorrectStatus.CorrectPosition) ?? false;

    public bool IsComplete 
        => (_word?.Length ?? 0) == _maxLength;

    public bool HasMadeGuess { get; private set; }

    public void Set(string text)
    {
        if (text is null || text.Length == 0)
        {
            ClearAll();
            return;
        }

        SetWordAndCharactersArray(text);
    }

    public bool MakeGuess(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return false;
        }

        for (int i = 0; i < word.Length; i++)
        {
            var wordChar = Characters[i];

            if (word[i] == wordChar.Character)
            {
                wordChar.CharacterStatus = Enums.CharacterCorrectStatus.CorrectPosition;
            }
            else if (word.Contains(wordChar.Character))
            {
                wordChar.CharacterStatus = Enums.CharacterCorrectStatus.IncorrectPosition;
            }
            else
            {
                wordChar.CharacterStatus = Enums.CharacterCorrectStatus.NotInWord;
            }
        }

        HasMadeGuess = true;

        return IsCorrect;
    }

    #region Private Methods

    void SetWordAndCharactersArray(string text)
    {
        if (text.Length > _maxLength)
        {
            throw new ArgumentOutOfRangeException($"Parameter {nameof(text)} must not exceed length of {_maxLength}.", nameof(text));
        }

        Word = text;
        
        var newCharacters = new WordCharacter[_maxLength];
        var textLength = string.IsNullOrEmpty(text) ? 0 : text.Length;

        for (int i = 0; i < Characters.Length; i++)
        {
            newCharacters[i] = i < textLength
                ? new WordCharacter(text[i]) 
                : new WordCharacter();
        }

        Characters = newCharacters;
    }

    void ClearAll()
    {
        SetWordAndCharactersArray(string.Empty);
    }

    #endregion

}
