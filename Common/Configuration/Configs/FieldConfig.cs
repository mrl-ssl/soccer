using MRL.SSL.Common.Math;
using MRL.SSL.Common.SSLWrapperCommunication;
using System.Linq;

namespace MRL.SSL.Common.Configuration
{
    public class FieldConfig : ConfigBase
    {
        public new static FieldConfig Default { get => (FieldConfig)_default[(int)ConfigType.Field]; }
        public override ConfigType Id => ConfigType.Field;
        public float BorderWidth { get; set; }
        public float GoalWidth { get; set; }
        public float GoalDepth { get; set; }
        public float CenterCircleRadi { get; set; }
        public float DefenceAreaHeight { get; set; }
        public float DefenceAreaWidth { get; set; }
        public VectorF2D OurLeftCorner { get; set; }
        public VectorF2D OurRightCorner { get; set; }
        public VectorF2D OppLeftCorner { get; set; }
        public VectorF2D OppRightCorner { get; set; }
        public VectorF2D OurGoalLeft { get; set; }
        public VectorF2D OurGoalRight { get; set; }
        public VectorF2D OurGoalCenter { get; set; }
        public VectorF2D OppGoalLeft { get; set; }
        public VectorF2D OppGoalRight { get; set; }
        public VectorF2D OppGoalCenter { get; set; }

        public void UpdateFromGeometry(SSLGeometryData geometry)
        {
            if (geometry != null)
            {
                if (geometry.Field.FieldLines.Count > 0)
                    BorderWidth = geometry.Field.FieldLines[0].Thickness / 1000.0f;
                if (geometry.Field.FieldArcs.Any(a => a.Name == "CenterCircle"))
                    CenterCircleRadi = geometry.Field.FieldArcs.Where(w => w.Name == "CenterCircle").First().Radius / 1000.0f;

                DefenceAreaHeight = geometry.Field.PenaltyAreaDepth;
                DefenceAreaWidth = geometry.Field.PenaltyAreaWidth;

                OppGoalCenter = new VectorF2D(-geometry.Field.FieldLength / 2000.0f, 0f);
                OppGoalRight = new VectorF2D(-geometry.Field.FieldLength / 2000.0f, -geometry.Field.GoalWidth / 2000.0f);
                OppGoalLeft = new VectorF2D(-geometry.Field.FieldLength / 2000.0f, geometry.Field.GoalWidth / 2000.0f);

                OurGoalCenter = new VectorF2D(geometry.Field.FieldLength / 2000.0f, 0f);
                OurGoalRight = new VectorF2D(geometry.Field.FieldLength / 2000.0f, geometry.Field.GoalWidth / 2000.0f);
                OurGoalLeft = new VectorF2D(geometry.Field.FieldLength / 2000.0f, -geometry.Field.GoalWidth / 2000.0f);


                OurLeftCorner = new VectorF2D(geometry.Field.FieldLength / 2000.0f, -geometry.Field.FieldWidth / 2000.0f);
                OurRightCorner = new VectorF2D(geometry.Field.FieldLength / 2000.0f, geometry.Field.FieldWidth / 2000.0f);

                OppLeftCorner = new VectorF2D(-geometry.Field.FieldLength / 2000.0f, geometry.Field.FieldWidth / 2000.0f);
                OppRightCorner = new VectorF2D(-geometry.Field.FieldLength / 2000.0f, -geometry.Field.FieldWidth / 2000.0f);

            }
        }
        public FieldConfig()
        {
        }

    }
}