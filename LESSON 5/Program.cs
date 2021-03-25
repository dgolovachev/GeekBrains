using System;
using System.Collections.Generic;

namespace LESSON_5
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<string, string> expressionBuilder = ExpressionBuilder;
            Func<string, double> expressionCalculator = OPNExpressionCalculator;
            Action<string, double> resultHandler = (expression, result) => { Console.WriteLine($"{expression}\n{result}"); };

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

                    var expression = BuildExpression(input, expressionBuilder);
                    var result = CalculateExpression(expression, expressionCalculator);
                    HandleRusult(expression, result, resultHandler);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Возникла ошибка в приложении: {e.Message}");
                }

            } while (input.ToLower() != "exit");

        }

        /// <summary>
        /// Метод обратодки результат
        /// </summary>
        /// <param name="expression">Выражение ОПН</param>
        /// <param name="result">Результат вычисления Выражение ОПН</param>
        /// <param name="resultHandler">Функция обработчик результата</param>
        public static void HandleRusult(string expression, double result, Action<string, double> resultHandler)
        {
            resultHandler(expression, result);
        }

        /// <summary>
        /// Метод перевода выражения в постфиксную запись
        /// </summary>
        /// <param name="input">Выражение в инфиксной записи</param>
        /// <param name="expressionBuilder">Функция перевода выражения в постфиксную запись</param>
        /// <returns></returns>
        public static string BuildExpression(string input, Func<string, string> expressionBuilder)
        {
            return expressionBuilder(input);
        }

        /// <summary>
        /// Метод перевода выражения в постфиксную запись
        /// </summary>
        /// <param name="input">Выражение в инфиксной записи</param>
        /// <returns>Выражение в постфиксной записи</returns>
        public static string ExpressionBuilder(string input)
        {
            var expression = string.Empty;
            var operators = new Stack<char>();

            for (int i = 0; i < input.Length; i++) // Перебераем каждый символ во входной строке
            {
                if (IsDelimeter(input[i])) continue; // Разделители пропускаем и переходим к следующему символу 

                if (input[i] == '-' && ((i > 0 && !char.IsDigit(input[i - 1])) || i == 0)) // Проверка на отрицательное число 
                {
                    i++;
                    expression += "-";
                }

                if (char.IsDigit(input[i])) // Если символ - цифра, то считываем все число
                {
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i])) // Читаем до разделителя или оператора
                    {
                        expression += input[i];
                        i++;
                        if (i == input.Length) break;
                    }

                    expression += " "; // Дописываем после числа пробел в строку с выражением
                    i--; // Возвращаемся на один символ назад, к символу перед разделителем
                }

                if (IsOperator(input[i])) // Если символ - оператор
                {
                    if (input[i] == '(') operators.Push(input[i]); // Если символ - открывающая скобка записываем её в стек
                    else if (input[i] == ')') // Если символ - закрывающая скобка
                    {
                        var s = operators.Pop(); // Выписываем все операторы до открывающей скобки в строку

                        while (s != '(')
                        {
                            expression += s.ToString() + ' ';
                            s = operators.Pop();
                        }
                    }
                    else // Любой другой оператор
                    {
                        if (operators.Count > 0) // Если в стеке есть элементы
                            if (GetPriority(input[i]) <= GetPriority(operators.Peek())) // Если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                expression += operators.Pop().ToString() + " "; // То добавляем последний оператор из стека в строку с выражением

                        operators.Push(char.Parse(input[i].ToString())); // Если стек пуст, или же приоритет оператора выше добавляем операторов на вершину стека 
                    }
                }
            } while (operators.Count > 0) expression += operators.Pop() + " ";

            return expression;
        }

        /// <summary>
        /// Метод решения выражения постфиксной записи
        /// </summary>
        /// <param name="expression">Выражение в постфиксной записи</param>
        /// <param name="expressionCalculator">Функция решения выражения постфиксной записи</param>
        /// <returns></returns>
        public static double CalculateExpression(string expression, Func<string, double> expressionCalculator)
        {
            return expressionCalculator(expression);
        }

        /// <summary>
        /// Метод решения выражения постфиксной записи
        /// </summary>
        /// <param name="expression">Выражение в постфиксной записи</param>
        /// <returns>Результат расчета выражения в постфиксной записи </returns>
        public static double OPNExpressionCalculator(string expression)
        {
            var result = string.Empty;
            var elements = expression.Split(' ');

            for (int i = 0; i < elements.Length; i++)
            {
                switch (elements[i])
                {
                    case "+":
                        result = (double.Parse(elements[i - 2]) + double.Parse(elements[i - 1])).ToString();
                        elements[i - 2] = result;
                        for (int j = i - 1; j < elements.Length - 2; j++)
                        {
                            elements[j] = elements[j + 2];
                        }

                        Array.Resize(ref elements, elements.Length - 2);
                        i -= 2;
                        break;
                    case "-":
                        result = (double.Parse(elements[i - 2]) - double.Parse(elements[i - 1])).ToString();
                        elements[i - 2] = result;
                        for (int j = i - 1; j < elements.Length - 2; j++)
                        {
                            elements[j] = elements[j + 2];
                        }

                        Array.Resize(ref elements, elements.Length - 2);
                        i -= 2;
                        break;
                    case "*":
                        result = (double.Parse(elements[i - 2]) * double.Parse(elements[i - 1])).ToString();
                        elements[i - 2] = result;
                        for (int j = i - 1; j < elements.Length - 2; j++)
                        {
                            elements[j] = elements[j + 2];
                        }

                        Array.Resize(ref elements, elements.Length - 2);
                        i -= 2;
                        break;
                    case "/":
                        result = (double.Parse(elements[i - 2]) / double.Parse(elements[i - 1])).ToString();
                        elements[i - 2] = result;
                        for (int j = i - 1; j < elements.Length - 2; j++)
                        {
                            elements[j] = elements[j + 2];
                        }

                        Array.Resize(ref elements, elements.Length - 2);
                        i -= 2;
                        break;
                    case "^":
                        result = (Math.Pow(double.Parse(elements[i - 2]), double.Parse(elements[i - 1]))).ToString();
                        elements[i - 2] = result;
                        for (int j = i - 1; j < elements.Length - 2; j++)
                        {
                            elements[j] = elements[j + 2];
                        }

                        Array.Resize(ref elements, elements.Length - 2);
                        i -= 2;
                        break;
                }
            }

            return double.Parse(elements[0]);
        }

        /// <summary>
        /// Метод возвращает приоритет оператора
        /// </summary>
        /// <param name="s">Оператор</param>
        /// <returns>Возвращает приоритет оператора</returns>
        public static byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                case '^': return 5;
                default: return 6;
            }
        }

        /// <summary>
        /// Проверка на оператор
        /// </summary>
        /// <param name="с">Символ</param>
        /// <returns></returns>
        public static bool IsOperator(char с)
        {
            return "+-/*^()".IndexOf(с) != -1;
        }

        /// <summary>
        /// Проверка на разделитель ("пробел" или "равно")
        /// </summary>
        /// <param name="c">Символ</param>
        /// <returns></returns>
        public static bool IsDelimeter(char c)
        {
            return " =".IndexOf(c) != -1;
        }
    }
}
