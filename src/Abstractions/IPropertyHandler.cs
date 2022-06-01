namespace AntiRap.Core.Abstractions
{
    using System;
    using System.Reflection;

    public interface IPropertyHandler
    {
        object GetPropertyValue(object modelObject, string propertName);

        Type GetPropertyType(object model, string propertyName);
        Type GetPropertyType<TModel>(string propertyName);

        void SetPropertyValue(object model, string propertyName, object value);

        PropertyInfo GetProperty(object model, string propertyName);
        PropertyInfo GetProperty<TModel>(string propertyName);
    }
}
