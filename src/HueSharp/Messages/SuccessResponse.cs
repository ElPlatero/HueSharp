using System.Collections.Generic;

namespace HueSharp.Messages
{
    public class SuccessResponse : Dictionary<string, object>, IHueResponse
    {
        public void AddSetProperty(string name, object value)
        {
            Add(name, value);
        }
    }
}
