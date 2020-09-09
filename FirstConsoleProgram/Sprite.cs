using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public class Sprite
    {
        public Vector2 position = new Vector2();
        public float radius = 0;
        public Color color = WHITE;

        public Sprite(Vector2 position, float radius, Color color)
        {
            this.position = position;
            this.radius = radius;
            this.color = color;
        }

        public Sprite()
        {

        }

        public void Draw()
        {
            DrawCircleV(position, radius, color);
        }

        public virtual void Update() { }
    }
}
