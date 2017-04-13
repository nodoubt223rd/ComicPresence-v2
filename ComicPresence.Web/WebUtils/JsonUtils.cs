using Newtonsoft.Json;

namespace ComicPresence.Web.WebUtils
{
    public static class JsonUtils
    {
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}