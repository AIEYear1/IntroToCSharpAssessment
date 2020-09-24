using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Manages collisions with simplified methods
    /// </summary>
    static class CollisionManager
    {
        #region Colliding
        /// <summary>
        /// checks to see if two Sprites are colliding
        /// </summary>
        public static bool Colliding(Sprite spriteOne, Sprite spriteTwo)
        {
            return CheckCollisionCircles(spriteOne.position, spriteOne.radius, spriteTwo.position, spriteTwo.radius);
        }
        /// <summary>
        /// checks to see if a Sprite and an AnimatedObject are colliding
        /// </summary>
        public static bool Colliding(Sprite spriteOne, AnimatedObject spriteTwo)
        {
            return CheckCollisionCircles(spriteOne.position, spriteOne.radius, spriteTwo.Position, spriteTwo.radius);
        }
        /// <summary>
        /// checks to see if two AnimatedObjects are colliding
        /// </summary>
        public static bool Colliding(AnimatedObject spriteOne, AnimatedObject spriteTwo)
        {
            return CheckCollisionCircles(spriteOne.Position, spriteOne.radius, spriteTwo.Position, spriteTwo.radius);
        }
        /// <summary>
        /// checks to see if an AnimatedObject and a Rectangle are colliding
        /// </summary>
        public static bool Colliding(AnimatedObject spriteOne, Rectangle spriteTwo)
        {
            return CheckCollisionCircleRec(spriteOne.Position, spriteOne.radius, spriteTwo);
        }
        /// <summary>
        /// checks to see if a Sprite and a Rectangle are colliding
        /// </summary>
        public static bool Colliding(Sprite spriteOne, Rectangle spriteTwo)
        {
            return CheckCollisionCircleRec(spriteOne.position, spriteOne.radius, spriteTwo);
        }
        /// <summary>
        /// checks to see if two Rectangles are colliding
        /// </summary>
        public static bool Colliding(Rectangle spriteOne, Rectangle spriteTwo)
        {
            return CheckCollisionRecs(spriteOne, spriteTwo);
        }
        /// <summary>
        /// checks to see if an AnimatedObject and a LineSprite are colliding
        /// </summary>
        public static bool Colliding(AnimatedObject spriteOne, LineSprite spriteTwo)
        {
            float u = (spriteOne.Position.X - spriteTwo.StartPos.X) * (spriteTwo.EndPos.X - spriteTwo.StartPos.X);
            u += (spriteOne.Position.Y - spriteTwo.StartPos.Y) * (spriteTwo.EndPos.Y - spriteTwo.StartPos.Y);
            u /= MathF.Pow((spriteTwo.EndPos - spriteTwo.StartPos).Length(), 2);
            u = MathF.Max(u, 0);
            u = MathF.Min(u, 1);
            Vector2 P = spriteTwo.StartPos + (u * (spriteTwo.EndPos - spriteTwo.StartPos));

            return Vector2.Distance(P, spriteOne.Position) < spriteOne.radius + spriteTwo.thickness;
        }
        #endregion

        #region Push
        /// <summary>
        /// Moves objeBePushed back until it isn't colliding with objPushing
        /// </summary>
        public static void Push(AnimatedObject objPushing, AnimatedObject objBeingPushed)
        {
            if (Colliding(objPushing, objBeingPushed))
            {
                Vector2 push = Utils.ClampMagnitude(objBeingPushed.Position - objPushing.Position, 1);
                push *= MathF.Abs(Vector2.Distance(objBeingPushed.Position, objPushing.Position) - (objBeingPushed.radius + objPushing.radius));

                objBeingPushed.Position += push;
            }
        }
        /// <summary>
        /// Moves objeBePushed back until it isn't colliding with objPushing
        /// </summary>
        public static void Push(Sprite objPushing, AnimatedObject objBeingPushed)
        {
            if (Colliding(objPushing, objBeingPushed))
            {
                Vector2 push = Utils.ClampMagnitude(objBeingPushed.Position - objPushing.position, 1);
                push *= MathF.Abs(Vector2.Distance(objBeingPushed.Position, objPushing.position) - (objBeingPushed.radius + objPushing.radius));

                objBeingPushed.Position += push;
            }
        }
        /// <summary>
        /// Moves objeBePushed back until it isn't colliding with objPushing
        /// </summary>
        public static void Push(AnimatedObject objPushing, Sprite objBeingPushed)
        {
            if (Colliding(objBeingPushed, objPushing))
            {
                Vector2 push = Utils.ClampMagnitude(objBeingPushed.position - objPushing.Position, 1);
                push *= MathF.Abs(Vector2.Distance(objBeingPushed.position, objPushing.Position) - (objBeingPushed.radius + objPushing.radius));

                objBeingPushed.position += push;
            }
        }
        /// <summary>
        /// Moves objeBePushed back until it isn't colliding with objPushing
        /// </summary>
        public static void Push(Sprite objPushing, Sprite objBeingPushed)
        {
            if (Colliding(objPushing, objBeingPushed))
            {
                Vector2 push = Utils.ClampMagnitude(objBeingPushed.position - objPushing.position, 1);
                push *= MathF.Abs(Vector2.Distance(objBeingPushed.position, objPushing.position) - (objBeingPushed.radius + objPushing.radius));

                objBeingPushed.position += push;
            }
        }
        #endregion
    }
}
