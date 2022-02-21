using Wordle.Lib.Enums;

namespace Wordle.App.Views;

public partial class WordRowLetterView : Label
{
	private static readonly Color _incorrectPlacementColor = Color.FromHex("f5d142");
	private static readonly Color _correctPlacementColor = Color.FromHex("4dbd51");
	private static readonly Color _notInWordColor = Color.FromHex("6b6b6b");
	private static readonly Color _notInWordTextColor = Colors.White;
	private static readonly Color _unknownTextColor = Color.FromHex("212121");
	private static readonly Color _incorrectPlacementTextColor = Colors.White;
	private static readonly Color _correctPlacementTextColor = Colors.White;

	public static readonly BindableProperty CharacterStatusProperty = BindableProperty.Create
	(
		propertyName: nameof(CharacterStatus),
		returnType: typeof(CharacterCorrectStatus),
		declaringType: typeof(WordRowLetterView),
		defaultValue: CharacterCorrectStatus.Unknown,
		defaultBindingMode: BindingMode.OneWay,
		propertyChanged: OnIsCorrectPropertyChanged
	);

	public WordRowLetterView()
	{
		InitializeComponent();

		BackgroundColor = Colors.LightGrey;
	}

	public bool CharacterStatus
	{
		get => (bool)GetValue(CharacterStatusProperty);
		set => SetValue(CharacterStatusProperty, value);
	}

	protected static void OnIsCorrectPropertyChanged(BindableObject sender, object oldValue, object newValue)
	{
		if (sender is WordRowLetterView letterView 
			&& newValue is CharacterCorrectStatus status)
        {
			Color bgColor;
			Color textColor;

			switch (status)
            {
				case CharacterCorrectStatus.CorrectPosition:
					textColor = _correctPlacementTextColor;
					bgColor = _correctPlacementColor;
					break;
				case CharacterCorrectStatus.IncorrectPosition:
					textColor = _incorrectPlacementTextColor;
					bgColor = _incorrectPlacementColor;
					break;
				case CharacterCorrectStatus.NotInWord:
					textColor = _notInWordTextColor;
					bgColor = _notInWordColor;
					break;
				default:
				case CharacterCorrectStatus.Unknown:
					textColor = _notInWordTextColor;
					bgColor = _unknownTextColor;
					break;
            }

			letterView.TextColor = textColor;
			letterView.BackgroundColor = bgColor;
		}
	}
}