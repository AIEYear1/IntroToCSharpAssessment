using CRPGNamespace;
using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Player character for window attacks
    /// </summary>
    public class Character : AnimatedObject
    {
        /// <summary>
        /// speed of the character
        /// </summary>
        public float speed = 300;
        /// <summary>
        /// Player velocity
        /// </summary>
        public Vector2 velocity;
        /// <summary>
        /// Player's direction of travel
        /// </summary>
        public Vector2 direction = new Vector2();

        /// <summary>
        /// How easily the player moves
        /// </summary>
        public float sensitivity = 5;

        /// <summary>
        /// Creture it holds either Player or Monster
        /// </summary>
        public LivingCreature creature;

        /// <summary>
        /// Identical to animatedObject
        /// </summary>
        /// <param name="image">Spritesheet to go off of</param>
        /// <param name="position">Start position of the AI</param>
        /// <param name="color">Color overlay</param>
        /// <param name="tileSize">Pixel size of both the x and y of the tile</param>
        /// <param name="frames">The total number of collumns (X) and Rows (Y)</param>
        /// <param name="radius">Radius of the animated object</param>
        /// <param name="framesSpeed">how fast the animation occurs</param>
        public Character(Texture2D image, Vector2 position, Color color, int tileSize, Vector2 frames, int radius, float framesSpeed = 8) : base(image, position, color, tileSize, frames, radius, framesSpeed)
        {
            SetState(3);
        }

        /// <summary>
        /// Initializes Character on showing window
        /// </summary>
        public virtual void Start()
        {
            direction = Vector2.Zero;
            velocity = Vector2.Zero;
        }

        /// <summary>
        /// Update character for movement
        /// </summary>
        public virtual void Update()
        {
            direction.X = Input.GetAxis("Horizontal", sensitivity);
            direction.Y = Input.GetAxis("Vertical", sensitivity);

            direction = Utils.ClampMagnitude(direction, 1);

            velocity = direction * speed * GetFrameTime();
            Position += velocity;
            Border();

            SetPlayerAnimState();
        }

        /// <summary>
        /// Set Character Movement animation set
        /// </summary>
        public void SetPlayerAnimState()
        {
            if (direction == Vector2.Zero)
            {
                animate = false;
                return;
            }
            animate = true;

            if (MathF.Abs(direction.X) > MathF.Abs(direction.Y))
            {
                if (direction.X > 0)
                {
                    SetState(2);
                    return;
                }
                if (direction.X < 0)
                {
                    SetState(1);
                    return;
                }
            }

            if (direction.Y < 0)
            {
                SetState(3);
                return;
            }
            if (direction.Y > 0)
            {
                SetState(0);
            }
        }

        /// <summary>
        /// Blocks characters from leaving the screen view
        /// </summary>
        public void Border()
        {
            Vector2 tmpPos = Position;

            if (tmpPos.X < Window.playZoneBarrier.X + radius)
            {

                tmpPos.X = Window.playZoneBarrier.X + radius;
            }
            else if (tmpPos.X > Window.playZoneBarrier.Z - radius)
            {
                tmpPos.X = Window.playZoneBarrier.Z - radius;
            }

            if (tmpPos.Y < Window.playZoneBarrier.Y + radius)
            {
                tmpPos.Y = Window.playZoneBarrier.Y + radius;
            }
            else if (tmpPos.Y > Window.playZoneBarrier.W - radius)
            {
                tmpPos.Y = Window.playZoneBarrier.W - radius;
            }

            Position = tmpPos;
        }
    }
}
