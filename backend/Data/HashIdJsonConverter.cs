using System;
using HashidsNet;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Codetester
{
    public class HashIdJsonConverter : JsonConverter<int>
    {
        Hashids hashids = new Hashids("!2ZX5i3KVR@S79*MEGB&j^FLwapS2Kh7hfpc!^", 16);// Add salt 

        public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);

            return hashids.Decode(str)[0];
        }

        public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, hashids.Encode(value));
        }

    }

}