using CRPGNamespace;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Player character for window attacks
    /// </summary>
    public class Character : AnimatedObject
    {
        /// <summary>
        /// Creture it holds either Player or Monster
        /// </summary>
        public LivingCreature creature;
        /// <summary>
        /// Player's direction of travel
        /// </summary>
        public Vector2 direction = new Vector2();
        /// <summary>
        /// speed of the character
        /// </summary>
        public float speed = 300;
        /// <summary>
        /// How easily the player moves
        /// </summary>
        public float sensitivity = 5;

        // Holds all the PopUps for this creature
        protected List<PopUpText> popUps = new List<PopUpText>();

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
            popUps.Clear();
            direction = Vector2.Zero;
        }

        /// <summary>
        /// Update character for movement
        /// </summary>
        public virtual void Update()
        {
            for (int x = 0; x < popUps.Count; x++)
            {
                popUps[x].Update();
            }

            direction.X = Input.GetAxis("Horizontal", sensitivity);
            direction.Y = Input.GetAxis("Vertical", sensitivity);

            direction = Utils.ClampMagnitude(direction, 1);

            Vector2 velocity = direction * speed * GetFrameTime();
            Position += velocity;
            Border();

            SetPlayerAnimState();
        }

        public override void Draw()
        {
            base.Draw();
            for (int x = 0; x < popUps.Count; x++)
            {
                popUps[x].Draw();
            }
        }

        /// <summary>
        /// Adds text popup to be shown
        /// </summary>
        /// <param name="text">Text for popup to show</param>
        public void PopUp(string text, int size)
        {
            //Ensure the min font size is 20
            size = (int)MathF.Max(20, size);

            popUps.Add(new PopUpText(text, Position + (-Vector2.UnitY * radius), size, RED, 150, Utils.RotationMatrix(-Vector2.UnitY, Utils.DegToRad(Utils.NumberBetween(-50, 50)), 1), 1));
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
