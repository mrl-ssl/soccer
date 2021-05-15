using System;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;

namespace MRL.SSL.Common
{
    public static class GameParameters
    {
        public static FieldConfig Field { get; set; } = FieldConfig.Default;
        public static bool IsUpdated { get; set; }
        public static bool ReUpdate { get; set; }

        public static void UpdateParamsFromGeometry(SSLGeometryFieldSize f, bool isReverse)
        {
            var arcs = f.FieldArcs != null ? f.FieldArcs : new();
            var lines = f.FieldLines != null ? f.FieldLines : new();

            var _field = new FieldConfig();
            var length = f.FieldLength > 0 ? f.FieldLength : Field.FieldLength * 1000f;
            var width = f.FieldWidth > 0 ? f.FieldWidth : Field.FieldWidth * 1000f;
            var gWidth = f.GoalWidth > 0 ? f.GoalWidth : Field.GoalWidth * 1000f;
            var gDepth = f.GoalDepth > 0 ? f.GoalDepth : Field.GoalDepth * 1000f;
            var penaltyWidth = f.PenaltyAreaWidth > 0 ? f.PenaltyAreaWidth : Field.PenaltyAreaWidth * 1000f;
            var penaltyDepth = f.PenaltyAreaDepth > 0 ? f.PenaltyAreaDepth : Field.PenaltyAreaDepth * 1000f;

            _field.BoundaryWidth = f.BoundaryWidth > 0 ? f.BoundaryWidth / 1000f : Field.BoundaryWidth;
            _field.Thickness = lines.Count > 0 && lines[0].Thickness > 0 ? lines[0].Thickness / 1000f : Field.Thickness;
            _field.FieldLength = length / 1000f;
            _field.FieldWidth = width / 1000f;
            _field.GoalWidth = gWidth / 1000f;
            _field.GoalDepth = gDepth / 1000f;
            _field.PenaltyAreaWidth = penaltyWidth / 1000;
            _field.PenaltyAreaDepth = penaltyDepth / 1000f;

            var idx = arcs.FindIndex(a => a.Name == "CenterCircle");

            if (idx != -1)
                _field.CenterCircleRadius = arcs[idx].Radius / 1000f;
            else
                _field.CenterCircleRadius = Field.CenterCircleRadius;

            idx = lines.FindIndex(a => a.Name == "TopTouchLine");

            var l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(-length / 2f, width / 2),
                P2 = new VectorF2D(length / 2f, width / 2),
            };

            if (isReverse)
            {
                _field.OurLeftCorner = l.P1.ToAiCoordinate(isReverse);
                _field.OppRightCorner = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OppLeftCorner = l.P1.ToAiCoordinate(isReverse);
                _field.OurRightCorner = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "BottomTouchLine");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(-length / 2f, -width / 2),
                P2 = new VectorF2D(length / 2f, -width / 2),
            };

            if (isReverse)
            {
                _field.OurRightCorner = l.P1.ToAiCoordinate(isReverse);
                _field.OppLeftCorner = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OppRightCorner = l.P1.ToAiCoordinate(isReverse);
                _field.OurLeftCorner = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "RightGoalTopLine");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(length / 2f, gWidth / 2),
                P2 = new VectorF2D(length / 2f + gDepth, gWidth / 2),
            };

            if (isReverse)
            {
                _field.OppGoalRight = l.P1.ToAiCoordinate(isReverse);
                _field.OppGoalDepthRight = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OurGoalRight = l.P1.ToAiCoordinate(isReverse);
                _field.OurGoalDepthRight = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "RightGoalBottomLine");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(length / 2f, -gWidth / 2),
                P2 = new VectorF2D(length / 2f + gDepth, -gWidth / 2),
            };
            if (isReverse)
            {
                _field.OppGoalLeft = l.P1.ToAiCoordinate(isReverse);
                _field.OppGoalDepthLeft = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OurGoalLeft = l.P1.ToAiCoordinate(isReverse);
                _field.OurGoalDepthLeft = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "LeftGoalTopLine");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(-length / 2f, gWidth / 2),
                P2 = new VectorF2D(-length / 2f - gDepth, gWidth / 2)
            };
            if (isReverse)
            {
                _field.OurGoalLeft = l.P1.ToAiCoordinate(isReverse);
                _field.OurGoalDepthLeft = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OppGoalLeft = l.P1.ToAiCoordinate(isReverse);
                _field.OppGoalDepthLeft = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "LeftGoalBottomLine");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(-length / 2f, -gWidth / 2),
                P2 = new VectorF2D(-length / 2f - gDepth, -gWidth / 2)
            };
            if (isReverse)
            {
                _field.OurGoalRight = l.P1.ToAiCoordinate(isReverse);
                _field.OurGoalDepthRight = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OppGoalRight = l.P1.ToAiCoordinate(isReverse);
                _field.OppGoalDepthRight = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "RightFieldRightPenaltyStretch");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(length / 2f, penaltyWidth / 2),
                P2 = new VectorF2D(length / 2f - penaltyDepth, penaltyWidth / 2),
            };

            if (isReverse)
            {
                _field.OppPenaltyBackRight = l.P1.ToAiCoordinate(isReverse);
                _field.OppPenaltyRearRight = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OurPenaltyBackRight = l.P1.ToAiCoordinate(isReverse);
                _field.OurPenaltyRearRight = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "RightFieldLeftPenaltyStretch");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(length / 2f, -penaltyWidth / 2),
                P2 = new VectorF2D(length / 2f - penaltyDepth, -penaltyWidth / 2),
            };

            if (isReverse)
            {
                _field.OppPenaltyBackLeft = l.P1.ToAiCoordinate(isReverse);
                _field.OppPenaltyRearLeft = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OurPenaltyBackLeft = l.P1.ToAiCoordinate(isReverse);
                _field.OurPenaltyRearLeft = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "LeftFieldRightPenaltyStretch");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(-length / 2f, -penaltyWidth / 2),
                P2 = new VectorF2D(-length / 2f + penaltyDepth, -penaltyWidth / 2)
            };

            if (isReverse)
            {
                _field.OurPenaltyBackRight = l.P1.ToAiCoordinate(isReverse);
                _field.OurPenaltyRearRight = l.P2.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OppPenaltyBackRight = l.P1.ToAiCoordinate(isReverse);
                _field.OppPenaltyRearRight = l.P2.ToAiCoordinate(isReverse);
            }

            idx = lines.FindIndex(a => a.Name == "LeftFieldLeftPenaltyStretch");
            l = idx != -1 ? lines[idx] : new SSLFieldLineSegment()
            {
                P1 = new VectorF2D(-length / 2f, penaltyWidth / 2),
                P2 = new VectorF2D(-length / 2f + penaltyDepth, penaltyWidth / 2)
            };

            if (isReverse)
            {
                _field.OurPenaltyRearLeft = l.P2.ToAiCoordinate(isReverse);
                _field.OurPenaltyBackLeft = l.P1.ToAiCoordinate(isReverse);
            }
            else
            {
                _field.OppPenaltyBackLeft = l.P1.ToAiCoordinate(isReverse);
                _field.OppPenaltyRearLeft = l.P2.ToAiCoordinate(isReverse);
            }
            _field.OurGoalCenter = _field.OurGoalLeft.Interpolate(_field.OurGoalRight, 0.5f);
            _field.OppGoalCenter = _field.OppGoalLeft.Interpolate(_field.OppGoalRight, 0.5f);

            Field = _field;
            IsUpdated = true;
        }

        public static bool IsInField(VectorF2D location, bool boundary, float margin = 0)
        {
            var marg = boundary ? Field.BoundaryWidth + margin : margin;
            return (MathF.Abs(location.X) < Field.OurGoalCenter.X + marg)
                   || (MathF.Abs(location.Y) < Field.OurLeftCorner.Y);
        }
    }
}