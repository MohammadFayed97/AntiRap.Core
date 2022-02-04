namespace AntiRap.Core.DynamicFilter;

internal class LessThanFilter : ObjectFilter
{
    public LessThanFilter(int id, string name)
        : base(id, name)
    {
    }

    public LessThanFilter(int id, FilterOperation filterName) : base(id, filterName)
    {
    }

    /// <inheritdoc/>
    public override Expression<Func<TModel, bool>> GenerateExpression<TModel>(string propertyName, object value1, object value2)
    {
        ParameterExpression parameterExpression = Expression.Parameter(typeof(TModel), "e");
        Expression expression = parameterExpression;
        string[] array = propertyName.Split('.');
        foreach (string propertyOrFieldName in array)
        {
            expression = Expression.PropertyOrField(expression, propertyOrFieldName);
        }

        UnaryExpression unaryExpression = null;
        BinaryExpression binaryExpression = null;
        if (expression.Type.IsEnum)
        {
            object enumValue = Enum.Parse(expression.Type, value1.ToString());
            ConstantExpression right = Expression.Constant(Convert.ToInt32(enumValue));
            UnaryExpression left = Expression.ConvertChecked(expression, typeof(int));
            binaryExpression = Expression.LessThan(left, right);
        }
        else
        {
            if (expression.Type != value1.GetType()) value1 = Convert.ChangeType(value1, expression.Type);
            unaryExpression = Expression.ConvertChecked(Expression.Constant(value1), expression.Type);
            binaryExpression = Expression.LessThan(expression, unaryExpression);
        }

        Console.WriteLine(binaryExpression.ToString());

        return Expression.Lambda<Func<TModel, bool>>(binaryExpression, new ParameterExpression[1]
        {
                    parameterExpression
        });
    }

    /// <inheritdoc/>
    public override bool AllowsType(Type type)
    {
        if (type.IsEnum) return false;

        TypeCode typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.Int32:
                return true;
            case TypeCode.DateTime:
                return true;
        }

        return false;
    }
}