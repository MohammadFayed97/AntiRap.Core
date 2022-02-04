namespace AntiRap.Core.DynamicFilter;

internal class StringContainsFilter : ObjectFilter
{
    public StringContainsFilter(int id, string name)
        : base(id, name)
    {
    }

    public StringContainsFilter(int id, FilterOperation filterName) : base(id, filterName)
    {
    }

    /// <inheritdoc/>
    public override Expression<Func<TModel, bool>> GenerateExpression<TModel>(string propertyName, object value1, object value2)
    {
        ParameterExpression parameterExpression = Expression.Parameter(typeof(TModel), "e");
        Expression expression = parameterExpression;
        string[] propertyParts = propertyName.Split('.');
        foreach (string propertyPart in propertyParts)
        {
            expression = Expression.PropertyOrField(expression, propertyPart);
        }

        if (expression.Type != typeof(string))
            throw new Exception($"the type for Contains Expression should be string");

        ConstantExpression constantExpression = Expression.Constant($"{value1}");

        var method = expression.Type.GetMethod("Contains", new[] { expression.Type });
        MethodCallExpression body = Expression.Call(expression, method, constantExpression);

        return Expression.Lambda<Func<TModel, bool>>(body, new ParameterExpression[1]
        {
                    parameterExpression
        });
    }

    /// <inheritdoc/>
    public override bool AllowsType(Type type)
    {
        TypeCode typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.String:
                return true;
        }

        return false;
    }
}