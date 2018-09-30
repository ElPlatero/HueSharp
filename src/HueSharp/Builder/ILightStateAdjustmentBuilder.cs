namespace HueSharp.Builder
{
    public interface ILightStateAdjustmentBuilder
    {
        IModifyLightStateBuilder By(ushort amount);
    }
}