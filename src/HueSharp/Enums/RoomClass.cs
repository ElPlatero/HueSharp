using HueSharp.Converters;
using Newtonsoft.Json;
using System.ComponentModel;

namespace HueSharp.Enums
{
    [JsonConverter(typeof(DescriptionConverter))]
    public enum RoomClass : byte
    {
        [Description("Living room")]
        LivingRoom = 1,
        [Description("Kitchen")]
        Kitchen = 2,
        [Description("Dining")]
        Dining = 3,
        [Description("Bedroom")]
        Bedroom = 4,
        [Description("Kids bedroom")]
        BedroomKids = 5,
        [Description("Bathroom")]
        Bathroom = 6,
        [Description("Nursery")]
        Nursery = 7,
        [Description("Recreation")]
        Recreation = 8,
        [Description("Office")]
        Office = 9,
        [Description("Gym")]
        Gym = 10,
        [Description("Hallway")]
        Hallway = 11,
        [Description("Toilet")]
        Toilet = 12,
        [Description("Front door")]
        FrontDoor = 13,
        [Description("Garage")]
        Garage = 14,
        [Description("Terrace")]
        Terrace = 15,
        [Description("Garden")]
        Garden = 16,
        [Description("Driveway")]
        Driveway = 17,
        [Description("Carport")]
        Carport = 18,
        [Description("Other")]
        Other = 255
    }
}
