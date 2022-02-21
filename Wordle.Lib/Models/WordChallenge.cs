using Wordle.Lib.Constants;
using Wordle.Lib.Enums;

namespace Wordle.Lib.Models
{
    public class WordChallenge : NotifyPropertyModel
    {
        private readonly string _word;
        private readonly int _rowLength;
        private readonly int _maxGuessAttempts;

        private int _currentGuessAttempt = 0;
        private List<WordRow> _wordRows;
        private ChallengeState _state;

        public WordChallenge(string word, int numRows, int rowLength = Defaults.WordRowLength)
        {
            _word = word;
            _maxGuessAttempts = numRows;
            _rowLength = rowLength;

            WordRows = new List<WordRow>();

            for (var i = 0; i < _maxGuessAttempts; i++)
            {
                WordRows.Add(new WordRow(_rowLength));
            }
        }

        public List<WordRow> WordRows
        {
            get => _wordRows;
            private set => SetProperty(ref _wordRows, value);
        }

        public ChallengeState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public bool CanMakeGuessOnCurrentRow
            => WordRows[_currentGuessAttempt].IsComplete;

        public int MaxLength => _rowLength;

        public bool MakeGuess()
        {
            var result = WordRows[_currentGuessAttempt].MakeGuess(_word);

            if (result)
            {
                State = ChallengeState.Success;
            }

            _currentGuessAttempt++;

            if (_currentGuessAttempt > _maxGuessAttempts)
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
}
