using HueSharp.Messages;

namespace HueSharp.Builder
{
    public interface IModifyGroupStatusBuilder : IBuilder
    {

    }

    class ModifyGroupStatusBuilder : IModifyGroupStatusBuilder
    {
        private readonly int _groupId;

        public ModifyGroupStatusBuilder(int groupId)
        {
            _groupId = groupId;
        }

        public IHueRequest Build()
        {
            throw new System.NotImplementedException();
        }
    }
}