using System;
using System.Collections.Generic;
using System.Linq;
using HueSharp.Messages;
using HueSharp.Messages.Groups;

namespace HueSharp.Builder
{
    public interface ICreateGroupInitBuilder
    {
        ICreateGroupBuilder Duplicate(IHueResponse getGroupResponse);
        ICreateGroupBuilder New(int mandatoryLight, params int[] optionalLights);
    }

    class CreateGroupInitBuilder : ICreateGroupInitBuilder
    {
        public ICreateGroupBuilder Duplicate(IHueResponse response)
        {
            if (response is GetGroupResponse getGroupResponse)
            {
                return new CreateGroupBuilder(getGroupResponse.LightIds);
            }

            throw new InvalidOperationException("Duplicate must be a response containing an existing group.");
        }

        public ICreateGroupBuilder New(int mandatoryLight, params int[] optionalLights)
        {
            var list = new HashSet<int> { mandatoryLight };
            if (optionalLights != null && optionalLights.Any()) list.UnionWith(optionalLights);

            return new CreateGroupBuilder(list);
        }
    }

    public class CreateSceneInitBuilder
    {
        public CreateSceneAddSubjectsBuilder New(string name)
        {
            return new CreateSceneAddSubjectsBuilder(name);
        }
    }

    public class CreateSceneAddSubjectsBuilder
    {
        private readonly string _name;

        public CreateSceneAddSubjectsBuilder(string name)
        {
            _name = name;
        }

        public CreateSceneChooseTypeBuilder For
        {
            get { return new CreateSceneChooseTypeBuilder(_name); }
        }
    }

    public class CreateSceneChooseTypeBuilder
    {
        private readonly string _name;

        public CreateSceneChooseTypeBuilder(string name)
        {
            _name = name;
        }

        public CreateSceneFromGroupBuilder Group(int groupId)
        {
            return new CreateSceneFromGroupBuilder(_name, groupId);
        }

        public CreateSceneFromLightsBuilder Light(int lightId)
        {
            return new CreateSceneFromLightsBuilder(_name, lightId);
        }

    }

    public class CreateSceneFromLightsBuilder
    {
        private readonly string _name;
        private readonly int _lightId;

        public CreateSceneFromLightsBuilder(string name, int lightId)
        {
            _name = name;
            _lightId = lightId;
        }
    }

    public class CreateSceneFromGroupBuilder
    {
        private readonly string _name;
        private int _groupId;

        public CreateSceneFromGroupBuilder(string name, int groupId)
        {
            _name = name;
            _groupId = groupId;
        }
    }
}