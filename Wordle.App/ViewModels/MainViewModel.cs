using System.Windows.Input;
using Wordle.App.Services.WordService;
using Wordle.Lib.Constants;
using Wordle.Lib.Enums;
using Wordle.Lib.Models;

namespace Wordle.App.ViewModels;

public class MainViewModel : BaseViewModel
{
    readonly IWordService _wordService;
    readonly SemaphoreSlim _guessTextSemaphore = new(1, 1);

    bool _isGuessing = false;
    bool _canGuess = true;

    WordChallenge _challenge;
    string _guessText;

    public MainViewModel(IWordService wordService)
    {
        _wordService = wordService;

        SetSecretWord();
        SetCommands();
    }

    public ICommand SubmitGuessCommand { get; private set; }

    public int MaxLength => Defaults.WordRowLength;

    public WordChallenge Challenge
    {
        get => _challenge;
        private set
        {
            _challenge = value;
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
        private set 
        {
            _canGuess = value;
            OnPropertyChanged();
        }
    }

    protected void OnGuessTextChanged(string guessText)
    {
        Task.Run(async () => await GuessTextChanged(guessText));
    }

    public async Task GuessTextChanged(string guessText)
    {
        await _guessTextSemaphore.WaitAsync();

        await Task.Run(() => Challenge.UpdateCurrentGuessText(guessText?.ToUpper()));

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

        if (!Challenge.CanMakeGuessOnCurrentRow)
        {
            FinishSubmitGuess();
            return;
        }

        if (CompareGuessToWord())
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

    bool CompareGuessToWord()
    {
        return Challenge.MakeGuess();
    }

    void SetSecretWord()
    {
        Task.Run(async () =>
        {
            var secretWord = await _wordService.GetWordAsync();
            Challenge = new WordChallenge(secretWord.ToUpper(), 6);
        });
    }

    void SetCommands()
    {
        SubmitGuessCommand = new Command
        (
            async () => await SubmitGuess(),
            () => !_isGuessing
        );
    }

    async Task OnIncorrectGuess()
    {
        GuessText = String.Empty;

        if (Challenge.State == ChallengeState.Failure)
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