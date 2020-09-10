using Raylib_cs;
using System;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public struct Input
    {
        public static void  Start()
        {
            hInput1 = 0;
            vInput1 = 0;
        }

        private static float hInput1 = 0;
        private static float vInput1 = 0;
        public static float GetAxis(string axis, float sensitivity = 3)
        {
            float dead = .001f;
            float target = 0;

            switch (axis)
            {
                case "Vertical":
                    if (IsKeyDown(KeyboardKey.KEY_W))
                        target = -1;
                    if (IsKeyDown(KeyboardKey.KEY_S))
                        target = 1;

                    vInput1 = Utils.Lerp(vInput1, target, sensitivity * GetFrameTime());
                    return (MathF.Abs(vInput1) < dead) ? 0f : vInput1;
                case "Horizontal":
                    if (IsKeyDown(KeyboardKey.KEY_D))
                        target = 1;
                    if (IsKeyDown(KeyboardKey.KEY_A))
                        target = -1;

                    hInput1 = Utils.Lerp(hInput1, target, sensitivity * GetFrameTime());
                    return (MathF.Abs(hInput1) < dead) ? 0f : hInput1;
                default:
                    return 0;
            }
        }

        public static bool GetButtonDown(string button)
        {
            switch (button)
            {
                case "Attack":
                    if (IsKeyPressed(KeyboardKey.KEY_SPACE))
                        return true;
                    return false;
                default:
                    return false;
            }
        }
    }
}
