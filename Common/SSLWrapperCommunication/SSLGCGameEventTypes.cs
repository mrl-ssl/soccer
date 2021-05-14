#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192

using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{

    [ProtoContract()]
    public enum GameEventType
    {
        [ProtoEnum(Name = @"UNKNOWN_GAME_EVENT_TYPE")]
        UnknownGameEventType = 0,
        [ProtoEnum(Name = @"BALL_LEFT_FIELD_TOUCH_LINE")]
        BallLeftFieldTouchLine = 6,
        [ProtoEnum(Name = @"BALL_LEFT_FIELD_GOAL_LINE")]
        BallLeftFieldGoalLine = 7,
        [ProtoEnum(Name = @"AIMLESS_KICK")]
        AimlessKick = 11,
        [ProtoEnum(Name = @"ATTACKER_TOO_CLOSE_TO_DEFENSE_AREA")]
        AttackerTooCloseToDefenseArea = 19,
        [ProtoEnum(Name = @"DEFENDER_IN_DEFENSE_AREA")]
        DefenderInDefenseArea = 31,
        [ProtoEnum(Name = @"BOUNDARY_CROSSING")]
        BoundaryCrossing = 41,
        [ProtoEnum(Name = @"KEEPER_HELD_BALL")]
        KeeperHeldBall = 13,
        [ProtoEnum(Name = @"BOT_DRIBBLED_BALL_TOO_FAR")]
        BotDribbledBallTooFar = 17,
        [ProtoEnum(Name = @"BOT_PUSHED_BOT")]
        BotPushedBot = 24,
        [ProtoEnum(Name = @"BOT_HELD_BALL_DELIBERATELY")]
        BotHeldBallDeliberately = 26,
        [ProtoEnum(Name = @"BOT_TIPPED_OVER")]
        BotTippedOver = 27,
        [ProtoEnum(Name = @"ATTACKER_TOUCHED_BALL_IN_DEFENSE_AREA")]
        AttackerTouchedBallInDefenseArea = 15,
        [ProtoEnum(Name = @"BOT_KICKED_BALL_TOO_FAST")]
        BotKickedBallTooFast = 18,
        [ProtoEnum(Name = @"BOT_CRASH_UNIQUE")]
        BotCrashUnique = 22,
        [ProtoEnum(Name = @"BOT_CRASH_DRAWN")]
        BotCrashDrawn = 21,
        [ProtoEnum(Name = @"DEFENDER_TOO_CLOSE_TO_KICK_POINT")]
        DefenderTooCloseToKickPoint = 29,
        [ProtoEnum(Name = @"BOT_TOO_FAST_IN_STOP")]
        BotTooFastInStop = 28,
        [ProtoEnum(Name = @"BOT_INTERFERED_PLACEMENT")]
        BotInterferedPlacement = 20,
        [ProtoEnum(Name = @"POSSIBLE_GOAL")]
        PossibleGoal = 39,
        [ProtoEnum(Name = @"GOAL")]
        Goal = 8,
        [ProtoEnum(Name = @"INVALID_GOAL")]
        InvalidGoal = 42,
        [ProtoEnum(Name = @"ATTACKER_DOUBLE_TOUCHED_BALL")]
        AttackerDoubleTouchedBall = 14,
        [ProtoEnum(Name = @"PLACEMENT_SUCCEEDED")]
        PlacementSucceeded = 5,
        [ProtoEnum(Name = @"PENALTY_KICK_FAILED")]
        PenaltyKickFailed = 43,
        [ProtoEnum(Name = @"NO_PROGRESS_IN_GAME")]
        NoProgressInGame = 2,
        [ProtoEnum(Name = @"PLACEMENT_FAILED")]
        PlacementFailed = 3,
        [ProtoEnum(Name = @"MULTIPLE_CARDS")]
        MultipleCards = 32,
        [ProtoEnum(Name = @"MULTIPLE_FOULS")]
        MultipleFouls = 34,
        [ProtoEnum(Name = @"BOT_SUBSTITUTION")]
        BotSubstitution = 37,
        [ProtoEnum(Name = @"TOO_MANY_ROBOTS")]
        TooManyRobots = 38,
        [ProtoEnum(Name = @"CHALLENGE_FLAG")]
        ChallengeFlag = 44,
        [ProtoEnum(Name = @"EMERGENCY_STOP")]
        EmergencyStop = 45,
        [ProtoEnum(Name = @"UNSPORTING_BEHAVIOR_MINOR")]
        UnsportingBehaviorMinor = 35,
        [ProtoEnum(Name = @"UNSPORTING_BEHAVIOR_MAJOR")]
        UnsportingBehaviorMajor = 36,
        [ProtoEnum(Name = @"PREPARED")]
        [global::System.Obsolete]
        Prepared = 1,
        [ProtoEnum(Name = @"INDIRECT_GOAL")]
        [global::System.Obsolete]
        IndirectGoal = 9,
        [ProtoEnum(Name = @"CHIPPED_GOAL")]
        [global::System.Obsolete]
        ChippedGoal = 10,
        [ProtoEnum(Name = @"KICK_TIMEOUT")]
        [global::System.Obsolete]
        KickTimeout = 12,
        [ProtoEnum(Name = @"ATTACKER_TOUCHED_OPPONENT_IN_DEFENSE_AREA")]
        [global::System.Obsolete]
        AttackerTouchedOpponentInDefenseArea = 16,
        [ProtoEnum(Name = @"ATTACKER_TOUCHED_OPPONENT_IN_DEFENSE_AREA_SKIPPED")]
        [global::System.Obsolete]
        AttackerTouchedOpponentInDefenseAreaSkipped = 40,
        [ProtoEnum(Name = @"BOT_CRASH_UNIQUE_SKIPPED")]
        [global::System.Obsolete]
        BotCrashUniqueSkipped = 23,
        [ProtoEnum(Name = @"BOT_PUSHED_BOT_SKIPPED")]
        [global::System.Obsolete]
        BotPushedBotSkipped = 25,
        [ProtoEnum(Name = @"DEFENDER_IN_DEFENSE_AREA_PARTIALLY")]
        [global::System.Obsolete]
        DefenderInDefenseAreaPartially = 30,
        [ProtoEnum(Name = @"MULTIPLE_PLACEMENT_FAILURES")]
        [global::System.Obsolete]
        MultiplePlacementFailures = 33,
    }

    [ProtoContract()]
    public partial class BallLeftFieldType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

    }

    [ProtoContract()]
    public partial class AimlessKickType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"kick_location")]
        public VectorF2D KickLocation { get; set; }

    }

    [ProtoContract()]
    public partial class GoalType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(6, Name = @"kicking_team")]
        [global::System.ComponentModel.DefaultValue(Team.Unknown)]
        public Team KickingTeam
        {
            get => __pbn__KickingTeam ?? Team.Unknown;
            set => __pbn__KickingTeam = value;
        }
        public bool ShouldSerializeKickingTeam() => __pbn__KickingTeam != null;
        public void ResetKickingTeam() => __pbn__KickingTeam = null;
        private Team? __pbn__KickingTeam;

        [ProtoMember(2, Name = @"kicking_bot")]
        public uint? KickingBot
        {
            get => __pbn__KickingBot;
            set => __pbn__KickingBot = value;
        }
        public bool ShouldSerializeKickingBot() => __pbn__KickingBot != null;
        public void ResetKickingBot() => __pbn__KickingBot = null;
        private uint? __pbn__KickingBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"kick_location")]
        public VectorF2D KickLocation { get; set; }

        [ProtoMember(5, Name = @"max_ball_height")]
        public float? MaxBallHeight
        {
            get => __pbn__MaxBallHeight;
            set => __pbn__MaxBallHeight = value;
        }
        public bool ShouldSerializeMaxBallHeight() => __pbn__MaxBallHeight != null;
        public void ResetMaxBallHeight() => __pbn__MaxBallHeight = null;
        private float? __pbn__MaxBallHeight;

        [ProtoMember(7, Name = @"num_robots_by_team")]
        public uint? NumRobotsByTeam
        {
            get => __pbn__NumRobotsByTeam;
            set => __pbn__NumRobotsByTeam = value;
        }
        public bool ShouldSerializeNumRobotsByTeam() => __pbn__NumRobotsByTeam != null;
        public void ResetNumRobotsByTeam() => __pbn__NumRobotsByTeam = null;
        private uint? __pbn__NumRobotsByTeam;

        [ProtoMember(8, Name = @"last_touch_by_team")]
        public ulong? LastTouchByTeam
        {
            get => __pbn__LastTouchByTeam;
            set => __pbn__LastTouchByTeam = value;
        }
        public bool ShouldSerializeLastTouchByTeam() => __pbn__LastTouchByTeam != null;
        public void ResetLastTouchByTeam() => __pbn__LastTouchByTeam = null;
        private ulong? __pbn__LastTouchByTeam;

        [ProtoMember(9, Name = @"message")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Message
        {
            get => __pbn__Message ?? "";
            set => __pbn__Message = value;
        }
        public bool ShouldSerializeMessage() => __pbn__Message != null;
        public void ResetMessage() => __pbn__Message = null;
        private string __pbn__Message;

    }

    [ProtoContract()]
    public partial class IndirectGoalType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"kick_location")]
        public VectorF2D KickLocation { get; set; }

    }

    [ProtoContract()]
    public partial class ChippedGoalType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"kick_location")]
        public VectorF2D KickLocation { get; set; }

        [ProtoMember(5, Name = @"max_ball_height")]
        public float? MaxBallHeight
        {
            get => __pbn__MaxBallHeight;
            set => __pbn__MaxBallHeight = value;
        }
        public bool ShouldSerializeMaxBallHeight() => __pbn__MaxBallHeight != null;
        public void ResetMaxBallHeight() => __pbn__MaxBallHeight = null;
        private float? __pbn__MaxBallHeight;

    }

    [ProtoContract()]
    public partial class BotTooFastInStopType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"speed")]
        public float? Speed
        {
            get => __pbn__Speed;
            set => __pbn__Speed = value;
        }
        public bool ShouldSerializeSpeed() => __pbn__Speed != null;
        public void ResetSpeed() => __pbn__Speed = null;
        private float? __pbn__Speed;

    }

    [ProtoContract()]
    public partial class DefenderTooCloseToKickPointType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"distance")]
        public float? Distance
        {
            get => __pbn__Distance;
            set => __pbn__Distance = value;
        }
        public bool ShouldSerializeDistance() => __pbn__Distance != null;
        public void ResetDistance() => __pbn__Distance = null;
        private float? __pbn__Distance;

    }

    [ProtoContract()]
    public partial class BotCrashDrawnType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"bot_yellow")]
        public uint? BotYellow
        {
            get => __pbn__BotYellow;
            set => __pbn__BotYellow = value;
        }
        public bool ShouldSerializeBotYellow() => __pbn__BotYellow != null;
        public void ResetBotYellow() => __pbn__BotYellow = null;
        private uint? __pbn__BotYellow;

        [ProtoMember(2, Name = @"bot_blue")]
        public uint? BotBlue
        {
            get => __pbn__BotBlue;
            set => __pbn__BotBlue = value;
        }
        public bool ShouldSerializeBotBlue() => __pbn__BotBlue != null;
        public void ResetBotBlue() => __pbn__BotBlue = null;
        private uint? __pbn__BotBlue;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"crash_speed")]
        public float? CrashSpeed
        {
            get => __pbn__CrashSpeed;
            set => __pbn__CrashSpeed = value;
        }
        public bool ShouldSerializeCrashSpeed() => __pbn__CrashSpeed != null;
        public void ResetCrashSpeed() => __pbn__CrashSpeed = null;
        private float? __pbn__CrashSpeed;

        [ProtoMember(5, Name = @"speed_diff")]
        public float? SpeedDiff
        {
            get => __pbn__SpeedDiff;
            set => __pbn__SpeedDiff = value;
        }
        public bool ShouldSerializeSpeedDiff() => __pbn__SpeedDiff != null;
        public void ResetSpeedDiff() => __pbn__SpeedDiff = null;
        private float? __pbn__SpeedDiff;

        [ProtoMember(6, Name = @"crash_angle")]
        public float? CrashAngle
        {
            get => __pbn__CrashAngle;
            set => __pbn__CrashAngle = value;
        }
        public bool ShouldSerializeCrashAngle() => __pbn__CrashAngle != null;
        public void ResetCrashAngle() => __pbn__CrashAngle = null;
        private float? __pbn__CrashAngle;

    }

    [ProtoContract()]
    public partial class BotCrashUniqueType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"violator")]
        public uint? Violator
        {
            get => __pbn__Violator;
            set => __pbn__Violator = value;
        }
        public bool ShouldSerializeViolator() => __pbn__Violator != null;
        public void ResetViolator() => __pbn__Violator = null;
        private uint? __pbn__Violator;

        [ProtoMember(3, Name = @"victim")]
        public uint? Victim
        {
            get => __pbn__Victim;
            set => __pbn__Victim = value;
        }
        public bool ShouldSerializeVictim() => __pbn__Victim != null;
        public void ResetVictim() => __pbn__Victim = null;
        private uint? __pbn__Victim;

        [ProtoMember(4, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(5, Name = @"crash_speed")]
        public float? CrashSpeed
        {
            get => __pbn__CrashSpeed;
            set => __pbn__CrashSpeed = value;
        }
        public bool ShouldSerializeCrashSpeed() => __pbn__CrashSpeed != null;
        public void ResetCrashSpeed() => __pbn__CrashSpeed = null;
        private float? __pbn__CrashSpeed;

        [ProtoMember(6, Name = @"speed_diff")]
        public float? SpeedDiff
        {
            get => __pbn__SpeedDiff;
            set => __pbn__SpeedDiff = value;
        }
        public bool ShouldSerializeSpeedDiff() => __pbn__SpeedDiff != null;
        public void ResetSpeedDiff() => __pbn__SpeedDiff = null;
        private float? __pbn__SpeedDiff;

        [ProtoMember(7, Name = @"crash_angle")]
        public float? CrashAngle
        {
            get => __pbn__CrashAngle;
            set => __pbn__CrashAngle = value;
        }
        public bool ShouldSerializeCrashAngle() => __pbn__CrashAngle != null;
        public void ResetCrashAngle() => __pbn__CrashAngle = null;
        private float? __pbn__CrashAngle;

    }

    [ProtoContract()]
    public partial class BotPushedBotType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"violator")]
        public uint? Violator
        {
            get => __pbn__Violator;
            set => __pbn__Violator = value;
        }
        public bool ShouldSerializeViolator() => __pbn__Violator != null;
        public void ResetViolator() => __pbn__Violator = null;
        private uint? __pbn__Violator;

        [ProtoMember(3, Name = @"victim")]
        public uint? Victim
        {
            get => __pbn__Victim;
            set => __pbn__Victim = value;
        }
        public bool ShouldSerializeVictim() => __pbn__Victim != null;
        public void ResetVictim() => __pbn__Victim = null;
        private uint? __pbn__Victim;

        [ProtoMember(4, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(5, Name = @"pushed_distance")]
        public float? PushedDistance
        {
            get => __pbn__PushedDistance;
            set => __pbn__PushedDistance = value;
        }
        public bool ShouldSerializePushedDistance() => __pbn__PushedDistance != null;
        public void ResetPushedDistance() => __pbn__PushedDistance = null;
        private float? __pbn__PushedDistance;

    }

    [ProtoContract()]
    public partial class BotTippedOverType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"ball_location")]
        public VectorF2D BallLocation { get; set; }

    }

    [ProtoContract()]
    public partial class DefenderInDefenseAreaType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"distance")]
        public float? Distance
        {
            get => __pbn__Distance;
            set => __pbn__Distance = value;
        }
        public bool ShouldSerializeDistance() => __pbn__Distance != null;
        public void ResetDistance() => __pbn__Distance = null;
        private float? __pbn__Distance;

    }

    [ProtoContract()]
    public partial class DefenderInDefenseAreaPartiallyType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"distance")]
        public float? Distance
        {
            get => __pbn__Distance;
            set => __pbn__Distance = value;
        }
        public bool ShouldSerializeDistance() => __pbn__Distance != null;
        public void ResetDistance() => __pbn__Distance = null;
        private float? __pbn__Distance;

        [ProtoMember(5, Name = @"ball_location")]
        public VectorF2D BallLocation { get; set; }

    }

    [ProtoContract()]
    public partial class AttackerTouchedBallInDefenseAreaType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"distance")]
        public float? Distance
        {
            get => __pbn__Distance;
            set => __pbn__Distance = value;
        }
        public bool ShouldSerializeDistance() => __pbn__Distance != null;
        public void ResetDistance() => __pbn__Distance = null;
        private float? __pbn__Distance;

    }

    [ProtoContract()]
    public partial class BotKickedBallTooFastType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"initial_ball_speed")]
        public float? InitialBallSpeed
        {
            get => __pbn__InitialBallSpeed;
            set => __pbn__InitialBallSpeed = value;
        }
        public bool ShouldSerializeInitialBallSpeed() => __pbn__InitialBallSpeed != null;
        public void ResetInitialBallSpeed() => __pbn__InitialBallSpeed = null;
        private float? __pbn__InitialBallSpeed;

        [ProtoMember(5, Name = @"chipped")]
        public bool? Chipped
        {
            get => __pbn__Chipped;
            set => __pbn__Chipped = value;
        }
        public bool ShouldSerializeChipped() => __pbn__Chipped != null;
        public void ResetChipped() => __pbn__Chipped = null;
        private bool? __pbn__Chipped;

    }

    [ProtoContract()]
    public partial class BotDribbledBallTooFarType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"start")]
        public VectorF2D Start { get; set; }

        [ProtoMember(4, Name = @"end")]
        public VectorF2D End { get; set; }

    }

    [ProtoContract()]
    public partial class AttackerTouchedOpponentInDefenseAreaType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(4, Name = @"victim")]
        public uint? Victim
        {
            get => __pbn__Victim;
            set => __pbn__Victim = value;
        }
        public bool ShouldSerializeVictim() => __pbn__Victim != null;
        public void ResetVictim() => __pbn__Victim = null;
        private uint? __pbn__Victim;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

    }

    [ProtoContract()]
    public partial class AttackerDoubleTouchedBallType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

    }

    [ProtoContract()]
    public partial class AttackerTooCloseToDefenseAreaType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"distance")]
        public float? Distance
        {
            get => __pbn__Distance;
            set => __pbn__Distance = value;
        }
        public bool ShouldSerializeDistance() => __pbn__Distance != null;
        public void ResetDistance() => __pbn__Distance = null;
        private float? __pbn__Distance;

        [ProtoMember(5, Name = @"ball_location")]
        public VectorF2D BallLocation { get; set; }

    }

    [ProtoContract()]
    public partial class BotHeldBallDeliberatelyType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(4, Name = @"duration")]
        public float? Duration
        {
            get => __pbn__Duration;
            set => __pbn__Duration = value;
        }
        public bool ShouldSerializeDuration() => __pbn__Duration != null;
        public void ResetDuration() => __pbn__Duration = null;
        private float? __pbn__Duration;

    }

    [ProtoContract()]
    public partial class BotInterferedPlacementType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"by_bot")]
        public uint? ByBot
        {
            get => __pbn__ByBot;
            set => __pbn__ByBot = value;
        }
        public bool ShouldSerializeByBot() => __pbn__ByBot != null;
        public void ResetByBot() => __pbn__ByBot = null;
        private uint? __pbn__ByBot;

        [ProtoMember(3, Name = @"location")]
        public VectorF2D Location { get; set; }

    }

    [ProtoContract()]
    public partial class MultipleCardsType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

    }

    [ProtoContract()]
    public partial class MultipleFoulsType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

    }

    [ProtoContract()]
    public partial class MultiplePlacementFailuresType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

    }

    [ProtoContract()]
    public partial class KickTimeoutType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(3, Name = @"time")]
        public float? Time
        {
            get => __pbn__Time;
            set => __pbn__Time = value;
        }
        public bool ShouldSerializeTime() => __pbn__Time != null;
        public void ResetTime() => __pbn__Time = null;
        private float? __pbn__Time;

    }

    [ProtoContract()]
    public partial class NoProgressInGameType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(2, Name = @"time")]
        public float? Time
        {
            get => __pbn__Time;
            set => __pbn__Time = value;
        }
        public bool ShouldSerializeTime() => __pbn__Time != null;
        public void ResetTime() => __pbn__Time = null;
        private float? __pbn__Time;

    }

    [ProtoContract()]
    public partial class PlacementFailedType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"remaining_distance")]
        public float? RemainingDistance
        {
            get => __pbn__RemainingDistance;
            set => __pbn__RemainingDistance = value;
        }
        public bool ShouldSerializeRemainingDistance() => __pbn__RemainingDistance != null;
        public void ResetRemainingDistance() => __pbn__RemainingDistance = null;
        private float? __pbn__RemainingDistance;

    }

    [ProtoContract()]
    public partial class UnsportingBehaviorMinorType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"reason", IsRequired = true)]
        public string Reason { get; set; }

    }

    [ProtoContract()]
    public partial class UnsportingBehaviorMajor : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"reason", IsRequired = true)]
        public string Reason { get; set; }

    }

    [ProtoContract()]
    public partial class KeeperHeldBallType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"location")]
        public VectorF2D Location { get; set; }

        [ProtoMember(3, Name = @"duration")]
        public float? Duration
        {
            get => __pbn__Duration;
            set => __pbn__Duration = value;
        }
        public bool ShouldSerializeDuration() => __pbn__Duration != null;
        public void ResetDuration() => __pbn__Duration = null;
        private float? __pbn__Duration;

    }

    [ProtoContract()]
    public partial class PlacementSucceededType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"time_taken")]
        public float? TimeTaken
        {
            get => __pbn__TimeTaken;
            set => __pbn__TimeTaken = value;
        }
        public bool ShouldSerializeTimeTaken() => __pbn__TimeTaken != null;
        public void ResetTimeTaken() => __pbn__TimeTaken = null;
        private float? __pbn__TimeTaken;

        [ProtoMember(3, Name = @"precision")]
        public float? Precision
        {
            get => __pbn__Precision;
            set => __pbn__Precision = value;
        }
        public bool ShouldSerializePrecision() => __pbn__Precision != null;
        public void ResetPrecision() => __pbn__Precision = null;
        private float? __pbn__Precision;

        [ProtoMember(4, Name = @"distance")]
        public float? Distance
        {
            get => __pbn__Distance;
            set => __pbn__Distance = value;
        }
        public bool ShouldSerializeDistance() => __pbn__Distance != null;
        public void ResetDistance() => __pbn__Distance = null;
        private float? __pbn__Distance;

    }

    [ProtoContract()]
    public partial class PreparedType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"time_taken")]
        public float? TimeTaken
        {
            get => __pbn__TimeTaken;
            set => __pbn__TimeTaken = value;
        }
        public bool ShouldSerializeTimeTaken() => __pbn__TimeTaken != null;
        public void ResetTimeTaken() => __pbn__TimeTaken = null;
        private float? __pbn__TimeTaken;

    }

    [ProtoContract()]
    public partial class BotSubstitutionType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

    }

    [ProtoContract()]
    public partial class ChallengeFlagType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

    }

    [ProtoContract()]
    public partial class EmergencyStopType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

    }

    [ProtoContract()]
    public partial class TooManyRobotsType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"num_robots_allowed")]
        public int? NumRobotsAllowed
        {
            get => __pbn__NumRobotsAllowed;
            set => __pbn__NumRobotsAllowed = value;
        }
        public bool ShouldSerializeNumRobotsAllowed() => __pbn__NumRobotsAllowed != null;
        public void ResetNumRobotsAllowed() => __pbn__NumRobotsAllowed = null;
        private int? __pbn__NumRobotsAllowed;

        [ProtoMember(3, Name = @"num_robots_on_field")]
        public int? NumRobotsOnField
        {
            get => __pbn__NumRobotsOnField;
            set => __pbn__NumRobotsOnField = value;
        }
        public bool ShouldSerializeNumRobotsOnField() => __pbn__NumRobotsOnField != null;
        public void ResetNumRobotsOnField() => __pbn__NumRobotsOnField = null;
        private int? __pbn__NumRobotsOnField;

        [ProtoMember(4, Name = @"ball_location")]
        public VectorF2D BallLocation { get; set; }

    }

    [ProtoContract()]
    public partial class BoundaryCrossingType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"location")]
        public VectorF2D Location { get; set; }

    }

    [ProtoContract()]
    public partial class PenaltyKickFailedType : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"by_team", IsRequired = true)]
        public Team ByTeam { get; set; }

        [ProtoMember(2, Name = @"location")]
        public VectorF2D Location { get; set; }

    }
}
#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
