using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Line sprite that travels in one direction
    /// </summary>
    class LineSprite
    {
        /// <summary>
        /// Center position of the line
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// How thick the line is for collision sake
        /// </summary>
        public float thickness;
        /// <summary>
        /// Length of the line
        /// </summary>
        public readonly float Length;

        //Start position of the line
        Vector2 startPos;
        //End position of the line
        Vector2 endPos;
        //Direction of the line and where it travels
        Vector2 direction;
        //speed of travel
        readonly float speed;
        //Color of the line
        Color color;

        /// <summary>
        /// Start position of the line
        /// </summary>
        public Vector2 StartPos { get => startPos; }
        /// <summary>
        /// End position of the line
        /// </summary>
        public Vector2 EndPos { get => endPos; }

        public LineSprite(Vector2 position, Vector2 direction, float length, float thickness, float speed, Color color)
        {
            direction = Utils.LockMagnitude(direction, 1);
            this.position = position;
            startPos = position - (direction * (length / 2));
            endPos = position + (direction * (length / 2));
            this.direction = direction;
            this.Length = length;
            this.thickness = thickness;
            this.speed = speed;
            this.color = color;
        }

        //bool to make sure the LineSprite doesn't get spawned multiple times
        bool spawned = false;
        /// <summary>
        /// Spawns one line sprite
        /// </summary>
        /// <param name="player"></param>
        /// <param name="overridePosition"></param>
        public void Spawn(Vector2 player, bool overridePosition = false)
        {
            if (spawned)
                return;

            if (!overridePosition)
            {
                Vector2 pos = new Vector2(Utils.NumberBetween((int)(Window.playZoneBarrier.X + 10), (int)(Window.playZoneBarrier.Z - 10)),
                                      Utils.NumberBetween((int)(Window.playZoneBarrier.Y + 10), (int)(Window.playZoneBarrier.W - 10)));

                float incrementer = 0;
                while (Vector2.Distance(pos, player) < Length * 3)
                {
                    incrementer += Length * 3;
                    pos = new Vector2(Utils.NumberBetween((int)(Window.playZoneBarrier.X - incrementer + 10), (int)(Window.playZoneBarrier.Z + incrementer - 10)),
                                         Utils.NumberBetween((int)(Window.playZoneBarrier.Y - incrementer + 10), (int)(Window.playZoneBarrier.W + incrementer - 10)));
                }

                position = pos;
            }
            direction = player - position;
            spawned = true;
        }

        public void Update()
        {
            direction = Utils.LockMagnitude(direction, 1);
            position += direction * speed;

            startPos = position - (direction * (Length / 2));
            endPos = position + (direction * (Length / 2));
        }

        public void Draw()
        {
            DrawLineV(startPos, endPos, color);
        }
    }
}
