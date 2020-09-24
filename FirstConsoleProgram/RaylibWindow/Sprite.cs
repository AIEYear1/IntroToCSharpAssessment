using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Simple circular sprite
    /// </summary>
    public class Sprite
    {
        /// <summary>
        /// Position of the sprite
        /// </summary>
        public Vector2 position = new Vector2();
        /// <summary>
        /// Radius of the sprite
        /// </summary>
        public float radius = 0;
        /// <summary>
        /// Color of the sprite
        /// </summary>
        public Color color = WHITE;

        /// Parameters
        /// <param name="position">Position of the sprite (centered)</param>
        /// <param name="radius">Radius of the sprite</param>
        /// <param name="color">Color of the sprite</param>
        public Sprite(Vector2 position, float radius, Color color)
        {
            this.position = position;
            this.radius = radius;
            this.color = color;
        }

        public Sprite()
        {

        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        public void Draw()
        {
            DrawCircleV(position, radius, color);
        }

        /// <summary>
        /// Update Override
        /// </summary>
        public virtual void Update() { }
    }
}
