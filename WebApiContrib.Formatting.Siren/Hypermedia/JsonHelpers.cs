using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiContrib.MediaType.Hypermedia
{
    public static class JsonHelpers
    {
        public static Task WriteJsonToStreamAsync(this Stream stream, object objectToSerialize)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSerialize, jsonSerializerSettings);

            var writer = new StreamWriter(stream);
            return writer.WriteAsync(jsonResult).ContinueWith((result) => { writer.Flush(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static T CreateFromJsonStream<T>(this Stream stream)
        {
            JsonSerializer serializer = new JsonSerializer();
            T data;
            using (StreamReader streamReader = new StreamReader(stream))
            {
                data = (T)serializer.Deserialize(streamReader, typeof(T));
            }

            return data;
        }

        public static T CreateFromJsonString<T>(this string json)
        {
            T data;
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(json)))
            {
                data = CreateFromJsonStream<T>(stream);
            }

            return data;
        }

        public static T CreateFromJsonFile<T>(this string fileName)
        {
            T data;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                data = CreateFromJsonStream<T>(fileStream);
            }

            return data;
        }
    }
}