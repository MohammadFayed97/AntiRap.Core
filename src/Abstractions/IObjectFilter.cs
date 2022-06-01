namespace AntiRap.Core.Abstractions
{
    using System;
    using System.Linq.Expressions;

    public interface IObjectFilter
    {
        Expression<Func<TModel, bool>> GenerateExpression<TModel>(string propertyName, dynamic value1, dynamic value2);
    }
}

