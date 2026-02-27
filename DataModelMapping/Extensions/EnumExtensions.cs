using System.ComponentModel;
using DataModelMapping.Mapping;

namespace DataModelMapping.Extensions;

public static class EnumExtensions
{
    public static MappingType FromDescription(string description)
    {
        foreach (var field in typeof(MappingType).GetFields())
        {
            var attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute != null && attribute.Description == description)
                return (MappingType)field.GetValue(null)!;
        }

        throw new ArgumentException("Not found.", nameof(description));
    }
}