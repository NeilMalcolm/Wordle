using Wordle.Lib.Constants;
using Wordle.Lib.Enums;

namespace Wordle.Lib.Models;

public class WordChallenge : NotifyPropertyModel
{
    private readonly string _word;
    private readonly int _rowLength;
    private readonly int _maxGuessAttempts;

    private int _currentGuessAttempt = 0;
    private WordRow[] _wordRows;
    private ChallengeState _state;

    private WordCharacter[] _enteredCharacters;

    public WordChallenge(string word, int numRows, int rowLength = Defaults.WordRowLength)
    {
        _word = word;
        _maxGuessAttempts = numRows;
        _rowLength = rowLength;

        WordRows = new WordRow[_maxGuessAttempts];

        for (var i = 0; i < _maxGuessAttempts; i++)
        {
            WordRows[i] = new WordRow(_rowLength);
        }

        _enteredCharacters = new WordCharacter[_rowLength];
    }

    public bool CanMakeGuessOnCurrentRow => WordRows[_currentGuessAttempt].IsComplete;

    public int MaxLength => _rowLength;

    public WordRow[] WordRows
    {
        get => _wordRows;
        private set => SetProperty(ref _wordRows, value);
    }

    public ChallengeState State
    {
        get => _state;
        set => SetProperty(ref _state, value);
    }

    public WordCharacter[] EnteredCharacters
    {
        get => _enteredCharacters;
        set => SetProperty(ref _enteredCharacters, value);
    }

    public bool MakeGuess()
    {
        var result = WordRows[_currentGuessAttempt].MakeGuess(_word);

        EnteredCharacters = WordRows[_currentGuessAttempt].Characters;

        if (result)
        {
            State = ChallengeState.Success;
            return true;
        }

        _currentGuessAttempt++;

        if (_currentGuessAttempt == _maxGuessAttempts)
        {
            State = ChallengeState.Failure;
            return false;
        }

        State = ChallengeState.Ongoing;
        return result;
    }

    public void UpdateCurrentGuessText(string guessText)
    {
        WordRows[_currentGuessAttempt].Set(guessText);
    }
}