using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Popup text that disappears after a second
    /// </summary>
    public class PopUpText
    {
        /// <summary>
        /// Timer for alpha Fade
        /// </summary>
        public Timer alphaFade;

        //Text to show
        readonly string text;
        //Position of the text
        Vector2 position;
        //Size of the font
        readonly int fontsize;
        //Color of the text
        Color color;
        //Speed of movement
        readonly int movementSpeed;
        //Direction of movement
        Vector2 direction;

        /// Parameters
        /// <param name="text">Text to show</param>
        /// <param name="position">Position of the text</param>
        /// <param name="fontsize">Size of the text</param>
        /// <param name="color">Color of the text</param>
        /// <param name="movementSpeed">Speed of movement</param>
        /// <param name="direction">Direction of movement</param>
        /// <param name="fadeDur">How long it lasts</param>
        public PopUpText(string text, Vector2 position, int fontsize, Color color, int movementSpeed, Vector2 direction, float fadeDur)
        {
            this.text = text;
            this.position = position;
            this.fontsize = fontsize;
            this.color = color;
            this.movementSpeed = movementSpeed;
            this.direction = direction;
            alphaFade = new Timer(fadeDur);
        }

        /// <summary>
        /// Updaes the position of the text
        /// </summary>
        public void Update()
        {
            if (alphaFade.Check(false))
            {
                return;
            }

            Vector2 velocity = direction * movementSpeed * GetFrameTime();
            position += velocity;
        }

        /// <summary>
        /// Draws the text
        /// </summary>
        public void Draw()
        {
            if (alphaFade.IsComplete(false))
            {
                return;
            }

            DrawText(text, (int)position.X, (int)position.Y, fontsize, Fade(color, 1 - alphaFade.PercentComplete));
        }
    }
}
