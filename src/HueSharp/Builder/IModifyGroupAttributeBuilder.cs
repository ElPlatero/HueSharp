using System.Collections.Generic;
using System.Linq;
using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Groups;

namespace HueSharp.Builder
{
    public interface IModifyGroupAttributeBuilder : IBuilder
    {
        IModifyGroupAttributeBuilder Name(string newName);
        IModifyGroupAttributeBuilder Class(RoomClass roomClass);
    }

    class ModifyGroupAttributeBuilder : IModifyGroupAttributeBuilder
    {
        private readonly int _groupId;
        private string _name;
        private RoomClass? _roomClass;
        private readonly ICollection<int> _lightIds;

        public ModifyGroupAttributeBuilder(int groupId, int mandatoryLightId, params int[] optionalLightIds)
        {
            _groupId = groupId;
            var idList = new HashSet<int> { mandatoryLightId };
            if(optionalLightIds.Any()) idList.UnionWith(optionalLightIds);
            _lightIds = idList;
        }

        public IHueRequest Build()
        {
            var result = new SetGroupRequest { GroupId = _groupId, Lights = _lightIds.ToList() };
            
            if (!string.IsNullOrEmpty(_name)) result.NewName = _name;
            if (_roomClass.HasValue) result.Class = _roomClass.Value;
            return result;
        }

        public IModifyGroupAttributeBuilder Name(string newName)
        {
            if(!string.IsNullOrEmpty(newName)) _name = newName;
            return this;
        }

        public IModifyGroupAttributeBuilder Class(RoomClass roomClass)
        {
            _roomClass = roomClass;
            return this;
        }
    }
}