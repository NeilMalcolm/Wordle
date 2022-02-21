using System.Windows.Input;
using Wordle.App.Services.WordService;
using Wordle.Lib.Constants;
using Wordle.Lib.Models;

namespace Wordle.App.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly IWordService _wordService;
    private readonly SemaphoreSlim _guessTextSemaphore = new(1, 1);

    private string _secretWord;
    private bool _isGuessing = false;
    private int _currentGuessAttempt = 0;
    private int _maxGuessCount = 6;
    private bool _canGuess = true;

    private IList<WordRow> _wordRows;
    private string _guessText;

    public MainViewModel(IWordService wordService)
    {
        _wordService = wordService;

        SetWordRows();
        SetSecretWord();
        SetCommands();
    }

    public ICommand SubmitGuessCommand { get; private set; }
    public int MaxLength => Defaults.WordRowLength;

    public IList<WordRow> WordRows
    {
        get => _wordRows;
        private set
        {
            _wordRows = value;
            OnPropertyChanged();
        }
    }

    public string GuessText
    {
        get => _guessText; 
        set 
        { 
            _guessText = value;
            OnPropertyChanged();
            OnGuessTextChanged(value);
        }
    }

    public bool CanGuess
    {
        get => _canGuess;
        set 
        {
            _canGuess = value;
            OnPropertyChanged();
        }
    }

    void SetWordRows()
    {
        WordRows = new List<WordRow>();

        for (int i = 0; i < _maxGuessCount; i++)
        {
            WordRows.Add(new WordRow(MaxLength));
        }
    }

    void SetSecretWord()
    {
        Task.Run(async () => _secretWord = await _wordService.GetWordAsync());
    }

    void SetCommands()
    {
        SubmitGuessCommand = new Command
        (
            async () => await SubmitGuess(),
            () => !_isGuessing
        );
    }

    protected void OnGuessTextChanged(string guessText)
    {
        Task.Run(async () => await GuessTextChanged(guessText));
    }

    public async Task GuessTextChanged(string guessText)
    {
        await _guessTextSemaphore.WaitAsync();

        WordRows[_currentGuessAttempt].Set(guessText?.ToUpper());

        _guessTextSemaphore.Release();
    }

    public async Task SubmitGuess()
    {
        _isGuessing = true;
        await _guessTextSemaphore.WaitAsync();

        if (!CanGuess)
        {
            FinishSubmitGuess();
            return;
        }

        if (!WordRows[_currentGuessAttempt].IsComplete)
        {
            FinishSubmitGuess();
            return;
        }

        if (CompareGuessToWord(_secretWord.ToUpper(), WordRows[_currentGuessAttempt]))
        {
            await OnCorrectGuess();
        }
        else
        {
            await OnIncorrectGuess();
        }

        FinishSubmitGuess();
    }

    void FinishSubmitGuess()
    {
        _isGuessing = false;
        _guessTextSemaphore.Release();
    }

    bool CompareGuessToWord(string word, WordRow wordRow)
    {
        return wordRow.CompareTo(word);
    }

    async Task OnIncorrectGuess()
    {
        _currentGuessAttempt++;
        GuessText = String.Empty;

        if (_currentGuessAttempt > _maxGuessCount)
        {
            // show failure!
            CanGuess = false;
        }
    }

    async Task OnCorrectGuess()
    {
        // show success!
        CanGuess = false;
    }
}