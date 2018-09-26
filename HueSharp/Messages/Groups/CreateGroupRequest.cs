using HueSharp.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HueSharp.Messages.Groups
{
    public class CreateGroupRequest : SetGroupRequest
    {
        [JsonProperty(PropertyName = "type"), JsonConverter(typeof(StringEnumConverter))]
        public GroupType Type { get; set; }
        public bool ShouldSerializeType() => true;

        public override string Address => base.Address.Substring(0, base.Address.Length-2);

        public CreateGroupRequest(string name, params int[] lightIds) : this(name, GroupType.LightGroup, RoomClass.Other, lightIds) { }
        public CreateGroupRequest(string name, GroupType groupType, params int[] lightIds) : this(name, groupType, RoomClass.Other, lightIds) { }
        public CreateGroupRequest(string name, GroupType groupType, RoomClass roomClass, params int[] lightIds) : base(0, new List<int>(lightIds), roomClass)
        {
            if (!lightIds.Any() && groupType != GroupType.Room)
                throw new ArgumentException("Only groups of type \"room\" can be created without any lights in them.");

            _method = HttpMethod.Post;
            NewName = name;
            Type = groupType;
        }
    }
}
