using Raylib_cs;
using System;
using System.Numerics;
using static RaylibWindowNamespace.Objects;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public class AI : Character
    {
        public AI(Texture2D image, Vector2 position, Color color, int tileSize, Vector2 frames, int radius) : base(image, position, color, tileSize, frames, radius)
        {
            speed = 250;
            sensitivity = 1.5f;
        }

        public AI()
        {
            speed = 250;
            sensitivity = 1.5f;
        }

        public override void Update()
        {
            velocity = direction * (speed * SpeedMod) * GetFrameTime();
            position += velocity;
            Border();

            SetPlayerAnimState();
        }

        float vertDir = 0;
        float horDir = 0;
        public void SetDirection(Vector2 directionToTravel)
        {
            if (directionToTravel.X > 0)
                directionToTravel.X = 1;
            else if (directionToTravel.X < 0)
                directionToTravel.X = -1;
            if (directionToTravel.Y > 0)
                directionToTravel.Y = 1;
            else if (directionToTravel.Y < 0)
                directionToTravel.Y = -1;

            Vector2 directionToReturn = new Vector2();
            float dead = .001f;

            horDir = Utils.Lerp(horDir, directionToTravel.X, sensitivity * GetFrameTime());
            vertDir = Utils.Lerp(vertDir, directionToTravel.Y, sensitivity * GetFrameTime());

            directionToReturn.X = (MathF.Abs(horDir) < dead) ? 0f : horDir;
            directionToReturn.Y = (MathF.Abs(vertDir) < dead) ? 0f : vertDir;

            direction = directionToReturn;
        }
    }
}
