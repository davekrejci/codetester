using System;
using System.Collections.Generic;
using Codetester.Dtos;
using Codetester.Models;
using HashidsNet;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Codetester
{
    public class QuestionInstanceAttemptTurnInDtoJsonConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new QuestionInstanceAttemptTurnInDtoSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(QuestionInstanceAttemptTurnInDto));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            System.Console.WriteLine(jo.ToString());
            switch (jo["questionType"].Value<string>())
            {
                case QuestionType.MULTI_CHOICE:
                    return JsonConvert.DeserializeObject<MultiChoiceQuestionInstanceAttemptTurnInDto>(jo.ToString(), SpecifiedSubclassConversion);
                case QuestionType.FILL_IN_CODE:
                    return JsonConvert.DeserializeObject<FillInCodeQuestionInstanceAttemptTurnInDto>(jo.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }

    public class QuestionInstanceAttemptTurnInDtoSpecifiedConcreteClassConverter : DefaultContractResolver
{
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (typeof(QuestionInstanceAttemptTurnInDto).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
        return base.ResolveContractConverter(objectType);
    }
}

}