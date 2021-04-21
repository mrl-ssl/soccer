using System.Collections;
using System.Collections.Generic;

namespace MRL.SSL.Common
{
    public class WorldModel
    {
        bool ourMarkerIsYellow;
        bool fieldIsInverted;
        int? goalieID;
        IDictionary<int, SingleObjectState> ourRobots;
        IDictionary<int, SingleObjectState> opponents;

        IDictionary<int, RawObjectState> rawOurRobots;
        IDictionary<int, RawObjectState> rawOpponents;

        SingleObjectState ballState;
        RawObjectState rawBallState;

        public bool OurMarkerIsYellow { get => ourMarkerIsYellow; set => ourMarkerIsYellow = value; }
        public bool FieldIsInverted { get => fieldIsInverted; set => fieldIsInverted = value; }
        public int? GoalieID { get => goalieID; set => goalieID = value; }
        public IDictionary<int, SingleObjectState> OurRobots { get => ourRobots; set => ourRobots = value; }
        public IDictionary<int, SingleObjectState> Opponents { get => opponents; set => opponents = value; }
        public IDictionary<int, RawObjectState> RawOurRobots { get => rawOurRobots; set => rawOurRobots = value; }
        public IDictionary<int, RawObjectState> RawOpponents { get => rawOpponents; set => rawOpponents = value; }
        public SingleObjectState BallState { get => ballState; set => ballState = value; }
        public RawObjectState RawBallState { get => rawBallState; set => rawBallState = value; }
    }
}