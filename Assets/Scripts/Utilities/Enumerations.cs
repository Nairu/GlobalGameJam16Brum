using System.Reflection;
using System;
using System.ComponentModel;

public static class Enumerations {

    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute),
            false);

        if (attributes != null &&
            attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToString();
    }

    public static T GetEnumFromDescription<T>(this string value)
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException();
        foreach (var field in type.GetFields())
        {
            var attribute = Attribute.GetCustomAttribute(field,
                                       typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null)
            {
                if (attribute.Description == value)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == value)
                    return (T)field.GetValue(null);
            }
        }

        throw new ArgumentException("Not Found", "value");
    }
}
