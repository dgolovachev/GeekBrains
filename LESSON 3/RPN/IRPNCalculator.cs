namespace LESSON_3.RPN
{
    /// <summary>
    /// Интерфейс расчета ОПН
    /// </summary>
    public interface IRPNCalculator
    {
        /// <summary>
        /// Метод перевода выражения в постфиксную запись
        /// </summary>
        /// <param name="input">Выражение в инфиксной записи</param>
        /// <returns>Выражение в постфиксной записи</returns>
        string GetExpression(string input);

        /// <summary>
        /// Метод решения выражения постфиксной записи
        /// </summary>
        /// <param name="expression">Выражение в постфиксной записи</param>
        /// <returns>Результат расчета выражения в постфиксной записи </returns>
        double CalculateExpression(string expression);

    }
}
