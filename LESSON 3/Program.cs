using System;
using LESSON_3.RPN;

namespace LESSON_3
{
    class Program
    {
        static void Main(string[] args)
        {
            var rpnCalculator = new RPNCalculator();

            Console.WriteLine("Программа для перевода математических выражений в обратную польскую запись");
            Console.WriteLine("Введите математическое выражение:\nПример (1 + 2) * 4 + 3");

            var input = string.Empty;

            do
            {
                Console.WriteLine("Для выхода из приложения введите: exit");
                Console.WriteLine("Введите выражение");

                input = Console.ReadLine();

                try
                {
                    if (string.IsNullOrWhiteSpace(input)) continue;

                    var expression = rpnCalculator.GetExpression(input);
                    var result = rpnCalculator.CalculateExpression(expression);

                    Console.WriteLine($"{expression}\n{result}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Возникла ошибка в приложении: {e.Message}");
                }

            } while (input.ToLower() != "exit");

        }
    }
}
