using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public class RectangleSprite
    {
        public Color color = WHITE;
        Rectangle rectangle;

        public Rectangle Rectangle 
        { 
            get => rectangle;
            set => rectangle = value;
        }
        public Vector2 Position
        {
            get => new Vector2(rectangle.x, rectangle.y);
            set
            {
                rectangle.x = value.X;
                rectangle.y = value.Y;
            }
        }
        public float Width
        {
            get => rectangle.width;
            set => rectangle.width = value;
        }
        public float Height
        {
            get => rectangle.height;
            set => rectangle.height = value;
        }

        public RectangleSprite(Vector2 position, float width, float height, Color color)
        {
            this.rectangle = new Rectangle(position.X,position.Y, width, height);
            this.color = color;
        }

        public void Draw()
        {
            DrawRectangleRec(rectangle, color);
        }
    }
}
