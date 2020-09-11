using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    struct CollisionManager
    {
        #region Colliding
        public static bool Colliding(Sprite spriteOne, Sprite spriteTwo)
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
        public static bool Colliding(AnimatedObject spriteOne, Rectangle spriteTwo)
        {
            return CheckCollisionCircleRec(spriteOne.position, spriteOne.radius, spriteTwo);
        }
        public static bool Colliding(Sprite spriteOne, Rectangle spriteTwo)
        {
            return CheckCollisionCircleRec(spriteOne.position, spriteOne.radius, spriteTwo);
        }
        public static bool Colliding(Rectangle spriteOne, Rectangle spriteTwo)
        {
            return CheckCollisionRecs(spriteOne, spriteTwo);
        }
        public static bool Colliding(AnimatedObject spriteOne, LineSprite spriteTwo)
        {
            float u = (spriteOne.position.X - spriteTwo.StartPos.X) * (spriteTwo.EndPos.X - spriteTwo.StartPos.X); 
            u += (spriteOne.position.Y - spriteTwo.StartPos.Y) * (spriteTwo.EndPos.Y - spriteTwo.StartPos.Y);
            u /= MathF.Pow((spriteTwo.EndPos - spriteTwo.StartPos).Length(), 2);
            u = MathF.Max(u, 0);
            u = MathF.Min(u, 1);
            Vector2 P = spriteTwo.StartPos + (u * (spriteTwo.EndPos - spriteTwo.StartPos));

            return Vector2.Distance(P, spriteOne.position) < spriteOne.radius + spriteTwo.thickness;
        }
        //public static bool Colliding(AnimatedObject spriteOne, LineSprite spriteTwo)
        //{
        //    float u = (spriteOne.position.X - spriteTwo.StartPos.X) * (spriteTwo.EndPos.X - spriteTwo.StartPos.X);
        //    u += (spriteOne.position.Y - spriteTwo.StartPos.Y) * (spriteTwo.EndPos.Y - spriteTwo.StartPos.Y);
        //    u /= MathF.Pow((spriteTwo.EndPos - spriteTwo.StartPos).Length(), 2);
        //    Vector2 P = spriteTwo.StartPos + (u * (spriteTwo.EndPos - spriteTwo.StartPos));

        //    float buffer = spriteOne.radius + spriteTwo.thickness;

        //    bool toReturn = (Vector2.Distance(P, spriteTwo.EndPos) + buffer < spriteTwo.Length);
        //    toReturn &= (Vector2.Distance(P, spriteTwo.StartPos) + buffer < spriteTwo.Length);
        //    toReturn &= Vector2.Distance(P, spriteOne.position) < buffer;

        //    return toReturn;
        //}
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

        public static bool OutOfBounds(Vector2 position)
        {
            if (position.X < Window.playZoneBarrier.X)
            {
                return true;
            }
            if (position.X > Window.playZoneBarrier.Z)
            {
                return true;
            }
            if (position.Y < Window.playZoneBarrier.Y)
            {
                return true;
            }
            if (position.Y > Window.playZoneBarrier.W)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
