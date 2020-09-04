using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace raygamecsharp
{
    struct CollisionManager
    {
        #region Colliding
        public static bool Colliding(Sprite spriteOne, Sprite spriteTwo)
        {
            return CheckCollisionCircles(spriteOne.position, spriteOne.radius, spriteTwo.position, spriteTwo.radius);
        }
        public static bool Colliding(AnimatedObject spriteOne, Sprite spriteTwo)
        {
            return CheckCollisionCircles(spriteOne.position, spriteOne.radius, spriteTwo.position, spriteTwo.radius);
        }
        public static bool Colliding(Sprite spriteOne, AnimatedObject spriteTwo)
        {
            return CheckCollisionCircles(spriteOne.position, spriteOne.radius, spriteTwo.position, spriteTwo.radius);
        }
        public static bool Colliding(AnimatedObject spriteOne, AnimatedObject spriteTwo)
        {
            return CheckCollisionCircles(spriteOne.position, spriteOne.radius, spriteTwo.position, spriteTwo.radius);
        }
        #endregion

        #region Push
        public static void Push(AnimatedObject objPushing, AnimatedObject objBeingPushed)
        {
            if (Vector2.Distance(objBeingPushed.position, objPushing.position) < objBeingPushed.radius + objPushing.radius)
            {
                Vector2 push = Utils.ClampMagnitude(objBeingPushed.position - objPushing.position, 1);
                push *= MathF.Abs(Vector2.Distance(objBeingPushed.position, objPushing.position) - (objBeingPushed.radius + objPushing.radius));

                objBeingPushed.position += push;
            }
        }
        public static void Push(Sprite objPushing, AnimatedObject objBeingPushed)
        {
            if (Vector2.Distance(objBeingPushed.position, objPushing.position) < objBeingPushed.radius + objPushing.radius)
            {
                Vector2 push = Utils.ClampMagnitude(objBeingPushed.position - objPushing.position, 1);
                push *= MathF.Abs(Vector2.Distance(objBeingPushed.position, objPushing.position) - (objBeingPushed.radius + objPushing.radius));

                objBeingPushed.position += push;
            }
        }
        public static void Push(AnimatedObject objPushing, Sprite objBeingPushed)
        {
            if (Vector2.Distance(objBeingPushed.position, objPushing.position) < objBeingPushed.radius + objPushing.radius)
            {
                Vector2 push = Utils.ClampMagnitude(objBeingPushed.position - objPushing.position, 1);
                push *= MathF.Abs(Vector2.Distance(objBeingPushed.position, objPushing.position) - (objBeingPushed.radius + objPushing.radius));

                objBeingPushed.position += push;
            }
        }
        public static void Push(Sprite objPushing, Sprite objBeingPushed)
        {
            if (Vector2.Distance(objBeingPushed.position, objPushing.position) < objBeingPushed.radius + objPushing.radius)
            {
                Vector2 push = Utils.ClampMagnitude(objBeingPushed.position - objPushing.position, 1);
                push *= MathF.Abs(Vector2.Distance(objBeingPushed.position, objPushing.position) - (objBeingPushed.radius + objPushing.radius));

                objBeingPushed.position += push;
            }
        }
        #endregion

        public static bool BoxPointCollision(Rectangle box, Vector2 point)
        {
            if (point.X > box.x && point.X < (box.x + box.width) && point.Y > box.y && point.Y < (box.y + box.height))
                return true;

            return false;
        }
    }
}
