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

        /// <summary>
        /// The amount that the current value should be increased or decreased by.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public IModifyLightStateBuilder By(ushort amount)
        {
            _adjustAction(_isIncrease ? amount : -amount);
            return _builder;
        }
    }
}