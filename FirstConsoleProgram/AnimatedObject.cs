using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// A sprite with an animation
    /// </summary>
    public class AnimatedObject : ImageObject
    {
        /// <summary>
        /// Manages how the object animates
        /// </summary>
        public bool animate = true, animReset = false;
        /// <summary>
        /// Radius of the animated object
        /// </summary>
        public int radius = 0;

        /// <summary>
        /// The frame of the spritesheet to show
        /// </summary>
        Rectangle frameRec;
        /// <summary>
        /// The rectangle of the main actual sprite
        /// </summary>
        Rectangle mainRec;
        /// <summary>
        /// The total number of collumns (X) and Rows (Y)
        /// </summary>
        Vector2 frames;
        /// <summary>
        /// Where everything works from
        /// </summary>
        Vector2 origin;
        /// <summary>
        /// The time in seconds between every frame
        /// </summary>
        Timer timeBetweenFrames;
        /// <summary>
        /// Stores the current frame of the animation
        /// </summary>
        Timer curFrame;

        public new Vector2 position
        {
            get => new Vector2(mainRec.x, mainRec.y);
            set
            {
                mainRec.x = value.X;
                mainRec.y = value.Y;
            }
        }

        /// Parameters
        /// <param name="image">Spritesheet to go off of</param>
        /// <param name="position">Start position of the AI</param>
        /// <param name="color">Color overlay</param>
        /// <param name="tileSize">Pixel size of both the x and y of the tile</param>
        /// <param name="frames">The total number of collumns (X) and Rows (Y)</param>
        /// <param name="radius">Radius of the animated object</param>
        /// <param name="framesSpeed">how fast the animation occurs</param>
        public AnimatedObject(Texture2D image, Vector2 position, Color color, int tileSize, Vector2 frames, int radius, float framesSpeed = 8) : base(image, position, color)
        {
            this.radius = radius;
            float scalar = (float)(radius * 2) / (float)tileSize;

            frameRec = new Rectangle(0, 0, tileSize, tileSize);
            mainRec = new Rectangle(position.X, position.Y, tileSize * scalar, tileSize * scalar);
            origin = new Vector2(mainRec.width / 2, mainRec.height / 2);

            this.frames = frames;

            timeBetweenFrames = new Timer(60 / framesSpeed);
            curFrame = new Timer(this.frames.X);
        }

        /// <summary>
        /// Draw the texture
        /// </summary>
        public override void Draw()
        {
            if (animate)
            {
                Animate();
            }
            else if (!animReset)
            {
                ResetAnim();
            }

            //Spritesheet to pull from, frame of spritesheet to reference, actual sprite dimensions, where everything draws rotates and scales from, rotation set to 0, color to overlay the sprite
            DrawTexturePro(texture, frameRec, mainRec, origin, 0, color);
        }

        /// <summary>
        /// Manages changing the animation frame
        /// </summary>
        void Animate()
        {
            if (timeBetweenFrames.Check(1))
            {
                curFrame.Check(1);

                frameRec.x = curFrame.Time * (float)texture.width / frames.X;
            }

            animReset = false;
        }
        /// <summary>
        /// Resets the Animation
        /// </summary>
        void ResetAnim()
        {
            curFrame.Reset();
            frameRec.x = 0;
            animReset = true;
        }

        /// <summary>
        /// change the animation loop
        /// </summary>
        /// <param name="state">what animation state to change to</param>
        public void SetState(int state)
        {
            if (state >= frames.Y)
                state = 0;

            frameRec.y = state * (float)texture.height / frames.Y;
        }
    }
}
