using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба3
{
    public partial class Serpent : IReactable //Змей
    {
        public void Fight() // Метод для действия "сражаться"
        {
            if (PlayerStats.Inventory.Objects.Contains("меч")) // Если есть меч
            {
                Console.WriteLine("Вы решаетесь атаковать змея!\nСражение кажется бесконечным,но вы все же выходите из него победителем.");
                Console.WriteLine("Вы замечаете,что на поверженном змее висит ключ и забираете его.");
                PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
            }
            else // Если нет меча
            {
                Console.WriteLine("Вы бросаетесь на змея с кулаками.\nКажется,вы совсем не кажетесь ему опасным.\nОн ударяет вас своим огромным хвостом!");
                Console.WriteLine("От такого удара оправиться не получится...");
                int health = PlayerStats.Health.Health;
                PlayerStats.Health.RemoveHealth(health); // У игрока отнимается все здоровье
            }
        }

        public void ReactionToEscape() // Метод для реакции на побег игрока
        {
            Console.WriteLine("Вы решаете не рисковать и пройти мимо змея");
            switch (PlayerStats.Progression.ChoosedPath) // Зависит от выбранного пути
            {
                case "Путь стратега": // Если выбран путь стратега
                    Console.WriteLine("Вы тщательно обдумываете все свои шаги, и вам удается обойти змея, оставшись незамеченным.");
                    break;
                case "Путь воина": // Если выбран путь воина
                    Console.WriteLine("Вы оббегаете змея.\nУ самого выхода он замечает вас и задевает своим хвостом!");
                    PlayerStats.Health.RemoveHealth(50); // Уменьшаем здоровье
                    break;
            }
        }

        public void ReactToAction(PlayerAction action) // Метод для реакции на действие игрока
        {
            switch (action) 
            {
                case PlayerAction.Hit:
                    Fight(); // Вызываем действие "сражаться"
                    break;
                case PlayerAction.Run:
                    ReactionToEscape(); // Вызываем действие "реакция на побег"
                    break;
                default:
                    Console.WriteLine("Змей не понимает вашего действия."); // Действие неопределенно
                    break;
            }
        }
    }
}


