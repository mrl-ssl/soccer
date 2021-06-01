#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
namespace MRL.SSL.Common.SSLWrapperCommunication
{
    public class RobotCommands
    {
        public IDictionary<int, SingleWirelessCommand> Commands { get; set; }
        public RobotCommands()
        {
            Commands = new Dictionary<int, SingleWirelessCommand>();
        }
        public void AddCommand(int robotId, SingleWirelessCommand swc)
        {
            if (!Commands.ContainsKey(robotId))
                Commands.Add(robotId, swc);
            else
                Commands[robotId] = swc;
        }

        public RobotControl ToGrSim()
        {
            RobotControl robotControl = new RobotControl();
            foreach (var item in Commands)
            {
                RobotCommand robotCommand = new RobotCommand();
                robotCommand.Id = ((uint)item.Key);
                robotCommand.KickAngle = item.Value.KickAngle;
                robotCommand.KickSpeed = item.Value.KickSpeed;
                robotCommand.DribblerSpeed = item.Value.SpinSpeed;
                robotCommand.MoveCommand = new();
                robotCommand.MoveCommand.LocalVelocity = new MoveLocalVelocity();
                robotCommand.MoveCommand.LocalVelocity.Forward = item.Value.Vx;
                robotCommand.MoveCommand.LocalVelocity.Left = item.Value.Vy;
                robotCommand.MoveCommand.LocalVelocity.Angular = item.Value.W;
                robotControl.RobotCommands.Add(robotCommand);
            }
            return robotControl;
        }
    }


    [global::ProtoBuf.ProtoContract()]
    public partial class RobotCommand : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"id", IsRequired = true)]
        public uint Id { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"move_command")]
        public RobotMoveCommand MoveCommand { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"kick_speed")]
        public float KickSpeed
        {
            get => __pbn__KickSpeed.GetValueOrDefault();
            set => __pbn__KickSpeed = value;
        }
        public bool ShouldSerializeKickSpeed() => __pbn__KickSpeed != null;
        public void ResetKickSpeed() => __pbn__KickSpeed = null;
        private float? __pbn__KickSpeed;

        [global::ProtoBuf.ProtoMember(4, Name = @"kick_angle")]
        [global::System.ComponentModel.DefaultValue(0)]
        public float KickAngle
        {
            get => __pbn__KickAngle ?? 0;
            set => __pbn__KickAngle = value;
        }
        public bool ShouldSerializeKickAngle() => __pbn__KickAngle != null;
        public void ResetKickAngle() => __pbn__KickAngle = null;
        private float? __pbn__KickAngle;

        [global::ProtoBuf.ProtoMember(5, Name = @"dribbler_speed")]
        public float DribblerSpeed
        {
            get => __pbn__DribblerSpeed.GetValueOrDefault();
            set => __pbn__DribblerSpeed = value;
        }
        public bool ShouldSerializeDribblerSpeed() => __pbn__DribblerSpeed != null;
        public void ResetDribblerSpeed() => __pbn__DribblerSpeed = null;
        private float? __pbn__DribblerSpeed;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class RobotMoveCommand : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"wheel_velocity")]
        public MoveWheelVelocity WheelVelocity
        {
            get => __pbn__command.Is(1) ? ((MoveWheelVelocity)__pbn__command.Object) : default;
            set => __pbn__command = new global::ProtoBuf.DiscriminatedUnionObject(1, value);
        }
        public bool ShouldSerializeWheelVelocity() => __pbn__command.Is(1);
        public void ResetWheelVelocity() => global::ProtoBuf.DiscriminatedUnionObject.Reset(ref __pbn__command, 1);

        private global::ProtoBuf.DiscriminatedUnionObject __pbn__command;

        [global::ProtoBuf.ProtoMember(2, Name = @"local_velocity")]
        public MoveLocalVelocity LocalVelocity
        {
            get => __pbn__command.Is(2) ? ((MoveLocalVelocity)__pbn__command.Object) : default;
            set => __pbn__command = new global::ProtoBuf.DiscriminatedUnionObject(2, value);
        }
        public bool ShouldSerializeLocalVelocity() => __pbn__command.Is(2);
        public void ResetLocalVelocity() => global::ProtoBuf.DiscriminatedUnionObject.Reset(ref __pbn__command, 2);

        [global::ProtoBuf.ProtoMember(3, Name = @"global_velocity")]
        public MoveGlobalVelocity GlobalVelocity
        {
            get => __pbn__command.Is(3) ? ((MoveGlobalVelocity)__pbn__command.Object) : default;
            set => __pbn__command = new global::ProtoBuf.DiscriminatedUnionObject(3, value);
        }
        public bool ShouldSerializeGlobalVelocity() => __pbn__command.Is(3);
        public void ResetGlobalVelocity() => global::ProtoBuf.DiscriminatedUnionObject.Reset(ref __pbn__command, 3);

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MoveWheelVelocity : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"front_right", IsRequired = true)]
        public float FrontRight { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"back_right", IsRequired = true)]
        public float BackRight { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"back_left", IsRequired = true)]
        public float BackLeft { get; set; }

        [global::ProtoBuf.ProtoMember(4, Name = @"front_left", IsRequired = true)]
        public float FrontLeft { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MoveLocalVelocity : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"forward", IsRequired = true)]
        public float Forward { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"left", IsRequired = true)]
        public float Left { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"angular", IsRequired = true)]
        public float Angular { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MoveGlobalVelocity : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"x", IsRequired = true)]
        public float X { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"y", IsRequired = true)]
        public float Y { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"angular", IsRequired = true)]
        public float Angular { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class RobotControl : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"robot_commands")]
        public global::System.Collections.Generic.List<RobotCommand> RobotCommands { get; } = new global::System.Collections.Generic.List<RobotCommand>();

    }
}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion