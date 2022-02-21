using Wordle.Lib.Enums;

namespace Wordle.App.Views;

public partial class WordRowLetterView : Label
{
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

		Style = App.Current.Resources.GetStyleFromResourceDictionary("UnknownWordRowLetterStyle");
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
            string styleName;

            switch (status)
            {
				case CharacterCorrectStatus.CorrectPosition:
					styleName = "CorrectWordRowLetterStyle";
					break;
				case CharacterCorrectStatus.IncorrectPosition:
					styleName = "IncorrectWordRowLetterStyle";
					break;
				case CharacterCorrectStatus.NotInWord:
					styleName = "NotInWordWordRowLetterStyle";
					break;
				default:
				case CharacterCorrectStatus.Unknown:
					styleName = "UnknownWordRowLetterStyle";
					break;
            }

			letterView.Style = App.Current.Resources.GetStyleFromResourceDictionary(styleName);
		}
	}
}