using HueSharp.Messages;
using HueSharp.Messages.Lights;

namespace HueSharp.Builder
{
    public class SetLightAttributesBuilder : IModifyLightAttributeBuilder
    {
        private readonly int _lightId;
        private string _name;

        public SetLightAttributesBuilder(int lightId)
        {
            _lightId = lightId;
        }

        public IHueRequest Build()
        {
            var request = new SetLightAttributesRequest(_lightId);

            if (!string.IsNullOrEmpty(_name)) request.NewName = _name;
            return request;
        }

        public IBuilder Name(string newName)
        {
            if (!string.IsNullOrEmpty(newName)) _name = newName;
            return this;
        }
    }
}