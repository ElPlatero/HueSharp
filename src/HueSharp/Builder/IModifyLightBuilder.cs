namespace HueSharp.Builder
{
    public interface IModifyLightBuilder
    {
        IModifyLightAttributeBuilder Attributes { get; }
        IModifyLightStateBuilder Status { get; }
    }

    class ModifyLightBuilder : IModifyLightBuilder
    {
        private readonly int _lightId;

        public ModifyLightBuilder(int lightId)
        {
            _lightId = lightId;
        }
        public IModifyLightAttributeBuilder Attributes => new SetLightAttributesBuilder(_lightId);
        public IModifyLightStateBuilder Status => new SetLightStateRequestBuilder(_lightId);
    }
}