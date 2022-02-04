namespace AntiRap.Core.DynamicFilter;

internal class EqualFilter : ObjectFilter
{
    public EqualFilter(int id, string name)
        : base(id, name)
    {
    }

    public EqualFilter(int id, FilterOperation filterName) : base(id, filterName)
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
            Console.WriteLine(expression);
            expression = Expression.PropertyOrField(expression, propertyPart);
        }

        UnaryExpression unaryExpression = null;
        if (expression.Type.IsEnum)
        {
            object enumValue = Enum.Parse(expression.Type, value1.ToString());
            ConstantExpression expression2 = Expression.Constant(Convert.ToInt32(enumValue));
            unaryExpression = Expression.ConvertChecked(expression2, expression.Type);
        }
        else
        {
            if (expression.Type != value1.GetType()) value1 = Convert.ChangeType(value1, expression.Type);
            unaryExpression = Expression.ConvertChecked(Expression.Constant(value1), expression.Type);
        }
        BinaryExpression body = Expression.Equal(expression, unaryExpression);

        return Expression.Lambda<Func<TModel, bool>>(body, new ParameterExpression[1]
        {
                parameterExpression
        });


    }

    /// <inheritdoc/>
    public override bool AllowsType(Type type)
    {
        if (type.IsEnum)
            return true;

        TypeCode typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.Int32:
                return true;
            case TypeCode.Boolean:
                return true;
            case TypeCode.DateTime:
                return true;
            case TypeCode.String:
                return true;
        }

        return false;
    }
}
