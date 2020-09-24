using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// AI for Combat
    /// </summary>
    public class AI : Character
    {
        /// <summary>
        /// Identical to Character Constructor
        /// </summary>
        /// <param name="image">Spritesheet to go off of</param>
        /// <param name="position">Start position of the AI</param>
        /// <param name="color">Color overlay</param>
        /// <param name="tileSize">Pixel size of both the x and y of the tile</param>
        /// <param name="frames">The total number of collumns (X) and Rows (Y)</param>
        /// <param name="radius">Radius of the animated object</param>
        /// <param name="framesSpeed">how fast the animation occurs</param>
        public AI(Texture2D image, Vector2 position, Color color, int tileSize, Vector2 frames, int radius, float framesSpeed = 8) : base(image, position, color, tileSize, frames, radius, framesSpeed)
        {

        }

        /// <summary>
        /// Reset movement values to avoid erroneous initial movement
        /// </summary>
        public override void Start()
        {
            vertDir = 0;
            horDir = 0;
        }

        /// <summary>
        /// Make the AI move
        /// </summary>
        public override void Update()
        {
            Vector2 velocity = direction * speed * GetFrameTime();
            Position += velocity;
            Border();

            SetPlayerAnimState();
        }

        //Holds the actual vertical and horizontal values so they get added to everytime instead of overwritten
        float vertDir = 0, horDir = 0;
        /// <summary>
        /// Smoothly sets the direction the Ai will travel in
        /// </summary>
        /// <param name="directionToTravel">the direction in which the Ai will travel towards</param>
        public void SetDirection(Vector2 directionToTravel)
        {
            //simplifies the Vector2 to a simple eight-directional direction
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
