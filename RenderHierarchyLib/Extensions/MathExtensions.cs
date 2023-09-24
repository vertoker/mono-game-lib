using Microsoft.Xna.Framework;
using System;

namespace RenderHierarchyLib.Extensions
{
    public static class MathExtensions
    {
        public const float Deg2Rad = MathF.PI * 2F / 360F;
        public const float Rad2Deg = 1F / Deg2Rad;

        public static Vector2 RotateVector(this Vector2 vector, float angle)
        {
            var rad = angle * Deg2Rad;
            var sin = MathF.Sin(rad);
            var cos = MathF.Cos(rad);
            return vector.RotateVector(sin, cos);
        }
        public static Vector2 RotateVector(this Vector2 vector, Vector2 parent, float angle)
        {
            var rad = angle * Deg2Rad;
            var sin = MathF.Sin(rad);
            var cos = MathF.Cos(rad);
            return vector.RotateVector(parent, sin, cos);
        }

        public static Vector2 RotateVector(this Vector2 vector, float sin, float cos)
        {
            return new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
        }
        public static Vector2 RotateVector(this Vector2 vector, Vector2 parent, float sin, float cos)
        {
            return new Vector2(parent.X + vector.X * cos - vector.Y * sin, parent.Y + vector.X * sin + vector.Y * cos);
        }

        public static Vector2 RotateVector(float x, float y, float sin, float cos)
        {
            return new Vector2(x * cos - y * sin, x * sin + y * cos);
        }
        public static Vector2 RotateVector(float x, float y, Vector2 parent, float sin, float cos)
        {
            return new Vector2(parent.X + x * cos - y * sin, parent.Y + x * sin + y * cos);
        }

        public static void GetBordersRectangleByPivot(this Vector2 pixelSize, Vector2 pivot, out Vector2 TL, out Vector2 BR)
        {
            var center = pixelSize / 2f;
            TL = new(-center.X - pivot.X * center.X, -center.Y + pivot.Y * center.Y);
            BR = new(center.X - pivot.X * center.X, center.Y + pivot.Y * center.Y);
        }
    }
}
