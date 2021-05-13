// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: my.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192


namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [global::ProtoBuf.ProtoContract(Name = @"SSL_Referee_Game_Event")]
    public partial class SSLRefereeGameEvent : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public GameEventType gameEventType { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public Originator originator { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"message")]
        [global::System.ComponentModel.DefaultValue("")]
        public string Message
        {
            get => __pbn__Message ?? "";
            set => __pbn__Message = value;
        }
        public bool ShouldSerializeMessage() => __pbn__Message != null;
        public void ResetMessage() => __pbn__Message = null;
        private string __pbn__Message;

        [global::ProtoBuf.ProtoContract()]
        public partial class Originator : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, Name = @"team", IsRequired = true)]
            public SSLRefereeGameEvent.Team Team { get; set; }

            [global::ProtoBuf.ProtoMember(2)]
            public uint botId
            {
                get => __pbn__botId.GetValueOrDefault();
                set => __pbn__botId = value;
            }
            public bool ShouldSerializebotId() => __pbn__botId != null;
            public void ResetbotId() => __pbn__botId = null;
            private uint? __pbn__botId;

        }

        [global::ProtoBuf.ProtoContract()]
        public enum GameEventType
        {
            [global::ProtoBuf.ProtoEnum(Name = @"UNKNOWN")]
            Unknown = 0,
            [global::ProtoBuf.ProtoEnum(Name = @"CUSTOM")]
            Custom = 1,
            [global::ProtoBuf.ProtoEnum(Name = @"NUMBER_OF_PLAYERS")]
            NumberOfPlayers = 2,
            [global::ProtoBuf.ProtoEnum(Name = @"BALL_LEFT_FIELD")]
            BallLeftField = 3,
            [global::ProtoBuf.ProtoEnum(Name = @"GOAL")]
            Goal = 4,
            [global::ProtoBuf.ProtoEnum(Name = @"KICK_TIMEOUT")]
            KickTimeout = 5,
            [global::ProtoBuf.ProtoEnum(Name = @"NO_PROGRESS_IN_GAME")]
            NoProgressInGame = 6,
            [global::ProtoBuf.ProtoEnum(Name = @"BOT_COLLISION")]
            BotCollision = 7,
            [global::ProtoBuf.ProtoEnum(Name = @"MULTIPLE_DEFENDER")]
            MultipleDefender = 8,
            [global::ProtoBuf.ProtoEnum(Name = @"MULTIPLE_DEFENDER_PARTIALLY")]
            MultipleDefenderPartially = 9,
            [global::ProtoBuf.ProtoEnum(Name = @"ATTACKER_IN_DEFENSE_AREA")]
            AttackerInDefenseArea = 10,
            [global::ProtoBuf.ProtoEnum(Name = @"ICING")]
            Icing = 11,
            [global::ProtoBuf.ProtoEnum(Name = @"BALL_SPEED")]
            BallSpeed = 12,
            [global::ProtoBuf.ProtoEnum(Name = @"ROBOT_STOP_SPEED")]
            RobotStopSpeed = 13,
            [global::ProtoBuf.ProtoEnum(Name = @"BALL_DRIBBLING")]
            BallDribbling = 14,
            [global::ProtoBuf.ProtoEnum(Name = @"ATTACKER_TOUCH_KEEPER")]
            AttackerTouchKeeper = 15,
            [global::ProtoBuf.ProtoEnum(Name = @"DOUBLE_TOUCH")]
            DoubleTouch = 16,
            [global::ProtoBuf.ProtoEnum(Name = @"ATTACKER_TO_DEFENCE_AREA")]
            AttackerToDefenceArea = 17,
            [global::ProtoBuf.ProtoEnum(Name = @"DEFENDER_TO_KICK_POINT_DISTANCE")]
            DefenderToKickPointDistance = 18,
            [global::ProtoBuf.ProtoEnum(Name = @"BALL_HOLDING")]
            BallHolding = 19,
            [global::ProtoBuf.ProtoEnum(Name = @"INDIRECT_GOAL")]
            IndirectGoal = 20,
            [global::ProtoBuf.ProtoEnum(Name = @"BALL_PLACEMENT_FAILED")]
            BallPlacementFailed = 21,
            [global::ProtoBuf.ProtoEnum(Name = @"CHIP_ON_GOAL")]
            ChipOnGoal = 22,
        }

        [global::ProtoBuf.ProtoContract()]
        public enum Team
        {
            [global::ProtoBuf.ProtoEnum(Name = @"TEAM_UNKNOWN")]
            TeamUnknown = 0,
            [global::ProtoBuf.ProtoEnum(Name = @"TEAM_YELLOW")]
            TeamYellow = 1,
            [global::ProtoBuf.ProtoEnum(Name = @"TEAM_BLUE")]
            TeamBlue = 2,
        }

    }

    [global::ProtoBuf.ProtoContract(Name = @"SSL_Referee")]
    public partial class SSLReferee : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"packet_timestamp", IsRequired = true)]
        public ulong PacketTimestamp { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public Stage stage { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"stage_time_left", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
        public int StageTimeLeft
        {
            get => __pbn__StageTimeLeft.GetValueOrDefault();
            set => __pbn__StageTimeLeft = value;
        }
        public bool ShouldSerializeStageTimeLeft() => __pbn__StageTimeLeft != null;
        public void ResetStageTimeLeft() => __pbn__StageTimeLeft = null;
        private int? __pbn__StageTimeLeft;

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public Command command { get; set; }

        [global::ProtoBuf.ProtoMember(5, Name = @"command_counter", IsRequired = true)]
        public uint CommandCounter { get; set; }

        [global::ProtoBuf.ProtoMember(6, Name = @"command_timestamp", IsRequired = true)]
        public ulong CommandTimestamp { get; set; }

        [global::ProtoBuf.ProtoMember(7, Name = @"yellow", IsRequired = true)]
        public TeamInfo Yellow { get; set; }

        [global::ProtoBuf.ProtoMember(8, Name = @"blue", IsRequired = true)]
        public TeamInfo Blue { get; set; }

        [global::ProtoBuf.ProtoMember(9, Name = @"designated_position")]
        public Point DesignatedPosition { get; set; }

        [global::ProtoBuf.ProtoMember(10)]
        public bool blueTeamOnPositiveHalf
        {
            get => __pbn__blueTeamOnPositiveHalf.GetValueOrDefault();
            set => __pbn__blueTeamOnPositiveHalf = value;
        }
        public bool ShouldSerializeblueTeamOnPositiveHalf() => __pbn__blueTeamOnPositiveHalf != null;
        public void ResetblueTeamOnPositiveHalf() => __pbn__blueTeamOnPositiveHalf = null;
        private bool? __pbn__blueTeamOnPositiveHalf;

        [global::ProtoBuf.ProtoMember(11)]
        public SSLRefereeGameEvent gameEvent { get; set; }

        [global::ProtoBuf.ProtoContract()]
        public partial class TeamInfo : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, Name = @"name", IsRequired = true)]
            public string Name { get; set; }

            [global::ProtoBuf.ProtoMember(2, Name = @"score", IsRequired = true)]
            public uint Score { get; set; }

            [global::ProtoBuf.ProtoMember(3, Name = @"red_cards", IsRequired = true)]
            public uint RedCards { get; set; }

            [global::ProtoBuf.ProtoMember(4, Name = @"yellow_card_times", IsPacked = true)]
            public uint[] YellowCardTimes { get; set; }

            [global::ProtoBuf.ProtoMember(5, Name = @"yellow_cards", IsRequired = true)]
            public uint YellowCards { get; set; }

            [global::ProtoBuf.ProtoMember(6, Name = @"timeouts", IsRequired = true)]
            public uint Timeouts { get; set; }

            [global::ProtoBuf.ProtoMember(7, Name = @"timeout_time", IsRequired = true)]
            public uint TimeoutTime { get; set; }

            [global::ProtoBuf.ProtoMember(8, Name = @"goalie", IsRequired = true)]
            public uint Goalie { get; set; }

        }

        [global::ProtoBuf.ProtoContract()]
        public partial class Point : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, Name = @"x", IsRequired = true)]
            public float X { get; set; }

            [global::ProtoBuf.ProtoMember(2, Name = @"y", IsRequired = true)]
            public float Y { get; set; }

        }

        [global::ProtoBuf.ProtoContract()]
        public enum Stage
        {
            [global::ProtoBuf.ProtoEnum(Name = @"NORMAL_FIRST_HALF_PRE")]
            NormalFirstHalfPre = 0,
            [global::ProtoBuf.ProtoEnum(Name = @"NORMAL_FIRST_HALF")]
            NormalFirstHalf = 1,
            [global::ProtoBuf.ProtoEnum(Name = @"NORMAL_HALF_TIME")]
            NormalHalfTime = 2,
            [global::ProtoBuf.ProtoEnum(Name = @"NORMAL_SECOND_HALF_PRE")]
            NormalSecondHalfPre = 3,
            [global::ProtoBuf.ProtoEnum(Name = @"NORMAL_SECOND_HALF")]
            NormalSecondHalf = 4,
            [global::ProtoBuf.ProtoEnum(Name = @"EXTRA_TIME_BREAK")]
            ExtraTimeBreak = 5,
            [global::ProtoBuf.ProtoEnum(Name = @"EXTRA_FIRST_HALF_PRE")]
            ExtraFirstHalfPre = 6,
            [global::ProtoBuf.ProtoEnum(Name = @"EXTRA_FIRST_HALF")]
            ExtraFirstHalf = 7,
            [global::ProtoBuf.ProtoEnum(Name = @"EXTRA_HALF_TIME")]
            ExtraHalfTime = 8,
            [global::ProtoBuf.ProtoEnum(Name = @"EXTRA_SECOND_HALF_PRE")]
            ExtraSecondHalfPre = 9,
            [global::ProtoBuf.ProtoEnum(Name = @"EXTRA_SECOND_HALF")]
            ExtraSecondHalf = 10,
            [global::ProtoBuf.ProtoEnum(Name = @"PENALTY_SHOOTOUT_BREAK")]
            PenaltyShootoutBreak = 11,
            [global::ProtoBuf.ProtoEnum(Name = @"PENALTY_SHOOTOUT")]
            PenaltyShootout = 12,
            [global::ProtoBuf.ProtoEnum(Name = @"POST_GAME")]
            PostGame = 13,
        }

        [global::ProtoBuf.ProtoContract()]
        public enum Command
        {
            [global::ProtoBuf.ProtoEnum(Name = @"HALT")]
            Halt = 0,
            [global::ProtoBuf.ProtoEnum(Name = @"STOP")]
            Stop = 1,
            [global::ProtoBuf.ProtoEnum(Name = @"NORMAL_START")]
            NormalStart = 2,
            [global::ProtoBuf.ProtoEnum(Name = @"FORCE_START")]
            ForceStart = 3,
            [global::ProtoBuf.ProtoEnum(Name = @"PREPARE_KICKOFF_YELLOW")]
            PrepareKickoffYellow = 4,
            [global::ProtoBuf.ProtoEnum(Name = @"PREPARE_KICKOFF_BLUE")]
            PrepareKickoffBlue = 5,
            [global::ProtoBuf.ProtoEnum(Name = @"PREPARE_PENALTY_YELLOW")]
            PreparePenaltyYellow = 6,
            [global::ProtoBuf.ProtoEnum(Name = @"PREPARE_PENALTY_BLUE")]
            PreparePenaltyBlue = 7,
            [global::ProtoBuf.ProtoEnum(Name = @"DIRECT_FREE_YELLOW")]
            DirectFreeYellow = 8,
            [global::ProtoBuf.ProtoEnum(Name = @"DIRECT_FREE_BLUE")]
            DirectFreeBlue = 9,
            [global::ProtoBuf.ProtoEnum(Name = @"INDIRECT_FREE_YELLOW")]
            IndirectFreeYellow = 10,
            [global::ProtoBuf.ProtoEnum(Name = @"INDIRECT_FREE_BLUE")]
            IndirectFreeBlue = 11,
            [global::ProtoBuf.ProtoEnum(Name = @"TIMEOUT_YELLOW")]
            TimeoutYellow = 12,
            [global::ProtoBuf.ProtoEnum(Name = @"TIMEOUT_BLUE")]
            TimeoutBlue = 13,
            [global::ProtoBuf.ProtoEnum(Name = @"GOAL_YELLOW")]
            GoalYellow = 14,
            [global::ProtoBuf.ProtoEnum(Name = @"GOAL_BLUE")]
            GoalBlue = 15,
            [global::ProtoBuf.ProtoEnum(Name = @"BALL_PLACEMENT_YELLOW")]
            BallPlacementYellow = 16,
            [global::ProtoBuf.ProtoEnum(Name = @"BALL_PLACEMENT_BLUE")]
            BallPlacementBlue = 17,
        }

    }
}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
