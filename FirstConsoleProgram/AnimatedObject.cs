using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace raygamecsharp
{
    public class AnimatedObject : ImageObject
    {
        public bool animate = true, animReset = false;
        public int radius = 0;
        public int framesSpeed = 8;

        Rectangle frameRec;
        Vector2 frames;
        Timer timeBetweenFrames;
        Timer curFrame;
        readonly float scalar = 1;

        public AnimatedObject(Texture2D image, Vector2 position, Color color, int tileSize, Vector2 frames, int radius) : base(image, position, color)
        {
            this.radius = radius;
            scalar = (float)(radius * 2) / (float)tileSize;

            frameRec = new Rectangle(0, 0, tileSize, tileSize);

            this.frames = frames;

            timeBetweenFrames = new Timer(60 / framesSpeed);
            curFrame = new Timer(this.frames.X);
        }

        public AnimatedObject() { }

        public new void Draw()
        {
            if (animate)
            {
                Animate();
            }
            else if (!animReset)
            {
                ResetAnim();
            }

            DrawTexturePro(texture, frameRec, new Rectangle(position.X, position.Y, frameRec.width * scalar, frameRec.height * scalar),
                new Vector2(frameRec.width, frameRec.height), 0, color);
        }

        void Animate()
        {
            if (timeBetweenFrames.Check(1))
            {
                curFrame.Check(1);

                frameRec.x = curFrame.CurrentTime * (float)texture.width / frames.X;
            }

            animReset = false;
        }
        void ResetAnim()
        {
            curFrame.Reset();
            frameRec.x = 0;
            animReset = true;
        }

        public void SetState(int state)
        {
            if (state >= frames.Y)
                state = 0;

            frameRec.y = state * (float)texture.height / frames.Y;
        }

        public virtual void Update() { }
    }
}
