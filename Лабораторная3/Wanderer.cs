using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба3
{
    public partial class Wanderer : IReactable //Странник
    {
        string answerToTheRiddle = "Время"; //Ответ на загадку
        private const int MaxAttempts = 3; //Максимальное количество попыток

        public void GivePotion() // Дает зелье и увеличивает здоровье
        {
            Console.WriteLine("Странник протягивает вам зелье и говорит: «Соблюдай осторожность и используй это жизненное благо с умом» и исчезает.");
            PlayerStats.Health.AddHealth(35); // Увеличиваем здоровье
        }

        public void GiveMap() // Дает карту, требуя ответ на загадку
        {
            Console.WriteLine("Странник говорит: «Но просто так я тебе ее не отдам! Сначала нужно дать ответ на мой вопрос.");
            Console.WriteLine("Уничтожает все кругом:\nЦветы, зверей, высокий дом\nСжует железо, сталь сожрет\nИ скалы в порошок сотрет,\nМощь городов, власть королей\nЕго могущества слабей.»");

            bool mapGiven = false; // Флаг, дана ли карта

            switch (PlayerStats.Progression.ChoosedPath) // Определяем количество попыток
            {
                case "Путь стратега":
                    mapGiven = HandleRiddle(MaxAttempts); // Загадка с 3 попытками
                    break;
                case "Путь воина":
                    mapGiven = HandleRiddle(1); // Загадка с 1 попыткой
                    break;
            }

            if (!mapGiven && PlayerStats.Progression.ChoosedPath == "Путь стратега" || !mapGiven && PlayerStats.Progression.ChoosedPath == "Путь воина") // Если карта не дана
            {
                Console.WriteLine("Ответ неверный! Cтранник говорит: «Кажется я переоценил тебя» и насылает на вас заклятье!");
                PlayerStats.Health.RemoveHealth(25); // Уменьшаем здоровье
            }
        }

        private bool HandleRiddle(int maxAttempts) // Обрабатывает загадку
        {
            int attempts = 0; // Счетчик попыток
            string playerAnswer; // Ответ игрока
            while (attempts < maxAttempts) // Цикл для попыток
            {
                Console.WriteLine("Введите ответ:"); // Запрос ответа
                playerAnswer = Console.ReadLine(); // Получаем ответ
                attempts++; // Увеличиваем счетчик
                if (playerAnswer.Equals(answerToTheRiddle, StringComparison.OrdinalIgnoreCase)) // Проверяем ответ
                {
                    Console.WriteLine("Это правильный ответ! Странник говорит: «Я еще не встречал таких мудрых людей...» и протягивает вам карту");
                    Console.WriteLine("Вы не знаете что с ней делать, но развернув ее вы находите ключ!");
                    PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
                    return true; // Ответ верный
                }
                if (attempts < maxAttempts) // Если попытки еще есть
                    Console.WriteLine("Ответ неверный! Но странник говорит: «Я вижу ты не глупец и даю тебе еще одну попытку»");
            }
            return false; // Ответ неверный
        }

        public void ReactToAction(PlayerAction action) // Обрабатывает действие игрока
        {
            switch (action) // Выполняем действие
            {
                case PlayerAction.TakePotion:
                    GivePotion(); // Даем зелье
                    break;
                case PlayerAction.TakeTreasureMap:
                    GiveMap(); // Даем карту
                    break;
                default:
                    Console.WriteLine("Странник не понимает вашего действия."); // Действие непонятно
                    break;
            }
        }
    }
}
