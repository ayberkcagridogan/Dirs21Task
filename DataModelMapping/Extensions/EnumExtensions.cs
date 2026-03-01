using FluentResults;

namespace DataModelMapping.Extensions;

public static class EnumExtensions
{
    public static Result<TEnum> ToEnum<TEnum>(this string value)
        where TEnum : struct, Enum
    {
        if (value.TryParseEnum<TEnum>(out var result))
            return result;
    
        return Result.Fail<TEnum>(
            $"Invalid value '{value}' for enum {typeof(TEnum).Name}");
    }

    public static bool TryParseEnum<TEnum>(this string value, out TEnum result)
        where TEnum : struct, Enum
    {
        if (Enum.TryParse(value, ignoreCase: true, out result)
            && Enum.IsDefined(typeof(TEnum), result))
        {
            return true;
        }

        result = default;
        return false;
    }


}