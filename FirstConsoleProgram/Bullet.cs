using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public class Bullet : Sprite
    {
        readonly Character player, playerToHit;
        Vector2 velocity;

        public Bullet(Vector2 position, float radius, Color color, Character player, Character playerToHit, Vector2 velocity, float speed) : base(position, radius, color)
        {
            this.player = player;
            this.playerToHit = playerToHit;
            this.velocity = velocity * speed;
        }

        public override void Update()
        {
            position += velocity * GetFrameTime();

            if (Border())
            {
                return;
            }
        }

        public bool Border()
        {
            if (position.X < 0 + radius || position.X > Window.screenWidth - radius || position.Y < 0 + radius || position.Y > Window.screenHeight - radius)
            {
                return true;
            }

            return false;
        }
    }
}
