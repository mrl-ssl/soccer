
using static MRL.SSL.Common.SSLWrapperCommunication.SSLRefereePacket;

namespace MRL.SSL.Common
{
    public enum GameStatus
    {
        Halt = 0,
        Stop = 1,
        Normal,
        KickOffOurTeamWaiting,
        KickOffOurTeamGo,
        KickOffOpponentWaiting,
        KickOffOpponentGo,
        PenaltyOurTeamWaiting,
        PenaltyOurTeamGo,
        PenaltyOpponentWaiting,
        PenaltyOpponentGo,
        DirectFreeKickOurTeam,
        DirectFreeKickOpponent,
        IndirectFreeKickOurTeam,
        IndirectFreeKickOpponent,
        TimeoutOurTeam,
        TimeoutOpponent,
        BallPlaceOurTeam,
        BallPlaceOpponent,

    }
    public class GameStatusCalculator
    {
        public static GameStatus CalculateGameStatus(GameStatus LastGameStatus, CommandType command, bool OurTeamIsYellow)
        {
            switch (command)
            {
                case CommandType.Halt:
                    return GameStatus.Halt;
                case CommandType.Stop:
                    return GameStatus.Stop;
                case CommandType.NormalStart:
                    {
                        switch (LastGameStatus)
                        {
                            case GameStatus.KickOffOpponentWaiting:
                                return GameStatus.KickOffOpponentGo;
                            case GameStatus.KickOffOurTeamWaiting:
                                return GameStatus.KickOffOurTeamGo;
                            case GameStatus.PenaltyOpponentWaiting:
                                return GameStatus.PenaltyOpponentGo;
                            case GameStatus.PenaltyOurTeamWaiting:
                                return GameStatus.PenaltyOurTeamGo;
                            default:
                                return GameStatus.Normal;
                        }
                    }
                case CommandType.ForceStart:
                    {
                        return GameStatus.Normal;
                    }
                case CommandType.PrepareKickoffBlue:
                case CommandType.PrepareKickoffYellow:
                    return ((command == CommandType.PrepareKickoffYellow) ^ OurTeamIsYellow) ? GameStatus.KickOffOpponentWaiting : GameStatus.KickOffOurTeamWaiting;
                case CommandType.PreparePenaltyBlue:
                case CommandType.PreparePenaltyYellow:
                    return ((command == CommandType.PreparePenaltyYellow) ^ OurTeamIsYellow) ? GameStatus.PenaltyOpponentWaiting : GameStatus.PenaltyOurTeamWaiting;
                case CommandType.DirectFreeBlue:
                case CommandType.DirectFreeYellow:
                    return ((command == CommandType.DirectFreeYellow) ^ OurTeamIsYellow) ? GameStatus.DirectFreeKickOpponent : GameStatus.DirectFreeKickOurTeam;
                case CommandType.IndirectFreeBlue:
                case CommandType.IndirectFreeYellow:
                    return ((command == CommandType.IndirectFreeYellow) ^ OurTeamIsYellow) ? GameStatus.IndirectFreeKickOpponent : GameStatus.IndirectFreeKickOurTeam;
                case CommandType.TimeoutBlue:
                case CommandType.TimeoutYellow:
                    //return ((p == 't') ^ OurTeamIsYellow) ? GameStatus.Halt : GameStatus.Halt;
                    return ((command == CommandType.TimeoutYellow) ^ OurTeamIsYellow) ? GameStatus.TimeoutOpponent : GameStatus.TimeoutOurTeam;
                case CommandType.BallPlacementBlue:
                case CommandType.BallPlacementYellow:
                    return ((command == CommandType.BallPlacementYellow) ^ OurTeamIsYellow) ? GameStatus.BallPlaceOpponent : GameStatus.BallPlaceOurTeam;
                default:
                    return LastGameStatus;
            }
        }
    }
}