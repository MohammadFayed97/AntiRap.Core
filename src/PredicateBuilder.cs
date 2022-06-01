namespace AntiRap.Core
{
    using System;
    using System.Linq.Expressions;

    public static class PredicateBuilder
    {
        /// <summary>
        /// Concat between to Expressions by && operation
        /// </summary>
        /// <typeparam name="T">The type of the element in expressions.</typeparam>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            var body = Expression.AndAlso(Expression.Invoke(expr1, param), Expression.Invoke(expr2, param));

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
