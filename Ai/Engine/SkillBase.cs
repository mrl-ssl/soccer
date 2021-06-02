using MRL.SSL.Ai.MotionPlanner;

namespace MRL.SSL.Ai.Engine
{
    public abstract class SkillBase
    {
        protected PathPlanner planner = new PathPlanner();
        protected Controller controller = new Controller();

    }
}