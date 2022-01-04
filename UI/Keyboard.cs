using System.Collections.Generic;

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

        public IReadOnlyList<IReadOnlyList<string>> Keyboard { get; } //TODO: IReadOnlyList<IReadOnlyList<string>>
    }
}