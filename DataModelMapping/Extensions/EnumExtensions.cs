using System.ComponentModel;
using DataModelMapping.Mapping;
using FluentResults;

namespace DataModelMapping.Extensions;

public static class EnumExtensions
{
    public static Result<MappingType> FromDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Fail("Description cannot be null or empty.");

        foreach (var field in typeof(MappingType).GetFields())
        {
            var attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;

            if (attribute != null && attribute.Description == description)
            {
                var value = field.GetValue(null);
                if(value is MappingType mappingType)
                    Result.Ok(mappingType);
            }          
        }
        return Result.Fail($"Mapping Type with description '{description}' was not found.");
    }
}