namespace UI
{
    public class KeyboardProvider
    {
        public static readonly KeyboardProvider DefaultKeyboard = new(new[]
        {
            new[]
            {
                "Создать колоду", "Удалить колоду"
            },
            new[]
            {
                "Добавить карточку", "Удалить карточку"
            },
            new[]
            {
                "Учить колоду"
            }
        });

        public KeyboardProvider(string[][] keyboard)
        {
            Keyboard = keyboard;
        }

        public string[][] Keyboard { get; }
    }
}