using Raylib_cs;
using System;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public class Input
    {
        /// <summary>
        /// Resets the floats to limit launching the player at the start
        /// </summary>
        public static void  Start()
        {
            toReturnHorizontal = 0;
            toReturnVertical = 0;
        }
        /// <summary>
        /// To Return Values for GetAxis()
        /// </summary>
        private static float toReturnHorizontal = 0, toReturnVertical = 0;
        /// <summary>
        /// Gets input axes for player movement
        /// </summary>
        /// <param name="axis">input axis to get</param>
        /// <param name="sensitivity">how easily the player moves up to speed</param>
        /// <returns>Returns a float between 1 and -1 for the specified axis</returns>
        public static float GetAxis(string axis, float sensitivity = 3)
        {
            //How small the number has to be to not be registered
            float dead = .001f;
            //what the axis will try and reach
            float target = 0;

            switch (axis)
            {
                case "Vertical":
                    if (IsKeyDown(KeyboardKey.KEY_W))
                        target = -1;
                    if (IsKeyDown(KeyboardKey.KEY_S))
                        target = 1;

                    toReturnVertical = Utils.Lerp(toReturnVertical, target, sensitivity * GetFrameTime());
                    return (MathF.Abs(toReturnVertical) < dead) ? 0f : toReturnVertical;
                case "Horizontal":
                    if (IsKeyDown(KeyboardKey.KEY_D))
                        target = 1;
                    if (IsKeyDown(KeyboardKey.KEY_A))
                        target = -1;

                    toReturnHorizontal = Utils.Lerp(toReturnHorizontal, target, sensitivity * GetFrameTime());
                    return (MathF.Abs(toReturnHorizontal) < dead) ? 0f : toReturnHorizontal;
                default:
                    return 0;
            }
        }
    }
}
