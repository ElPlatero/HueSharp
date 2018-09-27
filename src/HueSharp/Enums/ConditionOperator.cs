using System.ComponentModel;

namespace HueSharp.Enums
{
    public enum ConditionOperator
    {
        [Description("eq")]
        EqualsCondition,
        [Description("gt")]
        GreatherThanCondition,
        [Description("lt")]
        LessThanCondition,
        [Description("dx")]
        ValueChangedCondition
    }
}
