using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace Лаба3
{
    public class Action //Действие игрока
    {
        public string Description { get; set; } //Описание
        public PlayerAction Type { get; set; } //Тип действия

        public Action() { } //Конструктор по умолчанию
        public Action(string description, PlayerAction type) //Конструктор с параметрами
        {
            Description = description;
            Type = type;
        }
    }

    public class Fork //Развилка
    {
        public string Description { get; set; } //Описание
        public List<string> Options { get; set; }  // Список вариантов выбора, которые доступны игроку.
        public List<Action> Actions { get; set; }  // Список действий, связанных с этой развилкой (например, последствия выбора).

        public Fork() { } // Конструктор по умолчанию
        public Fork(string description, List<string> options, List<Action> actions) //Конструктор с параметрами
        {
            Description = description;
            Options = options;
            Actions = actions;
        }
    }

    // Класс, управляющий игровым процессом и развилками.
    public class GameManager
    {
        private int currentForkIndex; //Текущая развилка
        private List<Fork> forks; //Список всех развилок в игре
        public List<Fork> Forks // Свойство
        {
            get { return forks; }
            set { forks = value; }
        }
        private void InitializePlayerStats() //Инициализация начальных параметров игрока
        {
            IHealthStats healthStats = new HealthStats(100);
            IInventory inventory = new Inventory(0);
            IProgressionStats progressionStats = new ProgressionStats();
            IGameFlags gameFlags = new GameFlags();
           
            // Создаем PlayerStats, передавая ему объекты статистики
            PlayerStats playerStats = new PlayerStats(healthStats, inventory, progressionStats, gameFlags);
        }
        public GameManager() // Конструктор
        {
            currentForkIndex = 0; // Начальный индекс развилки
            forks = new List<Fork>(); // Создание списка развилок
            InitializePlayerStats(); // Инициализация параметров игрока
            InitializeForks(); // Инициализация развилок
        }


        private void InitializeForks() // Инициализация развилок
        {
            // Добавление развилок
            forks.Add(new Fork(
                "Вы просыпаетесь от громкого звука в узком коридоре.\nВокруг темнота, а выхода не видно.\nПеред собой вы видите только коридор, меч и странную коробку.\nВы решаете взять что-нибудь с собой.",
                new List<string> { "Меч", "Коробка", "Сохранить игру" },
                null // Нет дополнительных действий на этой развилке
            )
            );

            forks.Add(new Fork(
                "Перед вами развилка.",
                new List<string> { "Лево", "Право", "Сохранить игру" },
                null // Нет дополнительных действий на этой развилке
            ));

            forks.Add(new Fork(
                "Вы идете налево и видите перед собой монстра. Кажется, он агрессивно настроен.",
                new List<string> { "Убежать", "Ударить", "Поприветствовать", "Сохранить игру" },
                new List<Action>
                {
                new Action("Убежать", PlayerAction.Run),
                new Action("Ударить", PlayerAction.Hit),
                new Action("Поприветствовать", PlayerAction.Greet)
                }
            ));

            forks.Add(new Fork(
                "Вы идете направо и видите перед собой  монстра. Он не кажется опасным...",
                new List<string> { "Ударить", "Поприветствовать", "Сохранить игру" },
                new List<Action>
                {
                new Action("Ударить", PlayerAction.Hit),
                new Action("Поприветствовать", PlayerAction.Greet)
                }
            ));
            forks.Add(new Fork(
                "Вы идете дальше по темным коридорам лабиринта и видите перед собой два прохода.",
                new List<string> { "Лево", "Право", "Сохранить игру" },
                new List<Action>
                {
                null
                }
            ));
            forks.Add(new Fork(
                "Вам показалось, что только что перед вами пробежал какой-то человек!",
                new List<string> { "Побежать за ним", "Проигнорировать", "Сохранить игру" },
                new List<Action>
                { null}

            ));
            forks.Add(new Fork(
               "Вы идете по направо и внезапно проваливаетесь в глубокую яму заполненную водой!\nВы стремительно идете ко дну.",
               new List<string> { "Попытаться выбраться", "Звать на помощь", "Сохранить игру" },
               new List<Action>
               { null}

           ));
            forks.Add(new Fork(
               "Вы идете дальше.Поворачивая, вы встречаете странника, который предлагает вам выбор:",
               new List<string> { "Зелье, восстанавливающее здоровье", "Карта сокровищ", "Ничего не брать", "Сохранить игру" },
               new List<Action>
               {new Action( "Зелье, восстанавливающее здоровье", PlayerAction.TakePotion),
                new Action("Карта сокровищ", PlayerAction.TakeTreasureMap)}

           ));
            forks.Add(new Fork(
               "Идя дальше в темном коридоре лабиринта, теряется ощущение времени, кажется, что выход уже никогда не найти.\nПеред вами снова распутье: ",
               new List<string> { "Право", "Прямо", "Лево", "Сохранить игру" },
               new List<Action>
               {null}

           ));
            forks.Add(new Fork(
               "Неожиданно, вы видите перед собой маленькую девочку",
               new List<string> { "Заговорить", "Пройти мимо", "Сохранить игру" },
               new List<Action>
               {new Action( "Заговорить", PlayerAction.Greet),
                new Action("Пройти мимо", PlayerAction.Ignore)}

           ));
            forks.Add(new Fork(
               "Сделав пару шагов, вы улавливаете мерзкий запах и видите перед собой огромного змея!",
               new List<string> { "Ударить", "Пройти мимо", "Сохранить игру" },
               new List<Action>
               {new Action( "Заговорить", PlayerAction.Hit),
                new Action("Пройти мимо", PlayerAction.Run)}

           ));
            forks.Add(new Fork(
               "Уже совсем потеряв надежду на спасение, вы оказываетесь перед несколькими дверьми.",
               new List<string> { "Далее" },
               new List<Action>
               { }

           ));
            forks.Add(new Fork(
              "Вы понимаете, что не собрали достаточно ключей, а по другому дверь не открыть.\nПридется выбирать из оставшихся дверей",
              new List<string> { "Правая", "Левая", "Сохранить игру" },
              new List<Action>
              { }

          ));
            forks.Add(new Fork(
                "\nНачать игру сначала?",
                new List<string> { "Да", "Нет" },
                new List<Action>
                {
                }

            ));
        }

        public void StartGame() //Запуск игры
        {

            LoadGame("save.json"); // Загружаем игру при старте
            ProcessCurrentFork();
        }
        private void ResetGame() //Перезапуск игры
        {
            currentForkIndex = 0;

            // Удаление файла сохранения
            string saveFilePath = "save.json"; // Путь к файлу сохранения
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath); // Удаление файла, если он существует
                Console.WriteLine("Сохранения успешно удалены.");
            }
           

            // Пересоздание PlayerStats с начальными значениями
            InitializePlayerStats();

            // Перезапуск обработки развилок с начала
            ProcessCurrentFork();
        }


        private void ProcessCurrentFork() // Обрабатываем текущую развилку, отображая её описание и варианты выбора.
        {
            if (currentForkIndex < forks.Count) // Проверяем, не вышли ли за пределы списка развилок
            {
                {
                    var currentFork = forks[currentForkIndex]; //Текущая развилка
                    Console.WriteLine(currentFork.Description); //Пишем описание развилки
                    for (int i = 0; i < currentFork.Options.Count; i++) 
                    {
                        Console.WriteLine($"{i + 1} - {currentFork.Options[i]}"); // Выводим каждый вариант выбора с номером
                    }

                    HandlePlayerChoice();  // Вызываем метод для обработки выбора игрока
                }
            }
            else
            {
                Console.WriteLine("\nИгра завершена!"); //Если развилки закончились, завершаем игру
            }
        }

        private void HandlePlayerChoice() //Обработка выбора игрока
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > forks[currentForkIndex].Options.Count) // Проверка ввода
            {
                Console.WriteLine("Неверный ввод! Попробуйте снова!");
            }

            // Обработка выбора игрока
            switch (currentForkIndex)
            {
                case 0: // Первая развилка
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Вы подбираете меч. Выбран путь воина. В инвентарь добавлен меч.");
                            PlayerStats.Inventory.Objects.Add("меч");
                            PlayerStats.Progression.ChoosedPath = "Путь воина";
                            break;
                        case 2:
                            Console.WriteLine("Вы подбираете загадочную коробку. Выбран путь стратега. В инвентарь добавлена коробка.");
                            PlayerStats.Inventory.Objects.Add("коробка");
                            PlayerStats.Progression.ChoosedPath = "Путь стратега";
                            break;
                        case 3:

                            SaveGame("save.json");
                            break;
                    }

                    currentForkIndex = 1; // Переход ко второй развилке
                    break;

                case 1: // Вторая развилка

                    switch (choice)
                    {
                        case 1:

                            currentForkIndex = 2; // Переход к третьей развилке
                            break;
                        case 2:

                            currentForkIndex = 3; // Переход к четвертой развилке
                            break;
                        case 3:
                            SaveGame("save.json"); //Сохранение игры
                            currentForkIndex = 1;
                            break;
                    }
                    break;

                case 2: // Третья развилка 

                    Monster evilMonster = new BadMonster();
                    switch (choice)
                    {
                        case 1:
                            evilMonster.ReactToAction(PlayerAction.Run); //Реакция на действие игрока
                            currentForkIndex = 4;

                            break;
                        case 2:
                            evilMonster.ReactToAction(PlayerAction.Hit);
                            currentForkIndex = 4;

                            break;
                        case 3:
                            evilMonster.ReactToAction(PlayerAction.Greet);
                            currentForkIndex = 4;

                            break;
                        case 4:
                            SaveGame("save.json");
                            currentForkIndex = 2;
                            break;
                    }
                    break;

                case 3: // Четвертая развилка 

                    Monster goodMonster = new GoodMonster();
                    switch (choice)
                    {
                        case 1:
                            goodMonster.ReactToAction(PlayerAction.Hit);
                            currentForkIndex = 4;
                            break;
                        case 2:
                            goodMonster.ReactToAction(PlayerAction.Greet);
                            PlayerStats.Flags.MeetGoodMonster = true;
                            currentForkIndex = 4;
                            break;
                        case 3:
                            SaveGame("save.json");
                            currentForkIndex = 3;
                            break;
                    }
                    break;
                case 4: //Пятая развилка

                    switch (choice)
                    {
                        case 1:
                            currentForkIndex = 5; 
                            break;
                        case 2:
                            currentForkIndex = 6; 
                            break;
                        case 3:
                            SaveGame("save.json");
                            currentForkIndex = 4;
                            break;
                    }
                    break;
                case 5: //Шестая развилка

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Вы побежали за ним, и увидели перед собой мужчину в потрепанной одежде.\nОн говорит “Я уже долго брожу по этим коридорам. Берегись, не поворачивай налево, не повторяй моей ошибки” И уходит.");
                            currentForkIndex = 7;
                            PlayerStats.Flags.MeetStranger = true;
                            break;
                        case 2:
                            Console.WriteLine("Вы думаете, что вам просто померещилось и идете дальше.");
                            currentForkIndex = 7;
                            break;
                        case 3:
                            SaveGame("save.json");
                            currentForkIndex = 5;
                            break;
                    }

                    break;
                case 6: //Седьмая развилка
                   
                    switch (choice)
                    {
                        case 1:
                            if (PlayerStats.Progression.ChoosedPath == "Путь воина")
                            {
                                Console.WriteLine("Вашей выносливости можно позавидовать!Вы изо всех сил гребёте и выбираетесь на поверхность.");
                                currentForkIndex = 7;
                            }
                            else { Console.WriteLine("Вы пытались выбраться, но ваши силы быстро иссякли. Вы полностью погрузились под воду.\nИгра окончена!"); currentForkIndex = 13; } //Плохой конец игры
                            break;
                        case 2:
                            Console.WriteLine("Вы зовете на помощь, хоть и понимаете, что никто не придёт.");
                            if (PlayerStats.Flags.MeetGoodMonster == true) //Выход на развилку при определенном условии
                            {
                                Console.WriteLine("Вы слышите громкие шаги и видите монстра, встретившегося вам в самом начале пути!\nОн протягивает свою широкую лапу и вытаскивает вас.\nОн убегает, а вы очень радуетесь, что тогда не проявили враждебности…"); currentForkIndex = 7;
                            }
                            else
                            {
                                Console.WriteLine("Как и ожидалось, никто не пришел...\nВы полностью погрузились под воду.\nИгра окончена!"); currentForkIndex = 13;
                            }
                            break;
                        case 3:
                            SaveGame("save.json");
                            currentForkIndex = 6;
                            break;
                    }
                    break;
                case 7:

                    Wanderer wanderer = new Wanderer();
                    switch (choice)
                    {
                        case 1:
                            wanderer.ReactToAction(PlayerAction.TakePotion);
                            currentForkIndex = 8;

                            break;
                        case 2:
                            wanderer.ReactToAction(PlayerAction.TakeTreasureMap);
                            currentForkIndex = 8;

                            break;
                        case 3:
                            Console.WriteLine("Вы решаете,что здесь лучше ничего не брать у незнакомцев и проходите мимо странника.");
                            currentForkIndex = 8;
                            break;
                        case 4:
                            SaveGame("save.json");
                            currentForkIndex = 7;
                            break;


                    }
                    break;
                case 8:

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Вы идете направо.");
                            currentForkIndex = 9;

                            break;
                        case 2:
                            Console.WriteLine("Вы замечаете, что уже видели похожие детали интерьера, и понимаете, что оказались вначале.\nПридется проходить всё сначала!");
                            ResetGame();

                            break;
                        case 3:
                            Console.WriteLine("Вы идете налево.");
                            currentForkIndex = 10;
                            break;
                        case 4:
                            SaveGame("save.json");
                            currentForkIndex = 8;
                            break;
                    }
                    break;
                case 9:

                    Girl girl = new Girl();
                    switch (choice)
                    {
                        case 1:
                            girl.ReactToAction(PlayerAction.Greet);
                            currentForkIndex = 11;


                            break;
                        case 2:
                            girl.ReactToAction(PlayerAction.Ignore);
                            currentForkIndex = 11;
                            break;
                        case 3:
                            SaveGame("save.json");
                            currentForkIndex = 9;
                            break;
                    }
                    break;

                case 10:

                    Serpent serpent = new Serpent();
                    if (PlayerStats.Flags.MeetStranger) Console.WriteLine("Кажется кто-то предупреждал вас о том,что не стоит поворачивать налево...");
                    switch (choice)
                    {
                        case 1:
                            serpent.ReactToAction(PlayerAction.Hit);
                            if (PlayerStats.Health.Health <= 0) { currentForkIndex = 13; }
                            else currentForkIndex = 11;
                            break;
                        case 2:
                            serpent.ReactToAction(PlayerAction.Run);
                            if (PlayerStats.Health.Health <= 0) { currentForkIndex = 13; }
                            else currentForkIndex = 11;
                            break;
                        case 3:
                            SaveGame("save.json");
                            currentForkIndex = 10;
                            break;
                    }
                    break;
                case 11:

                    Console.WriteLine("Вы видите свет, проникающий сквозь щель центральной двери.Кажется там выход.\nНо на ней висит три замка...");
                    if (PlayerStats.Inventory.Keys == 3)
                    {
                        Console.WriteLine("В памяти всплывает, что вы как раз собрали три ключа,блуждая по лабиринту.\nС их помощью, замки поддаются, и вы, наконец, выходите на свободу!\nХороший конец.\n");
                        currentForkIndex = 13;

                    }
                    else
                    {
                        currentForkIndex = 12;
                    }
                    break;
                case 12:

                    switch (choice)
                    {
                        case 1:
                            bool puzzleSolved = false; // Флаг, указывающий, решена ли головоломка
                            int attempts = 2; // Количество попыток для решения головоломки

                            Console.WriteLine("Вы решаете выбрать правую дверь.\nВы оказываетесь в комнате,в которой видите еще одну дверь, ведущую на свободу"); // Вывод сообщения о выборе двери
                            Console.WriteLine("Но она закрыта, а чтобы открыть ее придется решить еще одну головоломку."); // Вывод сообщения о головоломке
                            Console.WriteLine("Найдите корни квадратного уравнения: х^2 - 2х - 35 = 0"); // Вывод условия головоломки

                            if (PlayerStats.Inventory.Objects.Contains("коробка")) // Проверка наличия коробки в инвентаре
                            {
                                Console.WriteLine("Вы вспоминате, что у вас еще осталась загадочная коробка.\nОткрыв ее, вы видите на ее дне подсказку\nДискриминант равен 144. Один из корней равен 7"); // Вывод подсказки, если есть коробка
                            }

                            while (attempts > 0 && !puzzleSolved) // Цикл для попыток решения головоломки
                            {
                                Console.WriteLine($"Попыток осталось: {attempts}"); // Вывод количества оставшихся попыток
                                Console.WriteLine("Введите первый корень (через пробел второй):"); // Вывод запроса на ввод ответа

                                string playerInput = Console.ReadLine(); // Получение ввода игрока
                                if (string.IsNullOrEmpty(playerInput)) // Проверка, что ввод не пустой
                                {
                                    Console.WriteLine("Пожалуйста, введите два корня через пробел."); // Сообщение об ошибке ввода
                                    continue; // Переход к следующей итерации цикла
                                }
                                string[] parts = playerInput.Split(' '); // Разбиение ввода на части по пробелу
                                if (parts.Length != 2) // Проверка, что введено два числа
                                {
                                    Console.WriteLine("Пожалуйста, введите два корня через пробел."); // Сообщение об ошибке ввода
                                    continue; // Переход к следующей итерации цикла
                                }
                                if (int.TryParse(parts[0], out int root1) && int.TryParse(parts[1], out int root2)) // Попытка преобразовать ввод в числа
                                {
                                    if ((root1 == 7 && root2 == -5) || (root1 == -5 && root2 == 7)) // Проверка правильности ответа
                                    {
                                        Console.WriteLine("Правильно! Дверь открывается, и вы видите долгожданный выход!\nХороший конец.\n"); // Сообщение о правильном ответе
                                        puzzleSolved = true; // Установка флага решения головоломки
                                        break; // Выход из цикла
                                    }
                                    else Console.WriteLine("Ответ неверный."); // Сообщение о неверном ответе
                                }
                                else
                                {
                                    Console.WriteLine("Неверный формат ввода. Пожалуйста, введите целые числа."); // Сообщение об ошибке формата ввода
                                }
                                attempts--; // Уменьшение количества попыток
                            }

                            if (!puzzleSolved) // Если головоломка не решена
                            {
                                Console.WriteLine("К сожалению, все попытки исчерпаны. Дверь так и остается закрытой, а вы запертым в лабиринте...\nПлохой конец.\n"); // Сообщение о проигрыше
                            }

                            currentForkIndex = 13;
                            break;
                        case 2:
                            Console.WriteLine("Вы решаете выбрать левую дверь.\nВы оказываетесь в комнате,в которой видите еще одну дверь, ведущую на свободу");
                            Console.WriteLine("Но комната кишит монстрами. И чтобы добраться до заветной двери вам надо уничтожить их всех.");
                            if (PlayerStats.Inventory.Objects.Contains("меч") && PlayerStats.Health.Health >= 110) 
                            {
                                Console.WriteLine("Вы храбро сражались с монстрами и одержали победу!\nПуть к заветной двери свободен и вы выходите на свободу.\nХороший конец.\n");
                                currentForkIndex = 13;
                            }
                            else
                            {
                                Console.WriteLine("К сожалению, вы оказались бессильны перед таким количеством монстров...\nПлохой конец.\n");
                                currentForkIndex = 13;
                            }
                            break;
                        case 3:
                            SaveGame("save.json");
                            currentForkIndex = 12;
                            break;
                    }
                    break;
                case 13:

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Вы решили переиграть. Начинаем игру заново!\n");
                            ResetGame(); //Перезапуск игры
                            break;
                        case 2:
                            currentForkIndex = 14; // Завершение игры
                            break;

                    }
                    break;
            }
            ProcessCurrentFork();// Обработка следующей развилки
        }


        public void SaveGame(string filePath) //Сохранение игры
        {
            // Создаем объект GameState для сохранения текущего состояния игры
            var gameState = new GameState
            {
                CurrentForkIndex = currentForkIndex,
                Forks = forks
            };
            //Сериализуем объект в JSON формат
            string json = JsonConvert.SerializeObject(gameState, Formatting.Indented);
            File.WriteAllText(filePath, json);
            Console.WriteLine("Игра была сохранена"); // Выводим сообщение об успешном сохранении
        }

        public void LoadGame(string filePath) //Загрузка состояния игры 
        {
            // Проверяем, существует ли файл сохранения и не пуст ли он
            if (File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                // Читаем JSON из файла и десериализуем в объект GameState
                string json = File.ReadAllText(filePath);
                var gameState = JsonConvert.DeserializeObject<GameState>(json);

                // Восстанавливаем состояние игры из объекта GameState
                currentForkIndex = gameState.CurrentForkIndex; // Восстанавливаем индекс текущей развилки
                forks = gameState.Forks; // Восстанавливаем список развилок

                Console.WriteLine("Игра загружена."); // Выводим сообщение об успешной загрузке
            }
            else
            {
                Console.WriteLine("Начинаем новую игру."); // Выводим сообщение о начале новой игры, если сохранения нет
                ProcessCurrentFork(); // Начинаем новую игру
            }
        }

        // Вспомогательный класс для хранения состояния игры.
        private class GameState
        {
            public int CurrentForkIndex { get; set; } // Индекс текущей развилки
            public List<Fork> Forks { get; set; } // Список развилок

        }
    }
}
