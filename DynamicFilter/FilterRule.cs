namespace AntiRap.Core.DynamicFilter;

public class FilterRule<TModel>
{
    public Guid Id { get; private set; }
    public string PropertyName { get; private set; }
    public int Order { get; private set; }

    public ObjectFilter FilterType { get; set; }
    public Type ExpectedValueType { get; private set; }

    public dynamic? FilterValue1 { get; private set; } = null;
    public dynamic? FilterValue2 { get; private set; } = null;

    public bool IsNullable { get; private set; }
    public bool IsApplied { get; set; }

    public FilterRule(Guid id, string propertyName, Type propertyType, ObjectFilter objectFilter)
    {
        PropertyName = propertyName;
        FilterType = objectFilter;
        Id = id;

        UpdatePropertyType(propertyType);
    }
    public FilterRule(Guid id, string propertyName, Type propertyType, ObjectFilter objectFilter, bool isApplied)
    {
        PropertyName = propertyName;
        FilterType = objectFilter;
        Id = id;
        IsApplied = isApplied;

        UpdatePropertyType(propertyType);
    }

    public void UpdateFilterValue1(object value1) => FilterValue1 = value1;
    public void UpdateFilterValue2(object value2) => FilterValue2 = value2;
    public void UpdateFilterOrder(int order) => Order = order;
    public Expression<Func<TModel, bool>> GenerateExpression() => FilterType.GenerateExpression<TModel>(PropertyName, FilterValue1, FilterValue2);

    private void UpdatePropertyType(Type propertyType)
    {
        if (propertyType is null)
            return;

        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            propertyType = Nullable.GetUnderlyingType(propertyType);
            IsNullable = true;
        }

        ExpectedValueType = propertyType;

        if (propertyType.IsEnum) FilterValue1 = Enum.GetNames(propertyType)[0];
        else
        {
            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Int16:
                    FilterValue1 = default(System.Int16);
                    break;
                case TypeCode.Int32:
                    FilterValue1 = default(System.Int32);
                    break;
                case TypeCode.Int64:
                    FilterValue1 = default(System.Int64);
                    break;
                case TypeCode.UInt16:
                    FilterValue1 = default(System.UInt16);
                    break;
                case TypeCode.UInt32:
                    FilterValue1 = default(System.UInt32);
                    break;
                case TypeCode.UInt64:
                    FilterValue1 = default(System.UInt64);
                    break;
                case TypeCode.Double:
                    FilterValue1 = default(System.Double);
                    break;
                case TypeCode.Decimal:
                    FilterValue1 = default(System.Decimal);
                    break;
                case TypeCode.Byte:
                    FilterValue1 = default(System.Byte);
                    break;
                case TypeCode.Boolean:
                    FilterValue1 = false;
                    break;
                case TypeCode.String:
                    FilterValue1 = "";
                    break;
                case TypeCode.DateTime:
                    FilterValue1 = DateTime.UtcNow;
                    break;

                // TODO: Some types might be possible
                case TypeCode.Object:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Single:
                    throw new Exception("Unsupported property type for filtering");

            }
        }
        FilterValue2 = FilterValue1;
    }

}