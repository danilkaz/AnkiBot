using System.Collections;
using System.Collections.Generic;

namespace UI
{
    public class KeyboardProvider
    {
        public static KeyboardProvider DefaultKeyboard = new KeyboardProvider(new[]
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
        
        public string[][] Keyboard { get; }

        public KeyboardProvider(string[][] keyboard)
        {
            Keyboard = keyboard;
        }
    }
}