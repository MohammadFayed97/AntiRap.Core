namespace AntiRap.Core.DynamicFilter
{
    using AntiRap.Core.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public abstract class ObjectFilter : IObjectFilter
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public FilterOperation FilterName { get; private set; }


        public new static readonly ObjectFilter Equals = new EqualFilter(1, "Equals");

        public static readonly ObjectFilter NotEquals = new NotEqualsFilter(2, "NotEquals");

        public static readonly ObjectFilter GreaterThan = new GreaterThanFilter(3, "GreaterThan");

        public static readonly ObjectFilter LessThan = new LessThanFilter(4, "LessThan");

        public static readonly ObjectFilter Contains = new StringContainsFilter(7, "Contains");

        public static readonly ObjectFilter Between = new BetweenFilter(8, "Between");

        protected ObjectFilter(int id, string name)
        {
            Id = id;
            Name = name;
            FilterName = Enum.Parse<FilterOperation>(name, true);
        }
        protected ObjectFilter(int id, FilterOperation filterName)
        {
            Id = id;
            Name = filterName.ToString();
            FilterName = filterName;
        }

        /// <summary>
        /// Generate Expression for this <see cref="ObjectFilter"/>
        /// </summary>
        /// <param name="propertyName">used to left hand side for expression.</param>
        /// <param name="value1">used to right hand side for expression.</param>
        /// <param name="value2">used if expression need it.
        ///     ex: in BetweenFilter need value1 for greater than expression and for less than expression value2 </param>
        public abstract Expression<Func<TModel, bool>> GenerateExpression<TModel>(string propertyName, object value1, object? value2 = null);

        /// <summary>
        /// Indicate what type that allowed for this <see cref="ObjectFilter"/>
        /// </summary>
        /// <param name="type">the type to indicate if allowed or not for this <see cref="ObjectFilter"/></param>
        /// <returns>true if type is allowed.</returns>
        public abstract bool AllowsType(Type type);

        /// <summary>
        /// Get all instaces of filter objects that declared in <see cref="ObjectFilter"/>
        /// </summary>
        /// <returns>IEnumerable of <see cref="ObjectFilter"/>.</returns>
        public static IEnumerable<ObjectFilter> GetAll()
        {
            FieldInfo[] fields = typeof(ObjectFilter).GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            return fields.Select((FieldInfo f) => f.GetValue(null)).Cast<ObjectFilter>();
        }

        /// <summary>
        /// Get instance of ObjectFilter by <see cref="Id"/>
        /// </summary>
        /// <param name="value">Id for required ObjectFilter</param>
        public static ObjectFilter FromValue(int value) => GetAll().FirstOrDefault(o => o.Id == value);

        /// <summary>
        /// Get instance of ObjectFilter by <see cref="Name"/>
        /// </summary>
        /// <param name="name">Name for required ObjectFilter</param>
        public static ObjectFilter FromName(string name) => GetAll().FirstOrDefault(o => o.Name == name);

        /// <summary>
        /// Get instance of ObjectFilter by <see cref="FilterOperation"/>
        /// </summary>
        /// <param name="filterOperation">FilterOperation for required ObjectFilter</param>
        public static ObjectFilter FromFilterName(FilterOperation filterOperation) => GetAll().FirstOrDefault(o => o.FilterName == filterOperation);
    }
}
