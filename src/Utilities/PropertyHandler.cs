namespace AntiRap.Core.Utilities
{
    using AntiRap.Core.Abstractions;
    using System;
    using System.Reflection;

    public class PropertyHandler : IPropertyHandler
    {
        public object GetPropertyValue(object modelObject, string propertName) => GetProperty(modelObject, propertName)?.GetValue(modelObject, null);

        public Type GetPropertyType(object model, string propertyName) => GetProperty(model, propertyName)?.PropertyType;
        public Type GetPropertyType<TModel>(string propertyName) => GetProperty<TModel>(propertyName).PropertyType;

        public void SetPropertyValue(object model, string propertyName, object value) => GetProperty(model, propertyName)?.SetValue(model, value);

        public PropertyInfo GetProperty(object model, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            return model?.GetType().GetProperty(propertyName);
        }
        public PropertyInfo GetProperty<TModel>(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            return typeof(TModel).GetProperty(propertyName);
        }
    }
}
