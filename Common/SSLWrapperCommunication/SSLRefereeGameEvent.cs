#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192

using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{

    [ProtoContract()]
    public partial class GameEvent : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(40)]
        [DefaultValue(GameEventType.UnknownGameEventType)]
        public GameEventType EventType
        {
            get => __pbn__type ?? GameEventType.UnknownGameEventType;
            set => __pbn__type = value;
        }
        public bool ShouldSerializeEventType() => __pbn__type != null;
        public void ResetEventType() => __pbn__type = null;
        private GameEventType? __pbn__type;

        [ProtoMember(41, Name = @"origin")]
        public List<string> Origins { get; } = new List<string>();

        [ProtoMember(6, Name = @"ball_left_field_touch_line")]
        public BallLeftFieldType BallLeftFieldTouchLine
        {
            get => __pbn__event.Is(6) ? ((BallLeftFieldType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(6, value);
        }
        public bool ShouldSerializeBallLeftFieldTouchLine() => __pbn__event.Is(6);
        public void ResetBallLeftFieldTouchLine() => DiscriminatedUnionObject.Reset(ref __pbn__event, 6);

        private DiscriminatedUnionObject __pbn__event;

        [ProtoMember(7, Name = @"ball_left_field_goal_line")]
        public BallLeftFieldType BallLeftFieldGoalLine
        {
            get => __pbn__event.Is(7) ? ((BallLeftFieldType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(7, value);
        }
        public bool ShouldSerializeBallLeftFieldGoalLine() => __pbn__event.Is(7);
        public void ResetBallLeftFieldGoalLine() => DiscriminatedUnionObject.Reset(ref __pbn__event, 7);

        [ProtoMember(11)]
        public AimlessKickType AimlessKick
        {
            get => __pbn__event.Is(11) ? ((AimlessKickType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(11, value);
        }
        public bool ShouldSerializeAimlessKick() => __pbn__event.Is(11);
        public void ResetAimlessKick() => DiscriminatedUnionObject.Reset(ref __pbn__event, 11);

        [ProtoMember(19)]
        public AttackerTooCloseToDefenseAreaType AttackerTooCloseToDefenseArea
        {
            get => __pbn__event.Is(19) ? ((AttackerTooCloseToDefenseAreaType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(19, value);
        }
        public bool ShouldSerializeAttackerTooCloseToDefenseArea() => __pbn__event.Is(19);
        public void ResetAttackerTooCloseToDefenseArea() => DiscriminatedUnionObject.Reset(ref __pbn__event, 19);

        [ProtoMember(31)]
        public DefenderInDefenseAreaType DefenderInDefenseArea
        {
            get => __pbn__event.Is(31) ? ((DefenderInDefenseAreaType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(31, value);
        }
        public bool ShouldSerializeDefenderInDefenseArea() => __pbn__event.Is(31);
        public void ResetDefenderInDefenseArea() => DiscriminatedUnionObject.Reset(ref __pbn__event, 31);

        [ProtoMember(43)]
        public BoundaryCrossingType BoundaryCrossing
        {
            get => __pbn__event.Is(43) ? ((BoundaryCrossingType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(43, value);
        }
        public bool ShouldSerializeBoundaryCrossing() => __pbn__event.Is(43);
        public void ResetBoundaryCrossing() => DiscriminatedUnionObject.Reset(ref __pbn__event, 43);

        [ProtoMember(13)]
        public KeeperHeldBallType KeeperHeldBall
        {
            get => __pbn__event.Is(13) ? ((KeeperHeldBallType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(13, value);
        }
        public bool ShouldSerializeKeeperHeldBall() => __pbn__event.Is(13);
        public void ResetKeeperHeldBall() => DiscriminatedUnionObject.Reset(ref __pbn__event, 13);

        [ProtoMember(17)]
        public BotDribbledBallTooFarType BotDribbledBallTooFarType
        {
            get => __pbn__event.Is(17) ? ((BotDribbledBallTooFarType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(17, value);
        }
        public bool ShouldSerializeBotDribbledBallTooFarType() => __pbn__event.Is(17);
        public void ResetBotDribbledBallTooFarType() => DiscriminatedUnionObject.Reset(ref __pbn__event, 17);

        [ProtoMember(24)]
        public BotPushedBotType BotPushedBot
        {
            get => __pbn__event.Is(24) ? ((BotPushedBotType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(24, value);
        }
        public bool ShouldSerializeBotPushedBot() => __pbn__event.Is(24);
        public void ResetBotPushedBot() => DiscriminatedUnionObject.Reset(ref __pbn__event, 24);

        [ProtoMember(26)]
        public BotHeldBallDeliberatelyType BotHeldBallDeliberately
        {
            get => __pbn__event.Is(26) ? ((BotHeldBallDeliberatelyType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(26, value);
        }
        public bool ShouldSerializeBotHeldBallDeliberately() => __pbn__event.Is(26);
        public void ResetBotHeldBallDeliberately() => DiscriminatedUnionObject.Reset(ref __pbn__event, 26);

        [ProtoMember(27)]
        public BotTippedOverType BotTippedOver
        {
            get => __pbn__event.Is(27) ? ((BotTippedOverType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(27, value);
        }
        public bool ShouldSerializeBotTippedOver() => __pbn__event.Is(27);
        public void ResetBotTippedOver() => DiscriminatedUnionObject.Reset(ref __pbn__event, 27);

        [ProtoMember(15)]
        public AttackerTouchedBallInDefenseAreaType AttackerTouchedBallInDefenseArea
        {
            get => __pbn__event.Is(15) ? ((AttackerTouchedBallInDefenseAreaType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(15, value);
        }
        public bool ShouldSerializeAttackerTouchedBallInDefenseArea() => __pbn__event.Is(15);
        public void ResetAttackerTouchedBallInDefenseArea() => DiscriminatedUnionObject.Reset(ref __pbn__event, 15);

        [ProtoMember(18)]
        public BotKickedBallTooFastType BotKickedBallTooFast
        {
            get => __pbn__event.Is(18) ? ((BotKickedBallTooFastType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(18, value);
        }
        public bool ShouldSerializeBotKickedBallTooFast() => __pbn__event.Is(18);
        public void ResetBotKickedBallTooFast() => DiscriminatedUnionObject.Reset(ref __pbn__event, 18);

        [ProtoMember(22)]
        public BotCrashUniqueType BotCrashUnique
        {
            get => __pbn__event.Is(22) ? ((BotCrashUniqueType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(22, value);
        }
        public bool ShouldSerializeBotCrashUnique() => __pbn__event.Is(22);
        public void ResetBotCrashUnique() => DiscriminatedUnionObject.Reset(ref __pbn__event, 22);

        [ProtoMember(21)]
        public BotCrashDrawnType BotCrashDrawn
        {
            get => __pbn__event.Is(21) ? ((BotCrashDrawnType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(21, value);
        }
        public bool ShouldSerializeBotCrashDrawn() => __pbn__event.Is(21);
        public void ResetBotCrashDrawn() => DiscriminatedUnionObject.Reset(ref __pbn__event, 21);

        [ProtoMember(29)]
        public DefenderTooCloseToKickPointType DefenderTooCloseToKickPoint
        {
            get => __pbn__event.Is(29) ? ((DefenderTooCloseToKickPointType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(29, value);
        }
        public bool ShouldSerializeDefenderTooCloseToKickPoint() => __pbn__event.Is(29);
        public void ResetDefenderTooCloseToKickPoint() => DiscriminatedUnionObject.Reset(ref __pbn__event, 29);

        [ProtoMember(28)]
        public BotTooFastInStopType BotTooFastInStop
        {
            get => __pbn__event.Is(28) ? ((BotTooFastInStopType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(28, value);
        }
        public bool ShouldSerializeBotTooFastInStop() => __pbn__event.Is(28);
        public void ResetBotTooFastInStop() => DiscriminatedUnionObject.Reset(ref __pbn__event, 28);

        [ProtoMember(20)]
        public BotInterferedPlacementType BotInterferedPlacement
        {
            get => __pbn__event.Is(20) ? ((BotInterferedPlacementType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(20, value);
        }
        public bool ShouldSerializeBotInterferedPlacement() => __pbn__event.Is(20);
        public void ResetBotInterferedPlacement() => DiscriminatedUnionObject.Reset(ref __pbn__event, 20);

        [ProtoMember(39, Name = @"possible_goal")]
        public GoalType PossibleGoal
        {
            get => __pbn__event.Is(39) ? ((GoalType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(39, value);
        }
        public bool ShouldSerializePossibleGoal() => __pbn__event.Is(39);
        public void ResetPossibleGoal() => DiscriminatedUnionObject.Reset(ref __pbn__event, 39);

        [ProtoMember(8)]
        public GoalType Goal
        {
            get => __pbn__event.Is(8) ? ((GoalType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(8, value);
        }
        public bool ShouldSerializeGoal() => __pbn__event.Is(8);
        public void ResetGoal() => DiscriminatedUnionObject.Reset(ref __pbn__event, 8);

        [ProtoMember(44, Name = @"invalid_goal")]
        public GoalType InvalidGoal
        {
            get => __pbn__event.Is(44) ? ((GoalType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(44, value);
        }
        public bool ShouldSerializeInvalidGoal() => __pbn__event.Is(44);
        public void ResetInvalidGoal() => DiscriminatedUnionObject.Reset(ref __pbn__event, 44);

        [ProtoMember(14)]
        public AttackerDoubleTouchedBallType AttackerDoubleTouchedBall
        {
            get => __pbn__event.Is(14) ? ((AttackerDoubleTouchedBallType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(14, value);
        }
        public bool ShouldSerializeAttackerDoubleTouchedBall() => __pbn__event.Is(14);
        public void ResetattAttackerDoubleTouchedBall() => DiscriminatedUnionObject.Reset(ref __pbn__event, 14);

        [ProtoMember(5)]
        public PlacementSucceededType PlacementSucceeded
        {
            get => __pbn__event.Is(5) ? ((PlacementSucceededType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(5, value);
        }
        public bool ShouldSerializePlacementSucceeded() => __pbn__event.Is(5);
        public void ResetPlacementSucceeded() => DiscriminatedUnionObject.Reset(ref __pbn__event, 5);

        [ProtoMember(45)]
        public PenaltyKickFailedType PenaltyKickFailed
        {
            get => __pbn__event.Is(45) ? ((PenaltyKickFailedType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(45, value);
        }
        public bool ShouldSerializePenaltyKickFailed() => __pbn__event.Is(45);
        public void ResetPenaltyKickFailed() => DiscriminatedUnionObject.Reset(ref __pbn__event, 45);

        [ProtoMember(2)]
        public NoProgressInGameType NoProgressInGame
        {
            get => __pbn__event.Is(2) ? ((NoProgressInGameType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(2, value);
        }
        public bool ShouldSerializeNoProgressInGame() => __pbn__event.Is(2);
        public void ResetNoProgressInGame() => DiscriminatedUnionObject.Reset(ref __pbn__event, 2);

        [ProtoMember(3)]
        public PlacementFailedType PlacementFailed
        {
            get => __pbn__event.Is(3) ? ((PlacementFailedType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(3, value);
        }
        public bool ShouldSerializePlacementFailed() => __pbn__event.Is(3);
        public void ResetPlacementFailed() => DiscriminatedUnionObject.Reset(ref __pbn__event, 3);

        [ProtoMember(32)]
        public MultipleCardsType MultipleCards
        {
            get => __pbn__event.Is(32) ? ((MultipleCardsType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(32, value);
        }
        public bool ShouldSerializeMultipleCards() => __pbn__event.Is(32);
        public void ResetMultipleCards() => DiscriminatedUnionObject.Reset(ref __pbn__event, 32);

        [ProtoMember(34)]
        public MultipleFoulsType MultipleFouls
        {
            get => __pbn__event.Is(34) ? ((MultipleFoulsType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(34, value);
        }
        public bool ShouldSerializeMultipleFouls() => __pbn__event.Is(34);
        public void ResetMultipleFouls() => DiscriminatedUnionObject.Reset(ref __pbn__event, 34);

        [ProtoMember(37)]
        public BotSubstitutionType BotSubstitution
        {
            get => __pbn__event.Is(37) ? ((BotSubstitutionType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(37, value);
        }
        public bool ShouldSerializeBotSubstitution() => __pbn__event.Is(37);
        public void ResetBotSubstitution() => DiscriminatedUnionObject.Reset(ref __pbn__event, 37);

        [ProtoMember(38)]
        public TooManyRobotsType TooManyRobots
        {
            get => __pbn__event.Is(38) ? ((TooManyRobotsType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(38, value);
        }
        public bool ShouldSerializeTooManyRobots() => __pbn__event.Is(38);
        public void ResetTooManyRobots() => DiscriminatedUnionObject.Reset(ref __pbn__event, 38);

        [ProtoMember(46)]
        public ChallengeFlagType ChallengeFlag
        {
            get => __pbn__event.Is(46) ? ((ChallengeFlagType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(46, value);
        }
        public bool ShouldSerializeChallengeFlag() => __pbn__event.Is(46);
        public void ResetChallengeFlag() => DiscriminatedUnionObject.Reset(ref __pbn__event, 46);

        [ProtoMember(47)]
        public EmergencyStopType EmergencyStop
        {
            get => __pbn__event.Is(47) ? ((EmergencyStopType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(47, value);
        }
        public bool ShouldSerializeEmergencyStop() => __pbn__event.Is(47);
        public void ResetEmergencyStop() => DiscriminatedUnionObject.Reset(ref __pbn__event, 47);

        [ProtoMember(35)]
        public UnsportingBehaviorMinorType UnsportingBehaviorMinor
        {
            get => __pbn__event.Is(35) ? ((UnsportingBehaviorMinorType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(35, value);
        }
        public bool ShouldSerializeUnsportingBehaviorMinor() => __pbn__event.Is(35);
        public void ResetUnsportingBehaviorMinor() => DiscriminatedUnionObject.Reset(ref __pbn__event, 35);

        [ProtoMember(36)]
        public UnsportingBehaviorMajor UnsportingBehaviorMajor
        {
            get => __pbn__event.Is(36) ? ((UnsportingBehaviorMajor)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(36, value);
        }
        public bool ShouldSerializeUnsportingBehaviorMajor() => __pbn__event.Is(36);
        public void ResetUnsportingBehaviorMajor() => DiscriminatedUnionObject.Reset(ref __pbn__event, 36);

        [ProtoMember(1)]
        [System.Obsolete]
        public PreparedType Prepared
        {
            get => __pbn__event.Is(1) ? ((PreparedType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(1, value);
        }
        public bool ShouldSerializePrepared() => __pbn__event.Is(1);
        public void ResetPrepared() => DiscriminatedUnionObject.Reset(ref __pbn__event, 1);

        [ProtoMember(9)]
        [System.Obsolete]
        public IndirectGoalType IndirectGoal
        {
            get => __pbn__event.Is(9) ? ((IndirectGoalType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(9, value);
        }
        public bool ShouldSerializeIndirectGoal() => __pbn__event.Is(9);
        public void ResetIndirectGoal() => DiscriminatedUnionObject.Reset(ref __pbn__event, 9);

        [ProtoMember(10)]
        [System.Obsolete]
        public ChippedGoalType ChippedGoal
        {
            get => __pbn__event.Is(10) ? ((ChippedGoalType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(10, value);
        }
        public bool ShouldSerializeChippedGoal() => __pbn__event.Is(10);
        public void ResetChippedGoal() => DiscriminatedUnionObject.Reset(ref __pbn__event, 10);

        [ProtoMember(12)]
        [System.Obsolete]
        public KickTimeoutType KickTimeout
        {
            get => __pbn__event.Is(12) ? ((KickTimeoutType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(12, value);
        }
        public bool ShouldSerializeKickTimeout() => __pbn__event.Is(12);
        public void ResetKickTimeout() => DiscriminatedUnionObject.Reset(ref __pbn__event, 12);

        [ProtoMember(16)]
        [System.Obsolete]
        public AttackerTouchedOpponentInDefenseAreaType AttackerTouchedOpponentInDefenseArea
        {
            get => __pbn__event.Is(16) ? ((AttackerTouchedOpponentInDefenseAreaType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(16, value);
        }
        public bool ShouldSerializeAttackerTouchedOpponentInDefenseArea() => __pbn__event.Is(16);
        public void ResetAttackerTouchedOpponentInDefenseArea() => DiscriminatedUnionObject.Reset(ref __pbn__event, 16);

        [ProtoMember(42, Name = @"attacker_touched_opponent_in_defense_area_skipped")]
        [System.Obsolete]
        public AttackerTouchedOpponentInDefenseAreaType AttackerTouchedOpponentInDefenseAreaSkipped
        {
            get => __pbn__event.Is(42) ? ((AttackerTouchedOpponentInDefenseAreaType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(42, value);
        }
        public bool ShouldSerializeAttackerTouchedOpponentInDefenseAreaSkipped() => __pbn__event.Is(42);
        public void ResetAttackerTouchedOpponentInDefenseAreaSkipped() => DiscriminatedUnionObject.Reset(ref __pbn__event, 42);

        [ProtoMember(23, Name = @"bot_crash_unique_skipped")]
        [System.Obsolete]
        public BotCrashUniqueType BotCrashUniqueSkipped
        {
            get => __pbn__event.Is(23) ? ((BotCrashUniqueType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(23, value);
        }
        public bool ShouldSerializeBotCrashUniqueSkipped() => __pbn__event.Is(23);
        public void ResetBotCrashUniqueSkipped() => DiscriminatedUnionObject.Reset(ref __pbn__event, 23);

        [ProtoMember(25, Name = @"bot_pushed_bot_skipped")]
        [System.Obsolete]
        public BotPushedBotType BotPushedBotSkipped
        {
            get => __pbn__event.Is(25) ? ((BotPushedBotType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(25, value);
        }
        public bool ShouldSerializeBotPushedBotSkipped() => __pbn__event.Is(25);
        public void ResetBotPushedBotSkipped() => DiscriminatedUnionObject.Reset(ref __pbn__event, 25);

        [ProtoMember(30)]
        [System.Obsolete]
        public DefenderInDefenseAreaPartiallyType DefenderInDefenseAreaPartially
        {
            get => __pbn__event.Is(30) ? ((DefenderInDefenseAreaPartiallyType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(30, value);
        }
        public bool ShouldSerializeDefenderInDefenseAreaPartially() => __pbn__event.Is(30);
        public void ResetDefenderInDefenseAreaPartially() => DiscriminatedUnionObject.Reset(ref __pbn__event, 30);

        [ProtoMember(33)]
        [System.Obsolete]
        public MultiplePlacementFailuresType MultiplePlacementFailures
        {
            get => __pbn__event.Is(33) ? ((MultiplePlacementFailuresType)__pbn__event.Object) : default;
            set => __pbn__event = new DiscriminatedUnionObject(33, value);
        }
        public bool ShouldSerializeMultiplePlacementFailures() => __pbn__event.Is(33);
        public void ResetMultiplePlacementFailures() => DiscriminatedUnionObject.Reset(ref __pbn__event, 33);


    }

}
#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
