
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.Drawings
{
    public class DrawingPacket
    {
        private static object _lock = new object();

        public static List<DrawableObject> Objects { get; set; }

        private static void AddObject(DrawableObject obj)
        {
            lock (_lock)
            {
                if (GameConfig.Default.Debug)
                    Objects.Add(obj);
            }
        }

        public static void AddObject(string text, Color color, float fontSize = 12f, float opacity = 1f)
        {
            AddObject(new DrawableObject { Text = text, FillColor = color.ToArgb(), FontSize = fontSize, Opacity = opacity });
        }

        public static void AddObject(Circle circle, Color strokeColor, float strokeWidth = 0.1f, bool isFilled = false, Color fillColor = new Color(), float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Circle = circle,
                FillColor = fillColor.ToArgb(),
                Fill = isFilled,
                StrokeColor = strokeColor.ToArgb(),
                StrokeWidth = strokeWidth,
                Opacity = opacity
            });
        }

        public static void AddObject(VectorF2D center, float radius, Color strokeColor, float strokeWidth = 0.1f, bool isFilled = false, Color fillColor = new Color(), float opacity = 1f)
        {
            AddObject(new Circle(center, radius), strokeColor, strokeWidth, isFilled, fillColor, opacity);
        }

        public static void AddObject(Line line, Color strokeColor, float strokeWidth = 0.1f, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Line = line,
                StrokeColor = strokeColor.ToArgb(),
                StrokeWidth = strokeWidth,
                Opacity = opacity
            });
        }

        public static void AddObject(VectorF2D p1, VectorF2D p2, Color strokeColor, float strokeWidth = 0.1f, float opacity = 1f)
        {
            AddObject(new Line(p1, p2), strokeColor, strokeWidth, opacity);
        }

        public static void AddObject(VectorF2D position, float width, float height, Color strokeColor, float strokeWidth = 0.1f, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Region = new List<Vector2D<float>> { position, new VectorF2D(position.X + width, position.Y), new VectorF2D(position.X, position.Y - height) },
                StrokeColor = strokeColor.ToArgb(),
                StrokeWidth = strokeWidth,
                Opacity = opacity
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
    }
}