namespace HueSharp.Builder
{
    public interface IModifyLightAttributeBuilder : IBuilder
    {
        IBuilder Name(string newName);
    }
}