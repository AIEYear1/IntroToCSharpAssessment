using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public class ImageObject
    {
        public Texture2D texture = new Texture2D();
        public Vector2 position = new Vector2();
        public Color color = WHITE;

        public ImageObject(Texture2D image, Vector2 position, Color color)
        {
            this.texture = image;
            this.position = position;
            this.color = color;
        }
        public ImageObject()
        {
        }

        public void Draw()
        {
            DrawTextureV(texture, position, color);
        }
    }
}
