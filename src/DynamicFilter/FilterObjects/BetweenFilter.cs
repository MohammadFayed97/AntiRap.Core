namespace AntiRap.Core.DynamicFilter
{
    using System;
    using System.Linq.Expressions;

    internal class BetweenFilter : ObjectFilter
{
    public BetweenFilter(int id, string name)
        : base(id, name)
    {
    }

    public BetweenFilter(int id, FilterOperation filterName) : base(id, filterName)
    {
    }

    /// <inheritdoc/>
    public override Expression<Func<TModel, bool>> GenerateExpression<TModel>(string propertyName, object value1, object value2)
    {
        var greaterThanExpression = GreaterThan.GenerateExpression<TModel>(propertyName, value1, null);
        var lessThanExpression = LessThan.GenerateExpression<TModel>(propertyName, value2, null);

        return PredicateBuilder.And(greaterThanExpression, lessThanExpression);
    }

    /// <inheritdoc/>
    public override bool AllowsType(Type type)
    {
        TypeCode typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.Int32:
                return true;
            case TypeCode.Boolean:
                return true;
            case TypeCode.DateTime:
                return true;
        }

        return false;
    }
}
}
