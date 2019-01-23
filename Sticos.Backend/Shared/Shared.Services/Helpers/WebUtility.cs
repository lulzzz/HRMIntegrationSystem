using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Shared.Services.Helpers
{
    public static class WebUtility
    {
        public static string GetQueryString(object T)
        {
            var result = new List<string>();
            var props = T.GetType().GetProperties().Where(p => p.GetValue(T, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(T, null);
                var enumerable = value as ICollection;
                if (enumerable != null)
                {
                    result.AddRange(from object v in enumerable select string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(v.ToString())));
                }
                else
                {
                    result.Add(string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(value.ToString())));
                }
            }
            return string.Join("&", result.ToArray());
        }

        public static T DeserializeObject<T>(string data)
        {
            var deserializedObject = JsonConvert.DeserializeObject<T>(data);
            return deserializedObject;
        }
    }
}