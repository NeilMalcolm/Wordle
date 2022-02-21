using Wordle.Lib.Enums;

namespace Wordle.Lib.Models;

public class WordCharacter : NotifyPropertyModel
{
    private char _character;
    private CharacterCorrectStatus _characterStatus
        = CharacterCorrectStatus.Unknown;

    public WordCharacter()
    {
    }

    public WordCharacter(char character)
    {
        Character = character;
    }

    public char Character
    {
        get => _character; 
        internal set => SetProperty(ref _character, value);
    }

    public CharacterCorrectStatus CharacterStatus
    {
        get => _characterStatus;
        internal set => SetProperty(ref _characterStatus, value);
    }
}