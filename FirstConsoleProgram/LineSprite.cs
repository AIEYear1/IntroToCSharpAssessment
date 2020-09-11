using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    class LineSprite
    {
        public Vector2 position;
        public float thickness;

        Vector2 startPos;
        Vector2 endPos;
        Vector2 direction;
        float length;
        float speed;
        Color color;

        public Vector2 StartPos { get => startPos; }
        public Vector2 EndPos { get => endPos; }
        public float Length { get => length; }

        public LineSprite(Vector2 position, Vector2 direction, float length, float thickness, float speed, Color color)
        {
            direction = Utils.LockMagnitude(direction, 1);
            this.position = position;
            startPos = position - (direction * (length / 2));
            endPos = position + (direction * (length / 2));
            this.direction = direction;
            this.length = length;
            this.thickness = thickness;
            this.speed = speed;
            this.color = color;
        }

        bool spawned = false;
        public void Spaawn(Vector2 player)
        {
            if (spawned)
                return;

            Vector2 pos = new Vector2(Utils.NumberBetween((int)(Window.playZoneBarrier.X + 10), (int)(Window.playZoneBarrier.Z - 10)),
                                  Utils.NumberBetween((int)(Window.playZoneBarrier.Y + 10), (int)(Window.playZoneBarrier.W - 10)));

            float incrementer = 0;
            while(Vector2.Distance(pos, player) < length * 3)
            {
                incrementer += length*3;
                pos = new Vector2(Utils.NumberBetween((int)(Window.playZoneBarrier.X - incrementer + 10), (int)(Window.playZoneBarrier.Z + incrementer - 10)),
                                     Utils.NumberBetween((int)(Window.playZoneBarrier.Y - incrementer + 10), (int)(Window.playZoneBarrier.W + incrementer - 10)));
            }

            position = pos;
            direction = player - position;
            spawned = true;
        }

        public void Update()
        {
            direction = Utils.LockMagnitude(direction, 1);
            position += direction * speed * GetFrameTime();

            startPos = position - (direction * (length / 2));
            endPos = position + (direction * (length / 2));
        }

        public void Draw()
        {
            DrawLineV(startPos, endPos, color);
        }
    }
}
