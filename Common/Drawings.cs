
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using MRL.SSL.Common.Utils;
using ProtoBuf;
using System.Linq;

namespace MRL.SSL.Common
{
    public static class Drawings
    {
        private static object _lock = new object();

        public static List<DrawableObject> Objects { get; set; } = new List<DrawableObject>();

        private static void AddObject(DrawableObject obj)
        {
            lock (_lock)
            {
                if (GameConfig.Default.Debug)
                    Objects.Add(obj);
            }
        }

        /*private static string Convert2HexARGB(Color color, float opacity)
        {
            int intArgb = color.ToArgb();
            byte[] argbBytes = new byte[4];
            for (int i = 0; i < argbBytes.Length; i++)
                argbBytes[i] = Convert.ToByte((intArgb >> (8 * (argbBytes.Length - 1 - i))) & 0xff);
            return '#' + Convert.ToHexString(argbBytes[1..]);
        }*/

        private static uint Convert2Argb(Color color, float opacity)
        {
            int a = 0;
            if (opacity >= 1f) a = 255;
            else if (opacity <= 0f) a = 0;
            else a = (int)(opacity * 255);
            return (uint)Color.FromArgb(a, color.R, color.G, color.B).ToArgb();
        }

        public static void AddText(string text, VectorF2D position, Color color = default, int fontSize = 12, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                String = new DrawableString { Position = position, Text = text },
                StrokeColor = Convert2Argb(color, opacity),
                FontSize = fontSize,
                Type = DrawableType.String
            });
        }

        public static void AddCircle(Circle circle, Color color = default, bool fill = false, float strokeWidth = 0.01f, float opacity = 1f)
        {
            uint c = Convert2Argb(color, opacity);
            AddObject(new DrawableObject
            {
                Circle = circle,
                StrokeColor = c,
                FillColor = fill ? c : null,
                StrokeWidth = strokeWidth,
                Type = DrawableType.Circle
            });
        }

        public static void AddCircle(VectorF2D center, float radius, Color color = default, bool fill = false, float strokeWidth = 0.01f, float opacity = 1f)
        {
            AddCircle(new Circle(center, radius), color, fill, strokeWidth, opacity);
        }

        public static void AddLine(Line line, Color strokeColor = default, float strokeWidth = 0.01f, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Line = line,
                StrokeColor = Convert2Argb(strokeColor, opacity),
                StrokeWidth = strokeWidth,
                Type = DrawableType.Line
            });
        }

        public static void AddLine(VectorF2D p1, VectorF2D p2, Color strokeColor = default, float strokeWidth = 0.01f, float opacity = 1f)
        {
            AddLine(new Line(p1, p2), strokeColor, strokeWidth, opacity);
        }

        public static void AddRegion(List<VectorF2D> points, Color color = default, float strokeWidth = 0.01f, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Region = points,
                StrokeColor = Convert2Argb(color, opacity),
                StrokeWidth = strokeWidth,
                Type = DrawableType.Region
            });
        }

        public static void AddPath(List<SingleObjectState> points, Color color = default, float strokeWidth = 0.01f, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Path = points.Select(s => s.Location).ToList(),
                StrokeColor = Convert2Argb(color, opacity),
                StrokeWidth = strokeWidth,
                Type = DrawableType.Path
            });
        }

        public static void Serialize(MemoryStream memoryStream, PrefixStyle style, int fieldNumber)
        {
            lock (_lock)
            {
                foreach (var item in Objects)
                    Serializer.SerializeWithLengthPrefix<DrawableObject>(memoryStream, item, style, fieldNumber);

                Objects.Clear();
            }
        }
        public static void Clear()
        {
            lock (_lock)
            {
                Objects.Clear();
            }
        }
    }
}