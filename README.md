# Anki Bot [![CI](https://github.com/danilkaz/AnkiBot/actions/workflows/CI.yml/badge.svg)](https://github.com/danilkaz/AnkiBot/actions/workflows/CI.yml)

Бот для интервального запоминания

В наше время человеку порой сложно запомнить большое количество информации, которая, зачастую, долго в голове не остается. 
Наш бот позволяет сделать запоминание эффективнее, используя интервальную технику.
Она заключается в повторении материала по определённым, постоянно возрастающим интервалам.

## Технологии

- C#,
- SQL - PostgreSQL
- telegram/vk API

## Компоненты

- UI
  - Это интерфейс работы с площадкой бота, работает с площадкой команд.
- App
  - Работает с интерфейсом для работы с базой данных и конвертации сущьностей
- Domain
  - Содержит описание моделей:
    - Карточка 
      - Перердняя сторона
      - Задняя сторона 
      - Состояния алгоритма запоминания
    - Колода
      - Набор карточек 
    - Методы изучения
      - Фунцдия для вычисления интервала запоминания
- Infrastructure
  - Работа с БД

## Точки расширения

- [Различные базы данных](https://github.com/danilkaz/anki/blob/8544395c7d4406990c509fed47965c4bd3be1b8d/Infrastructure/IDatabase.cs#L6)
  - Sqlite
  - PostgreSQL
- [Реализация функционала на разных площадках](https://github.com/danilkaz/anki/blob/8544395c7d4406990c509fed47965c4bd3be1b8d/UI/Bot.cs#L7)
  - VK
  - Telegram
- [Различные алгоритмы вычесления интервалов повторения](https://github.com/danilkaz/anki/blob/8544395c7d4406990c509fed47965c4bd3be1b8d/Domain/LearnMethods/ILearnMethod.cs#L6)
  - Линейный алгоритм запоминания
  - SuperMemo2
- [Команды и ветки команд с сохранением состояний](https://github.com/danilkaz/AnkiBot/blob/main/UI/Commands/ICommand.cs)

## Как запустить проект?
Два варианта запуска
### Build and Run
- Установите в окружение следующие переменные окружения ([example](https://github.com/danilkaz/AnkiBot/blob/main/example.env)):
  - для телеграм бота: ```TELEGRAM_TOKEN```
  - для вк бота: ```VK_TOKEN```
  - для id группы вк: ```VK_GROUP_ID```
  - базу данных для хранения: ```BOT_DATABASE``` (Sqlite или Postgres)
  - Если установлена Postgres, добавить также переменные окружения для postgress
- Собрать и запустить проект
### Docker compose
 - Скопировать файл example.env в файл .env 
``cp exapmle.env .env``
 - Установить необходимые переменные окружения в файле ``.env``
 - Если выбрана база Postgres запусть базу ``docker-compose -f postgres-compose up ``
 - Запустить бота ``docker-compose up``

