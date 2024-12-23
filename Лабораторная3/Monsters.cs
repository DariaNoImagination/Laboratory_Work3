using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба3
{
    public abstract partial class Monster : IReactable // Абстрактный базовый класс для монстров
    {
        protected string name; // Имя монстра

        public abstract void Hit(); // Действие удара
        public abstract void Greetings(); // Действие приветствия
        public abstract void ReactToAction(PlayerAction action); // Реакция на действие
    }

    public partial class GoodMonster : Monster // Класс доброго монстра
    {
        public override void Hit() // Реализация удара
        {
            Console.WriteLine("Вы ударили монстра!");
            Console.WriteLine("Он не ударил в ответ и убежал. Кажется, вы видели слезы в его глазах...");
            Console.WriteLine("Вы чувствуете себя виноватым.");

            if (PlayerStats.Inventory.Objects.Contains("меч")) // Если есть меч
            {
                Console.WriteLine("Меч был изъят!");
                PlayerStats.Inventory.Objects.Remove("меч"); // Удаляем меч
            }
            else // Если нет меча
            {
                Console.WriteLine("Загадочная коробка была изъята!");
                PlayerStats.Inventory.Objects.Remove("коробка"); // Удаляем коробку
            }
        }

        public override void Greetings() // Реализация приветствия
        {
            Console.WriteLine("Монстр рад, что вы не напали. Он задает вам вопрос:");
            Console.WriteLine("Ты за луну или за солнце?");
            Console.WriteLine("Вы думаете: достаточно странный вопрос. Но решаете ответить:");
            Console.WriteLine("1 - За Луну, конечно. Луна полезнее, ведь она светит ночью, а Солнце светит днём, когда и так светло.");
            Console.WriteLine("2 - Это зависит от настроения: иногда хочется яркого света, а иногда спокойной ночи.");

            int choice; // Выбор игрока
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3) // Проверка ввода
            {
                Console.WriteLine("Неверный ввод! Попробуйте снова!");
            }

            switch (choice) // Действие в зависимости от выбора
            {
                case 1:
                    Console.WriteLine("Монстру явно не понравился ваш ответ. Он стукнул вас и ушел. Здоровье уменьшилось.");
                    PlayerStats.Health.RemoveHealth(25); // Уменьшаем здоровье
                    break;
                case 2:
                    Console.WriteLine("Монстр улыбнулся - ему явно понравился ваш ответ. Он протягивает вам что-то.");
                    PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
                    break;
                case 3:
                    GameManager gameManager = new GameManager();
                    break;
            }
        }

        public override void ReactToAction(PlayerAction action) // Реализация реакции на действие
        {
            switch (action) // Выполнение действия
            {
                case PlayerAction.Hit:
                    Hit(); // Действие удара
                    break;
                case PlayerAction.Greet:
                    Greetings(); // Действие приветствия
                    break;
                default:
                    Console.WriteLine("Монстр не понимает вашего действия."); // Неопределенное действие
                    break;
            }
        }
    }

    public partial class BadMonster : Monster // Класс плохого монстра
    {
        public override void Hit() // Реализация действия удара
        {
            switch (PlayerStats.Progression.ChoosedPath) // Зависит от выбора пути
            {
                case "Путь воина": // Если выбран путь воина
                    Console.WriteLine("Вы чувствуете прилив сил и атакуете монстра мечом! ");
                    Console.WriteLine("После долгого сражения вы побеждаете монстра.Его огромная лапа разжимается и вы видите в ней ключ");
                    PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
                    break;

                case "Путь стратега": // Если выбран путь стратега
                    Console.WriteLine("Вы ударяете монстра!");
                    Console.WriteLine("Кажется удар только больше разозлил его");
                    Console.WriteLine("Вы шарите в карманах в надежде найти что-нибудь,что может помочь.");
                    Console.WriteLine("Вы находите загадочную коробку и решаете бросить ее в монстра.");
                    Console.WriteLine("Она попадает ему прямо в голову. На миг он замирает,но все же успевает нанести удар.");
                    PlayerStats.Health.RemoveHealth(25); // Уменьшаем здоровье
                    PlayerStats.Inventory.Objects.Remove("коробка"); // Убираем коробку
                    break;
            }
        }

        public override void Greetings() // Реализация действия приветствия
        {
            Console.WriteLine("Вам кажется что монстр не так уж и страшен.Вы решаете заговорить с ним");
            Console.WriteLine("Вы не успеваеете сказать и слова как монстра наносит удар!");
            PlayerStats.Health.RemoveHealth(50); // Уменьшаем здоровье
        }

        public void Run() // Реализация действия бегства
        {
            Console.WriteLine("Вы не думаете ни о чем и просто бежите. Монстр даже не успевает ничего понять.");
            Console.WriteLine("Вы убежали от монстра!");
            if (PlayerStats.Progression.ChoosedPath == "Путь стратега") // Если выбран путь стратега
            {
                Console.WriteLine("По пути вы видите ключ и поднимаете его.");
                PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
            }
        }

        public override void ReactToAction(PlayerAction action) // Реализация реакции на действие
        {
            switch (action) // Выбор действия
            {
                case PlayerAction.Hit:
                    Hit(); // Выполняем действие удара
                    break;
                case PlayerAction.Greet:
                    Greetings(); // Выполняем действие приветствия
                    break;
                case PlayerAction.Run:
                    Run(); // Выполняем действие бегства
                    break;
                default:
                    Console.WriteLine("Монстр не понимает вашего действия."); // Неопределенное действие
                    break;
            }
        }
    }
}