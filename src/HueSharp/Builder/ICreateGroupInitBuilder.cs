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
}