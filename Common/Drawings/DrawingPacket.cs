
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using MRL.SSL.Common.Configuration;
using MRL.SSL.Common.Math;
using ProtoBuf;

namespace MRL.SSL.Common.Drawings
{
    public static class DrawingPacket
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

        private static string Convert2HexRGB(Color color)
        {
            int intArgb = color.ToArgb();
            byte[] argbBytes = new byte[4];
            for (int i = 0; i < argbBytes.Length; i++)
                argbBytes[i] = Convert.ToByte((intArgb >> (8 * (argbBytes.Length - 1 - i))) & 0xff);
            return Convert.ToHexString(argbBytes[1..]);
        }

        public static void AddObject(string text, VectorF2D position, Color color = new Color(), float fontSize = 12f, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                String = new DrawableString { Position = position, Text = text },
                FillColor = Convert2HexRGB(color),
                FontSize = fontSize,
                Opacity = opacity,
                Type = DrawableType.String
            });
        }

        public static void AddObject(Circle circle, Color strokeColor = new Color(), float strokeWidth = 0.01f, bool isFilled = false, Color fillColor = new Color(), float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Circle = circle,
                FillColor = Convert2HexRGB(fillColor),
                Fill = isFilled,
                StrokeColor = Convert2HexRGB(strokeColor),
                StrokeWidth = strokeWidth,
                Opacity = opacity,
                Type = DrawableType.Circle
            });
        }

        public static void AddObject(VectorF2D center, float radius, Color strokeColor = new Color(), float strokeWidth = 0.01f, bool isFilled = false, Color fillColor = new Color(), float opacity = 1f)
        {
            AddObject(new Circle(center, radius), strokeColor, strokeWidth, isFilled, fillColor, opacity);
        }

        public static void AddObject(Line line, Color strokeColor, float strokeWidth = 0.01f, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Line = line,
                StrokeColor = Convert2HexRGB(strokeColor),
                StrokeWidth = strokeWidth,
                Opacity = opacity,
                Type = DrawableType.Line
            });
        }

        public static void AddObject(VectorF2D p1, VectorF2D p2, Color strokeColor = new Color(), float strokeWidth = 0.01f, float opacity = 1f)
        {
            AddObject(new Line(p1, p2), strokeColor, strokeWidth, opacity);
        }

        public static void AddObject(List<VectorF2D> points, Color strokeColor = new Color(), float strokeWidth = 0.01f, float opacity = 1f)
        {
            AddObject(new DrawableObject
            {
                Region = points,
                StrokeColor = Convert2HexRGB(strokeColor),
                StrokeWidth = strokeWidth,
                Opacity = opacity,
                Type = DrawableType.Region
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