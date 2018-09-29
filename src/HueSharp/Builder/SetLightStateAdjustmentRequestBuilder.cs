using System;

namespace HueSharp.Builder
{
    public class SetLightStateAdjustmentRequestBuilder : ILightStateAdjustmentBuilder
    {
        private readonly Action<int> _adjustAction;
        private readonly bool _isIncrease;
        private readonly IModifyLightStateBuilder _builder;

        public SetLightStateAdjustmentRequestBuilder(IModifyLightStateBuilder builder, Action<int> adjustAction, bool isIncrease)
        {
            _builder = builder;
            _adjustAction = adjustAction;
            _isIncrease = isIncrease;
        }

        public IModifyLightStateBuilder By(byte ammount)
        {
            _adjustAction(_isIncrease ? ammount : -ammount);
            return _builder;
        }
    }
}