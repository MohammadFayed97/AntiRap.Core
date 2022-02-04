namespace AntiRap.Core.Abstractions;

public interface IObjectFilter
{
    Expression<Func<TModel, bool>> GenerateExpression<TModel>(string propertyName, dynamic value1, dynamic value2);
}
