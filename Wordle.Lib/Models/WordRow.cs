using Wordle.Lib.Constants;

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
            throw new ArgumentException($"Parameter {nameof(rowLength)} must not exceed {nameof(rowLength)} of {rowLength}", nameof(rowLength));
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

    public void Set(string text)
    {
        if (text is null || text.Length == 0)
        {
            ClearAll();
            return;
        }

        if (text.Length > _maxLength)
        {
            // if word is beyond length limit do nothing.
            return;
        }

        SetWordAndCharactersArray(text);
    }

    public bool CompareTo(string word)
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

        return IsCorrect;
    }

    #region Private Methods

    void SetWordAndCharactersArray(string text)
    {
        Word = text;
        
        Characters = new WordCharacter[_maxLength];

        if (!string.IsNullOrWhiteSpace(text))
        {
            Array.Copy(text.Select(c => new WordCharacter(c)).ToArray(), _characters, text.Length);
        }
        else
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i] = new WordCharacter();
            }
        }
    }

    void ClearAll()
    {
        SetWordAndCharactersArray(string.Empty);
    }

    #endregion

}
