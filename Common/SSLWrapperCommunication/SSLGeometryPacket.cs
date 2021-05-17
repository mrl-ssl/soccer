#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192

using System.Collections.Generic;
using System.ComponentModel;
using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.SSLWrapperCommunication
{
    [ProtoContract(Name = @"SSL_FieldLineSegment")]
    public partial class SSLFieldLineSegment : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"name", IsRequired = true)]
        public string Name { get; set; }

        [ProtoMember(2, Name = @"p1", IsRequired = true)]
        public VectorF2D P1 { get; set; }

        [ProtoMember(3, Name = @"p2", IsRequired = true)]
        public VectorF2D P2 { get; set; }

        [ProtoMember(4, Name = @"thickness", IsRequired = true)]
        public float Thickness { get; set; }

        [ProtoMember(5, Name = @"type")]
        [DefaultValue(SSLFieldShapeType.Undefined)]
        public SSLFieldShapeType Type
        {
            get => __pbn__Type ?? SSLFieldShapeType.Undefined;
            set => __pbn__Type = value;
        }
        public bool ShouldSerializeType() => __pbn__Type != null;
        public void ResetType() => __pbn__Type = null;
        private SSLFieldShapeType? __pbn__Type;

    }

    [ProtoContract(Name = @"SSL_FieldCircularArc")]
    public partial class SSLFieldCircularArc : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"name", IsRequired = true)]
        public string Name { get; set; }

        [ProtoMember(2, Name = @"center", IsRequired = true)]
        public VectorF2D Center { get; set; }

        [ProtoMember(3, Name = @"radius", IsRequired = true)]
        public float Radius { get; set; }

        [ProtoMember(4, Name = @"a1", IsRequired = true)]
        public float A1 { get; set; }

        [ProtoMember(5, Name = @"a2", IsRequired = true)]
        public float A2 { get; set; }

        [ProtoMember(6, Name = @"thickness", IsRequired = true)]
        public float Thickness { get; set; }

        [ProtoMember(7, Name = @"type")]
        [DefaultValue(SSLFieldShapeType.Undefined)]
        public SSLFieldShapeType Type
        {
            get => __pbn__Type ?? SSLFieldShapeType.Undefined;
            set => __pbn__Type = value;
        }
        public bool ShouldSerializeType() => __pbn__Type != null;
        public void ResetType() => __pbn__Type = null;
        private SSLFieldShapeType? __pbn__Type;

    }

    [ProtoContract(Name = @"SSL_GeometryFieldSize")]
    public partial class SSLGeometryFieldSize : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"field_length", IsRequired = true)]
        public int FieldLength { get; set; }

        [ProtoMember(2, Name = @"field_width", IsRequired = true)]
        public int FieldWidth { get; set; }

        [ProtoMember(3, Name = @"goal_width", IsRequired = true)]
        public int GoalWidth { get; set; }

        [ProtoMember(4, Name = @"goal_depth", IsRequired = true)]
        public int GoalDepth { get; set; }

        [ProtoMember(5, Name = @"boundary_width", IsRequired = true)]
        public int BoundaryWidth { get; set; }

        [ProtoMember(6, Name = @"field_lines")]
        public List<SSLFieldLineSegment> FieldLines { get; } = new List<SSLFieldLineSegment>();

        [ProtoMember(7, Name = @"field_arcs")]
        public List<SSLFieldCircularArc> FieldArcs { get; } = new List<SSLFieldCircularArc>();

        [ProtoMember(8, Name = @"penalty_area_depth")]
        public int PenaltyAreaDepth
        {
            get => __pbn__PenaltyAreaDepth.GetValueOrDefault();
            set => __pbn__PenaltyAreaDepth = value;
        }
        public bool ShouldSerializePenaltyAreaDepth() => __pbn__PenaltyAreaDepth != null;
        public void ResetPenaltyAreaDepth() => __pbn__PenaltyAreaDepth = null;
        private int? __pbn__PenaltyAreaDepth;

        [ProtoMember(9, Name = @"penalty_area_width")]
        public int PenaltyAreaWidth
        {
            get => __pbn__PenaltyAreaWidth.GetValueOrDefault();
            set => __pbn__PenaltyAreaWidth = value;
        }
        public bool ShouldSerializePenaltyAreaWidth() => __pbn__PenaltyAreaWidth != null;
        public void ResetPenaltyAreaWidth() => __pbn__PenaltyAreaWidth = null;
        private int? __pbn__PenaltyAreaWidth;

    }

    [ProtoContract(Name = @"SSL_GeometryCameraCalibration")]
    public partial class SSLGeometryCameraCalibration : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"camera_id", IsRequired = true)]
        public uint CameraId { get; set; }

        [ProtoMember(2, Name = @"focal_length", IsRequired = true)]
        public float FocalLength { get; set; }

        [ProtoMember(3, Name = @"principal_point_x", IsRequired = true)]
        public float PrincipalPointX { get; set; }

        [ProtoMember(4, Name = @"principal_point_y", IsRequired = true)]
        public float PrincipalPointY { get; set; }

        [ProtoMember(5, Name = @"distortion", IsRequired = true)]
        public float Distortion { get; set; }

        [ProtoMember(6, Name = @"q0", IsRequired = true)]
        public float Q0 { get; set; }

        [ProtoMember(7, Name = @"q1", IsRequired = true)]
        public float Q1 { get; set; }

        [ProtoMember(8, Name = @"q2", IsRequired = true)]
        public float Q2 { get; set; }

        [ProtoMember(9, Name = @"q3", IsRequired = true)]
        public float Q3 { get; set; }

        [ProtoMember(10, Name = @"tx", IsRequired = true)]
        public float Tx { get; set; }

        [ProtoMember(11, Name = @"ty", IsRequired = true)]
        public float Ty { get; set; }

        [ProtoMember(12, Name = @"tz", IsRequired = true)]
        public float Tz { get; set; }

        [ProtoMember(13, Name = @"derived_camera_world_tx")]
        public float DerivedCameraWorldTx
        {
            get => __pbn__DerivedCameraWorldTx.GetValueOrDefault();
            set => __pbn__DerivedCameraWorldTx = value;
        }
        public bool ShouldSerializeDerivedCameraWorldTx() => __pbn__DerivedCameraWorldTx != null;
        public void ResetDerivedCameraWorldTx() => __pbn__DerivedCameraWorldTx = null;
        private float? __pbn__DerivedCameraWorldTx;

        [ProtoMember(14, Name = @"derived_camera_world_ty")]
        public float DerivedCameraWorldTy
        {
            get => __pbn__DerivedCameraWorldTy.GetValueOrDefault();
            set => __pbn__DerivedCameraWorldTy = value;
        }
        public bool ShouldSerializeDerivedCameraWorldTy() => __pbn__DerivedCameraWorldTy != null;
        public void ResetDerivedCameraWorldTy() => __pbn__DerivedCameraWorldTy = null;
        private float? __pbn__DerivedCameraWorldTy;

        [ProtoMember(15, Name = @"derived_camera_world_tz")]
        public float DerivedCameraWorldTz
        {
            get => __pbn__DerivedCameraWorldTz.GetValueOrDefault();
            set => __pbn__DerivedCameraWorldTz = value;
        }
        public bool ShouldSerializeDerivedCameraWorldTz() => __pbn__DerivedCameraWorldTz != null;
        public void ResetDerivedCameraWorldTz() => __pbn__DerivedCameraWorldTz = null;
        private float? __pbn__DerivedCameraWorldTz;

        [ProtoMember(16, Name = @"pixel_image_width")]
        public uint PixelImageWidth
        {
            get => __pbn__PixelImageWidth.GetValueOrDefault();
            set => __pbn__PixelImageWidth = value;
        }
        public bool ShouldSerializePixelImageWidth() => __pbn__PixelImageWidth != null;
        public void ResetPixelImageWidth() => __pbn__PixelImageWidth = null;
        private uint? __pbn__PixelImageWidth;

        [ProtoMember(17, Name = @"pixel_image_height")]
        public uint PixelImageHeight
        {
            get => __pbn__PixelImageHeight.GetValueOrDefault();
            set => __pbn__PixelImageHeight = value;
        }
        public bool ShouldSerializePixelImageHeight() => __pbn__PixelImageHeight != null;
        public void ResetPixelImageHeight() => __pbn__PixelImageHeight = null;
        private uint? __pbn__PixelImageHeight;

    }

    [ProtoContract(Name = @"SSL_BallModelStraightTwoPhase")]
    public partial class SSLBallModelStraightTwoPhase : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"acc_slide", IsRequired = true)]
        public double AccSlide { get; set; }

        [ProtoMember(2, Name = @"acc_roll", IsRequired = true)]
        public double AccRoll { get; set; }

        [ProtoMember(3, Name = @"k_switch", IsRequired = true)]
        public double KSwitch { get; set; }

    }

    [ProtoContract(Name = @"SSL_BallModelChipFixedLoss")]
    public partial class SSLBallModelChipFixedLoss : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"damping_xy_first_hop", IsRequired = true)]
        public double DampingXyFirstHop { get; set; }

        [ProtoMember(2, Name = @"damping_xy_other_hops", IsRequired = true)]
        public double DampingXyOtherHops { get; set; }

        [ProtoMember(3, Name = @"damping_z", IsRequired = true)]
        public double DampingZ { get; set; }

    }

    [ProtoContract(Name = @"SSL_GeometryModels")]
    public partial class SSLGeometryModels : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"straight_two_phase")]
        public SSLBallModelStraightTwoPhase StraightTwoPhase { get; set; }

        [ProtoMember(2, Name = @"chip_fixed_loss")]
        public SSLBallModelChipFixedLoss ChipFixedLoss { get; set; }

    }

    [ProtoContract(Name = @"SSL_GeometryData")]
    public partial class SSLGeometryData : IExtensible
    {
        private IExtension __pbn__extensionData;
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
            => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [ProtoMember(1, Name = @"field", IsRequired = true)]
        public SSLGeometryFieldSize Field { get; set; }

        [ProtoMember(2, Name = @"calib")]
        public List<SSLGeometryCameraCalibration> Calibs { get; } = new List<SSLGeometryCameraCalibration>();

        [ProtoMember(3, Name = @"models")]
        public SSLGeometryModels Models { get; set; }

    }

    [ProtoContract(Name = @"SSL_FieldShapeType")]
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

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
