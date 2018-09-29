namespace HueSharp.Builder
{
    public interface ILightStateAdjustmentBuilder
    {
        IModifyLightStateBuilder By(byte ammount);
    }
}