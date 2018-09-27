using HueSharp.Enums;
using Newtonsoft.Json;

namespace HueSharp.Messages
{
    public class Condition
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "operator")]
        public ConditionOperator Operator { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string CompareValue { get; set; }
    }
}
