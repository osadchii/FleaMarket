using Newtonsoft.Json;

namespace FleaMarket.Infrastructure.Extensions;

public static class CommonExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T FromJson<T>(this string str)
    {
        return JsonConvert.DeserializeObject<T>(str);
    }
}