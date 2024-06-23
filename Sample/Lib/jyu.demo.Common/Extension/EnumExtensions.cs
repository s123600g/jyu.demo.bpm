namespace jyu.demo.Common.Extension;

using System.Reflection;
using System.Runtime.Serialization;

public static class EnumExtensions
{
    /// <summary>
    /// Enum 擴充方法，取得 EnumMemberAttribute 的資料值
    /// </summary>
    /// <param name="enumEntity"></param>
    /// <returns></returns>
    public static string GetEnumMemberAttributeValue(
        this Enum enumEntity
    )
    {
        return enumEntity.GetType()?.GetMember(enumEntity.ToString())?
            .First()?.GetCustomAttribute<EnumMemberAttribute>()?.Value;
    }

    /// <summary>
    /// 字串轉列舉
    /// </summary>
    /// <returns></returns>
    public static T ConvertStrToEnum<T>(
        this string value
    )
        where T : struct // 一定要約束T
    {
        if (
            Enum.TryParse(value, out T enumEntity)
        )
        {
            return enumEntity;
        }
        else
        {
            throw new ArgumentException();
        }
    }
}