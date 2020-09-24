using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Rectangle sprites in raylib window
    /// </summary>
    public class RectangleSprite
    {
        /// <summary>
        /// Color the rectangle will be
        /// </summary>
        public Color color = WHITE;
        /// <summary>
        /// Actual rectangle
        /// </summary>
        Rectangle rectangle;

        /// <summary>
        /// public rectangle getter / setter
        /// </summary>
        public Rectangle Rectangle
        {
            get => rectangle;
            set => rectangle = value;
        }
        /// <summary>
        /// Position getter / setter
        /// </summary>
        public Vector2 Position
        {
            get => new Vector2(rectangle.x, rectangle.y);
            set
            {
                rectangle.x = value.X;
                rectangle.y = value.Y;
            }
        }
        /// <summary>
        /// Width getter / setter
        /// </summary>
        public float Width
        {
            get => rectangle.width;
            set => rectangle.width = value;
        }
        /// <summary>
        /// Height getter / setter
        /// </summary>
        public float Height
        {
            get => rectangle.height;
            set => rectangle.height = value;
        }

        /// Parameters
        /// <param name="position">Position of the rectangle sprite (top left)</param>
        /// <param name="width">Width of the rectangle</param>
        /// <param name="height">Height of the rectangle</param>
        /// <param name="color">Color of the sprite</param>
        public RectangleSprite(Vector2 position, float width, float height, Color color)
        {
            this.rectangle = new Rectangle(position.X, position.Y, width, height);
            this.color = color;
        }

        /// <summary>
        /// Draws the Rectangle sprite
        /// </summary>
        public void Draw()
        {
            DrawRectangleRec(rectangle, color);
        }
    }
}
