using System;
using System.Collections.Generic;
using System.Linq;
using HueSharp.Enums;
using HueSharp.Messages;
using HueSharp.Messages.Groups;

namespace HueSharp.Builder
{
    public interface ICreateGroupBuilder : IBuilder
    {
        ICreateGroupBuilder Name(string name);
        ICreateGroupBuilder Class(GroupType groupType);
    }

    class CreateGroupBuilder : ICreateGroupBuilder
    {
        private readonly IEnumerable<int> _lightIds;
        private string _name;
        private GroupType? _groupType;

        public CreateGroupBuilder(IEnumerable<int> lightIds)
        {
            _lightIds = lightIds;
        }

        public IHueRequest Build()
        {
            if (string.IsNullOrEmpty(_name)) throw new InvalidOperationException("New group's name must not be empty. Use Name() to set the name of the new group.");
            return new CreateGroupRequest(_name, _groupType ?? GroupType.Room, _lightIds.ToArray());
        }

        public ICreateGroupBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public ICreateGroupBuilder Class(GroupType type)
        {
            _groupType = type;
            return this;
        }
    }
}