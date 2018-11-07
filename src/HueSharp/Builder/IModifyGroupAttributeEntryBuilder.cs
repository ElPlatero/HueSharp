using System;
using System.Linq;
using HueSharp.Messages;
using HueSharp.Messages.Groups;

namespace HueSharp.Builder
{
    public interface IModifyGroupAttributeEntryBuilder
    {
        IModifyGroupAttributeBuilder UseTheseLights(int mandatoryLightId, params int[] optionalLights);
        IModifyGroupAttributeBuilder UseLightsInResponse();
    }

    class ModifyGroupAttributeEntryBuilder : IModifyGroupAttributeEntryBuilder
    {
        private readonly int _groupId;
        private readonly GetGroupResponse _getGroupResponse;

        public ModifyGroupAttributeEntryBuilder(int groupId, GetGroupResponse getGroupResponse = null)
        {
            _groupId = groupId;
            _getGroupResponse = getGroupResponse;
        }

        public IModifyGroupAttributeBuilder UseTheseLights(int mandatoryLightId, params int[] optionalLights)
        {
            return new ModifyGroupAttributeBuilder(_groupId, mandatoryLightId, optionalLights);
        }

        public IModifyGroupAttributeBuilder UseLightsInResponse()
        {
            if(_getGroupResponse == null) throw new InvalidOperationException("Response has not been set. Cannot use lights from response.");
            return new ModifyGroupAttributeBuilder(_groupId, _getGroupResponse.LightIds[0], _getGroupResponse.LightIds.Skip(1).ToArray());
        }
    }
}