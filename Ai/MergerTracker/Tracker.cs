using System;
using System.Collections.Generic;
using MRL.SSL.Ai.Utils;
using MRL.SSL.Common;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;

namespace MRL.SSL.Ai.MergerTracker
{
    public class Tracker
    {
        private int[,] id2index;
        private int[,] index2id;
        private RobotKalman[,] robots;
        private BallKalman ball;
        public IDictionary<uint, SSLGeometryCameraCalibration> Cameras { get; set; }

        public RobotKalman[,] Robots { get; set; }
        VectorF2D lastBallLoc;

        public Tracker()
        {
            id2index = new int[MergerTrackerConfig.Default.TeamsCount, MergerTrackerConfig.Default.MaxRobotId];
            index2id = new int[MergerTrackerConfig.Default.TeamsCount, MergerTrackerConfig.Default.MaxTeamRobots];
            robots = new RobotKalman[MergerTrackerConfig.Default.TeamsCount, MergerTrackerConfig.Default.MaxTeamRobots];
            ball = new BallKalman();
            lastBallLoc = VectorF2D.Zero;

            for (int t = 0; t < MergerTrackerConfig.Default.TeamsCount; t++)
            {
                for (int i = 0; i < MergerTrackerConfig.Default.MaxTeamRobots; i++)
                {
                    index2id[t, i] = -1;
                    if (t == 0)
                        robots[t, i] = new OurRobotKalman(MergerTrackerConfig.Default.Latency);
                    else robots[t, i] = new OppRobotKalman();
                }
                for (int i = 0; i < MergerTrackerConfig.Default.MaxRobotId; i++)
                    id2index[t, i] = -1;
            }

        }
        public float GetHeight(int team) => team == 0 ? MergerTrackerConfig.Default.OurRobotHeight : MergerTrackerConfig.Default.OpponentHeight;
        public float GetRadius(int team) => team == 0 ? MergerTrackerConfig.Default.OurRobotRadius : MergerTrackerConfig.Default.OpponentRadius;
        public bool Exists(int team, int idx) => (index2id[team, idx] >= 0);
        public RobotKalman GetRobot(int team, int idx) => robots[team, idx];
        public RobotKalman GetRobotById(int team, int id) => id2index[team, id] > 0 ? robots[team, id2index[team, id]] : null;
        private void ResetForgottens(ObservationModel model)
        {
            for (int t = 0; t < MergerTrackerConfig.Default.TeamsCount; t++)
            {
                for (int i = 0; i < MergerTrackerConfig.Default.MaxTeamRobots; i++)
                    index2id[t, i] = -1;
                for (int i = 0; i < MergerTrackerConfig.Default.MaxRobotId; i++)
                    id2index[t, i] = -1;
            }
            if (model != null)
            {
                int idx = 0;
                foreach (var item in model.Teammates.Keys)
                {
                    index2id[0, idx] = item;
                    id2index[0, item] = idx;
                    idx++;
                }
                idx = 0;
                foreach (var item in model.Opponents.Keys)
                {
                    index2id[1, idx] = item;
                    id2index[1, item] = idx;
                    idx++;
                }
            }
            for (int t = 0; t < MergerTrackerConfig.Default.TeamsCount; t++)
            {
                for (int i = 0; i < MergerTrackerConfig.Default.MaxTeamRobots; i++)
                    if (!Exists(t, i)) robots[t, i].Reset();
            }
            if (model.Ball == null)
                ball.Reset();

        }
        private SingleObjectState GetRobotState(int team, int indx, double dt)
        {
            var loc = robots[team, indx].Position(dt).ToAiCoordinate(GameConfig.Default.IsFieldInverted);
            var angle = robots[team, indx].Direction(dt);
            if (GameConfig.Default.IsFieldInverted)
            {
                angle = MathHelper.AngleMod(angle + MathF.PI);
            }

            var speed = robots[team, indx].Velocity(dt).ToAiCoordinate(GameConfig.Default.IsFieldInverted);
            var angularSpeed = robots[team, indx].AngularVelocity(dt);
            float stuck = 0;
            if (team == 0) stuck = ((OurRobotKalman)robots[team, indx]).Stuck(dt);
            return new SingleObjectState(loc, speed, angle, angularSpeed, 1);
        }
        private SingleObjectState GetBallState(double dt)
        {
            BallObservationMeta m = null;
            return GetBallState(dt, ref m);
        }
        private SingleObjectState GetBallState(double dt, ref BallObservationMeta meta)
        {
            var loc = ball.Position(dt).ToAiCoordinate(GameConfig.Default.IsFieldInverted);
            var speed = ball.Velocity(dt).ToAiCoordinate(GameConfig.Default.IsFieldInverted);
            if (meta != null)
            {
                int team = -1, robot = -1;
                var cov = ball.Covariances(dt);
                var col = ball.Collision(dt, ref team, ref robot);
                meta.Occluded = ball.Occluded;
                meta.OccludingTeam = ball.OccludingTeam;
                meta.OccludingId = index2id[ball.OccludingTeam, ball.OccludingRobot];
                meta.OccludingOffset = ball.OccludingOffset.ToAiCoordinate(GameConfig.Default.IsFieldInverted);
                meta.Covariances = cov;
                meta.HasCollision = col;
                meta.CollidedTeam = team;
                meta.CollidedRobot = robot;
            }
            return new SingleObjectState(loc, speed);
        }
        public void ObserveModel(ObservationModel model, RobotCommands commands)
        {
            ResetForgottens(model);
            foreach (var key in commands.Commands.Keys)
            {
                var cmd = commands.Commands[key];
                int idx = id2index[0, key];
                if (idx >= 0)
                {
                    var r = model.Teammates[key];
                    ((OurRobotKalman)robots[0, idx]).PushCommand(new VectorF3D(cmd.Vx * 1000, cmd.Vy * 1000, cmd.W),
                                                                 r.Time + r.NotSeen * MergerTrackerConfig.Default.FramePeriod);
                }
            }
            foreach (var key in model.Teammates.Keys)
            {
                var r = model.Teammates[key];
                if (r.Vision != null)
                {
                    robots[0, id2index[0, key]].VisionProblem = false;
                    robots[0, id2index[0, key]].Observe(r.Vision, r.Time);
                }
            }
            foreach (var key in model.Opponents.Keys)
            {
                var r = model.Opponents[key];
                if (r.Vision != null)
                {
                    robots[1, id2index[1, key]].VisionProblem = false;
                    robots[1, id2index[1, key]].Observe(r.Vision, r.Time);
                }
            }
            if (model.Ball != null)
            {
                var b = model.Ball;
                if (b.Vision != null)
                {
                    ball.CheckCollision = true;
                    ball.Observe(b.Vision, b.Time);
                }
            }
        }
        public WorldModel GetEstimations(ObservationModel obsModel)
        {
            WorldModel model = new();
            foreach (var key in obsModel.Teammates.Keys)
            {
                var r = obsModel.Teammates[key];
                var actionDelay = MergerTrackerConfig.Default.ActionDelay + r.NotSeen * MergerTrackerConfig.Default.FramePeriod;
                var viewDelay = (r.NotSeen + 1) * MergerTrackerConfig.Default.FramePeriod;
                var state = GetRobotState(0, id2index[0, key], actionDelay);
                var viewState = GetRobotState(0, id2index[0, key], viewDelay);
                r.ViewState = viewState;
                model.Teammates.Add(key, state);
            }
            foreach (var key in obsModel.Opponents.Keys)
            {
                var r = obsModel.Opponents[key];
                var actionDelay = MergerTrackerConfig.Default.ActionDelay + r.NotSeen * MergerTrackerConfig.Default.FramePeriod;
                var viewDelay = (r.NotSeen + 1) * MergerTrackerConfig.Default.FramePeriod;

                var state = GetRobotState(1, id2index[1, key], actionDelay);
                var viewState = GetRobotState(1, id2index[1, key], viewDelay);
                r.ViewState = viewState;
                model.Opponents.Add(key, state);
            }
            if (obsModel.Ball != null)
            {
                var b = obsModel.Ball;
                var actionDelay = MergerTrackerConfig.Default.ActionDelay + b.NotSeen * MergerTrackerConfig.Default.FramePeriod;
                var viewDelay = (b.NotSeen + 1) * MergerTrackerConfig.Default.FramePeriod;

                var state = GetBallState(actionDelay, ref b);
                var viewState = GetBallState(viewDelay);
                b.ViewState = viewState;
                model.Ball = state;
            }

            if (model.Ball == null)
                model.Ball = new SingleObjectState(lastBallLoc, VectorF2D.Zero);

            lastBallLoc = model.Ball.Location;
            model.Observations = obsModel;
            return model;
        }
    }
}