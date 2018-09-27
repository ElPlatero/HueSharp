using System.ComponentModel;

namespace HueSharp.Enums
{
    public enum ErrorCode
    {
        [Description("This will be returned if an invalid username is used in the request, or if the username does not have the rights to modify the resource.")]
        UnautorizedUser = 1,
        [Description("This will be returned if the body of the message contains invalid JSON.")]
        InvalidJson = 2,
        [Description("This will be returned if the addressed resource does not exist. E.g. the user specifies a light ID that does not exist.")]
        InvalidResource = 3,
        [Description("This will be returned if the method(GET / POST / PUT / DELETE) used is not supported by the URL e.g.DELETE is not supported on the / config resource")]
        InvalidMethod = 4,
        [Description("Will be returned if required parameters are not present in the message body. The presence of invalid parameters should not trigger this error as long as all required parameters are present.")]
        MissingParemeter = 5,
        [Description("This will be returned if a parameter sent in the message body does not exist. This error is specific to PUT commands; invalid parameters in other commands are simply ignored.")]
        InvalidParemeter = 6,
        [Description("This will be returned if the value set for a parameter is of the incorrect format or is out of range.")]
        InvalidValue = 7,
        [Description("This will be returned if an attempt to modify a read only parameter is made.")]
        ReadOnlyParameter = 8,
        [Description("List in request contains too many items.")]
        ListTooLarge = 11,
        [Description("Command requires portal connection. Returned if portalservices is false or the portal connection is down")]
        PortalConnectionUnavailable = 12,
        [Description("This will be returned if there is an internal error in the processing of the command.This indicates an error in the bridge, not in the message being sent.")]
        InternalError = 91,
        [Description("Link button has not been pressed in last 30 seconds.")]
        LinkButtonNotPressed = 101,
        [Description("DHCP can only be disabled if there is a valid static IP configuration")]
        DisableDhcpFailed = 110,
        [Description("checkforupdate can only be set in updatestate 0 and 1.")]
        InvalidUpdateState = 111,
        [Description("This will be returned if a user attempts to modify a parameter which cannot be modified due to current state of the device. This will most commonly be returned if the hue/sat/bri/effect/xy/ct parameters are modified while the on parameter is false.")]
        DeviceUnavailable = 201,
        [Description("The bridge can store a maximum of 16 groups. This error will be returned if there are already the maximum number of groups created in the bridge.")]
        GroupTableFull = 301,
        [Description("The lamp can store a maximum of 16 groups. This error will be returned if the device cannot accept any new groups in its internal table.")]
        GroupTableFull302 = 302,
        [Description("This will be returned if an attempt to update a light list in a group or delete a group of type \"Luminaire\" or \"LightSource\"")]
        DeviceUnreachable = 304,
        [Description("This will be returned if an attempt to update a light list in a group or delete a group of type \"Luminaire\" or \"LightSource\"")]
        DeleteUnavailable = 305,
        [Description("A light can only be used in 1 room at the same time.")]
        LightAlreadyInRoom = 306,
        [Description("This will be returned if a scene is activated which is currently still in the process of being created.")]
        SceneCreationFailed = 401,
        [Description("It is not possibly anymore to buffer scenes in the bridge for the lights. Application can try again later, let the user turn on lights, remove schedules or delete scenes")]
        SceneBufferFull = 402,
        [Description("Scene could not be removed, because it's locked. Delete the resource (schedule or rule action) that is locking it first. ")]
        SceneLocked = 403,
        [Description("Will be returned if the sensor type cannot be created using CLIP")]
        SensorCreationUnavailable = 501,
        [Description("This will be returned if there are already the maximum number of sensors created in the bridge.")]
        SensorListFull = 502,
        [Description("Returned when already 100 rules are created and no further rules can be added")]
        RuleEngineFull = 601,
        [Description("Rule conditions contain errors or operator combination is not allowed (e.g. only one dt operator is allowed)")]
        ConditionError = 607,
        [Description("Rule actions contain errors or multiple actions with the same resource address")]
        ActionError = 608,
        [Description("Unable to set rule status to ‘enable, because rule conditions references unknown resource or unsupported resource attribute")]
        UnsupportedResourceAttribute = 609,
        [Description("This will be returned if there are already the maximum number of schedules created in the bridge.")]
        ScheduleFull = 701,
        [Description("Cannot set parameter 'localtime', because timezone has not been configured.")]
        InvalidScheduleTimezone = 702,
        [Description("Cannot set parameter 'time' and 'localtime' at the same time.")]
        NotPermittedTimeSetRequest = 703,
        [Description("Cannot create schedule because tag, <tag>, is invalid.")]
        InvalidScheduleCreation = 704,
        [Description("The schedule has expired , the time pattern has to be updated before enabling")]
        InvalidDateTime = 705,
        [Description("Schedule command on a unsupported resource.")]
        UnsupportedResource = 706,
        [Description("Backup is requested on an unsupported bridge model.")]
        UnsupportedBridgeModel = 801,
        [Description("Backup is requested on a factory new bridge, nothing to backup.")]
        NothingToBackup = 802,
        [Description("Backup is requested in another state then idle.")]
        InvalidStateForBackup = 803
    }
}
