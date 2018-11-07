using System;
using HueSharp.Messages;
using HueSharp.Messages.Groups;

namespace HueSharp.Builder
{
    public interface IModifyGroupBuilder
    {
        IModifyGroupAttributeEntryBuilder Attributes { get; }
        IModifyGroupStatusBuilder Status { get; }
    }

    class ModifyGroupBuilder : IModifyGroupBuilder
    {
        private readonly int _groupId;
        private IModifyGroupAttributeEntryBuilder _attributeEntryBuilder;

        public ModifyGroupBuilder(int groupId)
        {
            _groupId = groupId;
        }

        public ModifyGroupBuilder(IHueResponse response)
        {
            if (response is GetGroupResponse getGroupResponse)
            {
                _groupId = getGroupResponse.Id;
                _attributeEntryBuilder = new ModifyGroupAttributeEntryBuilder(_groupId, getGroupResponse);
            }
            throw new InvalidOperationException($"Cannot initialize request with this response ({response.GetType().Name}).");
        }

        public IModifyGroupAttributeEntryBuilder Attributes => _attributeEntryBuilder ?? new ModifyGroupAttributeEntryBuilder(_groupId);
        public IModifyGroupStatusBuilder Status => new ModifyGroupStatusBuilder(_groupId);
    }
}