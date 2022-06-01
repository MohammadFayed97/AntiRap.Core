namespace AntiRap.Core.Utilities
{
    using System;
    using System.Reflection;

    public static class PropertyHandler
    {
        public static object GetPropertyValue(object modelObject, string propertName) => GetProperty(modelObject, propertName)?.GetValue(modelObject, null);

        public static Type GetPropertyType(object model, string propertyName) => GetProperty(model, propertyName)?.PropertyType;
        public static Type GetPropertyType<TModel>(string propertyName) => GetProperty<TModel>(propertyName).PropertyType;

        public static void SetPropertyValue(object model, string propertyName, object value) => GetProperty(model, propertyName)?.SetValue(model, value);

        public static PropertyInfo GetProperty(object model, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            return model?.GetType().GetProperty(propertyName);
        }
        public static PropertyInfo GetProperty<TModel>(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return null;

            return typeof(TModel).GetProperty(propertyName);
        }
    }
}
