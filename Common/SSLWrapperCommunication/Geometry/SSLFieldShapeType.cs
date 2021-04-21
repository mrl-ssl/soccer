
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract]
    public enum SSLFieldShapeType
    {
        Undefined = 0,
        CenterCircle = 1,
        TopTouchLine = 2,
        BottomTouchLine = 3,
        LeftGoalLine = 4,
        RightGoalLine = 5,
        HalfwayLine = 6,
        CenterLine = 7,
        LeftPenaltyStretch = 8,
        RightPenaltyStretch = 9,
        LeftFieldLeftPenaltyStretch = 10,
        LeftFieldRightPenaltyStretch = 11,
        RightFieldLeftPenaltyStretch = 12,
        RightFieldRightPenaltyStretch = 13,
    }
}