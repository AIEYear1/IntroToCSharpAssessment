using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// A sprite that has an actual texture
    /// </summary>
    public class ImageObject
    {
        /// <summary>
        /// Texture to be used
        /// </summary>
        public Texture2D texture = new Texture2D();
        /// <summary>
        /// Color overlay on the texture
        /// </summary>
        public Color color = WHITE;

        Vector2 position = new Vector2();

        /// <summary>
        /// Position of Sprite (Top Left)
        /// </summary>
        public virtual Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public ImageObject(Texture2D image, Vector2 position, Color color)
        {
            this.texture = image;
            this.position = position;
            this.color = color;
        }

        /// <summary>
        /// Draws the texture
        /// </summary>
        public virtual void Draw()
        {
            DrawTextureV(texture, position, color);
        }
    }
}
