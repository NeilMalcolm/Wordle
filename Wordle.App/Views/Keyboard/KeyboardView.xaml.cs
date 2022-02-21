using System.Text;
using Wordle.Lib.Enums;
using Wordle.Lib.Models;

namespace Wordle.App.Views;

public partial class KeyboardView : Grid
{
	const string KeyboardButtonCorrectPositionStyleKey = "KeyboardButtonCorrectPositionStyle";
	const string KeyboardButtonIncorrectPositionStyleKey = "KeyboardButtonIncorrectPositionStyle";
	const string KeyboardButtonNotInWordStyleKey = "KeyboardButtonNotInWordStyle";

	private StringBuilder _stringBuilder = new StringBuilder();

	public static readonly BindableProperty TextProperty = BindableProperty.Create
	(
		propertyName: nameof(Text),
		returnType: typeof(string),
		declaringType: typeof(KeyboardView),
		defaultValue: string.Empty,
		defaultBindingMode: BindingMode.TwoWay,
		propertyChanged: OnTextPropertyChanged
	);
	
	public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create
	(
		propertyName: nameof(MaxLength),
		returnType: typeof(int),
		declaringType: typeof(KeyboardView),
		defaultValue: 5,
		defaultBindingMode: BindingMode.OneWay
	);

	protected static readonly BindableProperty OnButtonTappedCommandProperty = BindableProperty.Create
	(
		propertyName: nameof(OnButtonTappedCommand),
		returnType: typeof(Command),
		declaringType: typeof(KeyboardView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay
	);
	
	protected static readonly BindableProperty BackspaceCommandProperty = BindableProperty.Create
	(
		propertyName: nameof(BackspaceCommand),
		returnType: typeof(Command),
		declaringType: typeof(KeyboardView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay
	);
	
	public static readonly BindableProperty OnEnterTappedCommandProperty = BindableProperty.Create
	(
		propertyName: nameof(OnEnterTappedCommand),
		returnType: typeof(Command),
		declaringType: typeof(KeyboardView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay
	);
	
	public static readonly BindableProperty EnteredCharactersProperty = BindableProperty.Create
	(
		propertyName: nameof(EnteredCharacters),
		returnType: typeof(WordCharacter[]),
		declaringType: typeof(KeyboardView),
		defaultValue: default,
		defaultBindingMode: BindingMode.OneWay,
		propertyChanged: OnEnteredCharactersChanged
	);

	public Button[] Buttons
    {
		get => Children.Where(x => x is Button b)
			.Select(y => y as Button)
			.ToArray();
    }

	public string Text
    {
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
    }
	
	public int MaxLength
	{
		get => (int)GetValue(MaxLengthProperty);
		set => SetValue(MaxLengthProperty, value);
    }

	public Command OnButtonTappedCommand
	{
		get => (Command)GetValue(OnButtonTappedCommandProperty);
		private set => SetValue(OnButtonTappedCommandProperty, value);
	}
	
	public Command BackspaceCommand
	{
		get => (Command)GetValue(BackspaceCommandProperty);
		private set => SetValue(BackspaceCommandProperty, value);
	}
	
	public Command OnEnterTappedCommand
	{
		get => (Command)GetValue(OnEnterTappedCommandProperty);
		set => SetValue(OnEnterTappedCommandProperty, value);
	}
	
	public WordCharacter[] EnteredCharacters
	{
		get => (WordCharacter[])GetValue(EnteredCharactersProperty);
		set => SetValue(EnteredCharactersProperty, value);
	}

	public KeyboardView()
	{
		InitializeComponent();

		OnButtonTappedCommand = new Command<string>(AddNewCharacterToText);
		BackspaceCommand = new Command(RemoveLastCharacterFromText);
	}

	void AddNewCharacterToText(string newText)
    {
		if (!IsEnabled || _stringBuilder.Length == MaxLength)
        {
			return;
        }

		_stringBuilder.Append(newText);

		SetText();
	}

	void RemoveLastCharacterFromText()
    {
		if (!IsEnabled || _stringBuilder.Length <= 0)
        {
			return;
        }

		_stringBuilder.Remove(_stringBuilder.Length - 1, 1);

		SetText();
	}

	void SetText()
	{
		Text = _stringBuilder.ToString();
	}

	void Reset()
	{
		_stringBuilder.Clear();

		var buttons = Buttons;
		foreach (var button in buttons)
		{
			if (button.Text == "ENTER" 
				|| button.Text == "&#x232B;")
			{
				continue;
			}

			SetButtonTheme(this, button, "KeyboardButtonNormalStyle");
		}
	}
	public static void OnTextPropertyChanged(BindableObject sender, object oldValue, object newValue)
    {
		if (sender is KeyboardView keyboardView)
        {
			if (newValue is string newString)
            {
				if (string.IsNullOrWhiteSpace(newString))
                {
					keyboardView._stringBuilder.Clear();
				}
            }
        }
    }

	public static void OnEnteredCharactersChanged(BindableObject sender, object oldValue, object newValue)
    {
		if (sender is KeyboardView keyboardView)
        {
			if (newValue is WordCharacter[] chars && chars.Length > 0)
            {
				var buttons = keyboardView.Buttons;
				foreach (var character in chars)
				{
					if (character is null)
                    {
						continue;
                    }
					var button = buttons.FirstOrDefault(x => x.Text == character.Character.ToString());

					if (character.CharacterStatus == CharacterCorrectStatus.CorrectPosition)
					{
						SetButtonTheme(keyboardView, button, KeyboardButtonCorrectPositionStyleKey);
					}
					else if (character.CharacterStatus == CharacterCorrectStatus.IncorrectPosition)
					{
						SetButtonTheme(keyboardView, button, KeyboardButtonIncorrectPositionStyleKey);
					}
					else
					{
						SetButtonTheme(keyboardView, button, KeyboardButtonNotInWordStyleKey);
					}
                }
            }
        }
    }

	static void SetButtonTheme(KeyboardView keyboardView, Button button, string styleName)
    {
		button.Style = keyboardView.Resources.GetStyleFromResourceDictionary(styleName);
	}
}