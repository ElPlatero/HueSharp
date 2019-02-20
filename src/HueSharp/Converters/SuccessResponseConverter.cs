using System;
using Newtonsoft.Json;
using HueSharp.Messages;
using System.Collections.Generic;
using System.Linq;

namespace HueSharp.Converters
{
    class SuccessResponseConverter : JsonConverter
    {
        private readonly string _address;

        public SuccessResponseConverter(string address)
        {
            _address = address;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SuccessResponse);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new SuccessResponse();
            while(reader.Read())
            {
                if(reader.TokenType == JsonToken.PropertyName && reader.Value.ToString().Equals("success") && reader.Read())
                {
                    //invalid response when deleting groups
                    if(reader.TokenType == JsonToken.String)
                    {
                        var strings = reader.Value.ToString().Split(' ');
                        result.AddSetProperty(strings[0].Substring(strings[0].LastIndexOf('/')+1), strings[1]);
                        continue;
                    }


                    while(reader.TokenType != JsonToken.PropertyName) reader.Read();
                    string propertyName = _address.Length + 2 <= reader.Value.ToString().Length ?
                        reader.Value.ToString().Substring(_address.Length + 2, reader.Value.ToString().Length - _address.Length - 2) :
                        reader.Value.ToString().Trim('/');
                    reader.Read();

                    object objectValue;
                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        var objectList = new List<object>();
                        while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                        {
                            if (propertyName == "lights") objectList.Add(Convert.ToInt32(reader.Value));
                            else objectList.Add(reader.Value.ToString());
                        }
                        objectValue = propertyName == "lights" ? objectList.OfType<int>().ToList() : (object)objectList;
                    }
                    else if (reader.TokenType == JsonToken.StartObject) objectValue = serializer.Deserialize<Command>(reader);
                    else objectValue = reader.Value;
                    result.AddSetProperty(propertyName, objectValue);
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
