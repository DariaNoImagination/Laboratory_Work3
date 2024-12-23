using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба3
{
    public partial class Girl : IReactable //Девочка
    {
        int answerToTheQuestion = 52; // Правильный ответ на вопрос
        string playerAnswer; // Ответ игрока

        public void AskQuestion() // Метод задать вопрос
        {
            Console.WriteLine("Она смеясь, повторяет таблицу умножения: «4 на 6 хахаха 24.. 5 на 8 ахах 40!»\nЗаметив вас она серьезно поворачивается и спрашивает «2 плюс 5 на 10..?»");
            Console.WriteLine("Введите ответ:"); // Запрос на ввод ответа
            playerAnswer = Console.ReadLine(); // Получаем ввод
            if (int.TryParse(playerAnswer, out int playerAnswerOnQuestion) && answerToTheQuestion == playerAnswerOnQuestion) // Проверяем ответ
            {
                Console.WriteLine("Это правильный ответ!\nДевочка расплывается в улыбке и хохоча уходит, обронив ключ.\nВы поднимаете ключ и идете дальше.");
                PlayerStats.Inventory.AddKeys(1); // Добавляем ключ
            }
            else Console.WriteLine("Ответ неверный!\nДевочка смотрит на вас с недовольным выражением лица и уходит ничего не сказав."); // Ответ неверный
        }

        public void ReactionOnIgnore() // Реакция на игнорирование
        {
            Console.WriteLine("Девочка злится, что вы ее проигнорировали.\nОна догоняет вас и дает вам пинка.\nУдар был слабым,но все же неприятным.");
            PlayerStats.Health.RemoveHealth(10); // Уменьшаем здоровье
        }

        public void ReactToAction(PlayerAction action) // Реакция на действие
        {
            switch (action) // Проверяем действие
            {
                case PlayerAction.Greet:
                    AskQuestion(); // Задаем вопрос
                    break;
                case PlayerAction.Ignore:
                    ReactionOnIgnore(); // Реакция на игнорирование
                    break;
                default:
                    Console.WriteLine("Девочка не понимает вашего действия."); // Если действие неопределенно
                    break;
            }
        }
    }
}
