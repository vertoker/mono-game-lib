namespace RenderHierarchyLib.Models
{
    public enum EasingType
    {
        Linear = 0,      Constant = 1,                        // default
        InSine = 2,      OutSine = 3,      InOutSine = 4,     // sin
        InQuad = 5,      OutQuad = 6,      InOutQuad = 7,     // 2-power functions
        InCubic = 8,     OutCubic = 9,     InOutCubic = 10,   // 3-power functions
        InQuart = 11,    OutQuart = 12,    InOutQuart = 13,   // 4-power functions
        InQuint = 14,    OutQuint = 15,    InOutQuint = 16,   // 5-power functions
        InExpo = 17,     OutExpo = 18,     InOutExpo = 19,    // exponential functions
        InCirc = 20,     OutCirc = 21,     InOutCirc = 22,    // circle functions
        InBack = 23,     OutBack = 24,     InOutBack = 25,    // inertial functions
        InElastic = 26,  OutElastic = 27,  InOutElastic = 28  // elastic functions
    }
}