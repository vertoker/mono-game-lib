using RenderHierarchyLib.Models;
using System;

namespace RenderHierarchyLib.Extensions
{
    public static class Easings
    {
        private const float c1 = 1.70158f;
        private const float c2 = c1 * 1.525f;
        private const float c3 = c1 + 1f;
        private const float c4 = 2 * MathF.PI / 3;
        private const float c5 = 2 * MathF.PI / 4.5f;

        public static float Get(float x, EasingType easing)
        {
            switch (easing)
            {
                case EasingType.Linear:
                    return x;
                case EasingType.Constant:
                    return MathF.Floor(x);

                case EasingType.InSine:
                    return 1 - MathF.Cos((x * MathF.PI) / 2);
                case EasingType.OutSine:
                    return MathF.Sin((x * MathF.PI) / 2);
                case EasingType.InOutSine:
                    return -(MathF.Cos(MathF.PI * x) - 1) / 2;

                case EasingType.InQuad:
                    return x * x;
                case EasingType.OutQuad:
                    return 1 - MathF.Pow(1 - x, 2);
                case EasingType.InOutQuad:
                    return x < 0.5f ? 2 * x * x : 1 - MathF.Pow(-2 * x + 2, 2) / 2;

                case EasingType.InCubic:
                    return x * x * x;
                case EasingType.OutCubic:
                    return 1 - MathF.Pow(1 - x, 3);
                case EasingType.InOutCubic:
                    return x < 0.5f ? 4 * x * x * x : 1 - MathF.Pow(-2 * x + 2, 3) / 2;

                case EasingType.InQuart:
                    return x * x * x * x;
                case EasingType.OutQuart:
                    return 1 - MathF.Pow(1 - x, 4);
                case EasingType.InOutQuart:
                    return x < 0.5f ? 8 * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 4) / 2;

                case EasingType.InQuint:
                    return x * x * x * x * x;
                case EasingType.OutQuint:
                    return 1 - MathF.Pow(1 - x, 5);
                case EasingType.InOutQuint:
                    return x < 0.5f ? 16 * x * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 5) / 2;

                case EasingType.InExpo:
                    return x == 0 ? 0 : MathF.Pow(2, 10 * x - 10);
                case EasingType.OutExpo:
                    return x == 1 ? 1 : 1 - MathF.Pow(2, -10 * x);
                case EasingType.InOutExpo:
                    return x == 0 ? 0 : x == 1 ? 1 : x < 0.5f
                    ? MathF.Pow(2, 20 * x - 10) / 2 : (2 - MathF.Pow(2, -20 * x + 10)) / 2;

                case EasingType.InCirc:
                    return 1 - MathF.Sqrt(1 - MathF.Pow(x, 2));
                case EasingType.OutCirc:
                    return MathF.Sqrt(1 - MathF.Pow(x - 1, 2));
                case EasingType.InOutCirc:
                    return x < 0.5f ? (1 - MathF.Sqrt(1 - MathF.Pow(2 * x, 2))) / 2
                    : (MathF.Sqrt(1 - MathF.Pow(-2 * x + 2, 2)) + 1) / 2;

                case EasingType.InBack:
                    return c3 * x * x * x - c1 * x * x;
                case EasingType.OutBack:
                    return 1 + c3 * MathF.Pow(x - 1, 3) + c1 * MathF.Pow(x - 1, 2);
                case EasingType.InOutBack:
                    return x < 0.5f ? (MathF.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
                        : (MathF.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;

                case EasingType.InElastic:
                    return x == 0 ? 0 : x == 1 ? 1 : -MathF.Pow(2, 10 * x - 10) * MathF.Sin((x * 10 - 10.75f) * c4);
                case EasingType.OutElastic:
                    return x == 0 ? 0 : x == 1 ? 1 : MathF.Pow(2, -10 * x) * MathF.Sin((x * 10 - 0.75f) * c4) + 1;
                case EasingType.InOutElastic:
                    return x == 0 ? 0 : x == 1 ? 1 : x < 0.5f
                    ? -(MathF.Pow(2, 20 * x - 10) * MathF.Sin((20 * x - 11.125f) * c5)) / 2
                        : (MathF.Pow(2, -20 * x + 10) * MathF.Sin((20 * x - 11.125f) * c5)) / 2 + 1;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}