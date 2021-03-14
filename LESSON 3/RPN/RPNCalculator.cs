using System;
using System.Collections.Generic;

namespace LESSON_3.RPN
{
    /// <summary>
    /// Класс расчета ОПН
    /// </summary>
    public class RPNCalculator : IRPNCalculator
    {
        /// <summary>
        /// Метод решения выражения постфиксной записи
        /// </summary>
        /// <param name="input">Выражение в инфиксной записи</param>
        /// <returns>Результат расчета выражения</returns>
        public double Calculate(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException("input");
            try
            {
                var expression = GetExpression(input);
                return CalculateExpression(expression);
            }
            catch (Exception e)
            {
                throw new Exception($"Возникла ошибка в процессе расчета выражения: {e.Message}", e);
            }
        }

        /// <summary>
        /// Метод перевода выражения в постфиксную запись
        /// </summary>
        /// <param name="input">Выражение в инфиксной записи</param>
        /// <returns>Выражение в постфиксной записи</returns>
        public string GetExpression(string input)
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
        /// <returns>Результат расчета выражения в постфиксной записи </returns>
        public double CalculateExpression(string expression)
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
        public byte GetPriority(char s)
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
        public bool IsOperator(char с)
        {
            return "+-/*^()".IndexOf(с) != -1;
        }

        /// <summary>
        /// Проверка на разделитель ("пробел" или "равно")
        /// </summary>
        /// <param name="c">Символ</param>
        /// <returns></returns>
        public bool IsDelimeter(char c)
        {
            return " =".IndexOf(c) != -1;
        }

    }
}
