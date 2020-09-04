﻿using Raylib_cs;
using System;
using System.Numerics;
using static raygamecsharp.Objects;
using static Raylib_cs.Raylib;

namespace raygamecsharp
{
    public class Character : AnimatedObject
    {
        public float speed = 300;
        public Vector2 velocity;
        public Vector2 direction = new Vector2();
        public float score = 0;

        public float sensitivity = 5;

        public float posSpeedMod = 1;
        public float negSpeedMod = 1;

        public float SpeedMod
        {
            get
            {
                return posSpeedMod * negSpeedMod;
            }
        }


        public Character(Texture2D image, Vector2 position, Color color, int tileSize, Vector2 frames, int radius) : base(image, position, color, tileSize, frames, radius)
        {
            SetState(2);
        }
        public Character()
        {
            SetState(2);
        }

        public override void Update()
        {
            direction.X = Input.GetAxis("Horizontal", sensitivity);
            direction.Y = Input.GetAxis("Vertical", sensitivity);

            direction = Utils.ClampMagnitude(direction, 1);

            velocity = direction * (speed * SpeedMod) * GetFrameTime();
            position += velocity;
            Border();

            SetPlayerAnimState();
        }

        public void SetPlayerAnimState()
        {
            if (direction == Vector2.Zero)
            {
                animate = false;
                return;
            }
            animate = true;

            if (MathF.Abs(direction.X) > MathF.Abs(direction.Y))
            {
                if (direction.X > 0)
                {
                    SetState(2);
                    return;
                }
                if (direction.X < 0)
                {
                    SetState(1);
                    return;
                }
            }

            if (direction.Y < 0)
            {
                SetState(3);
                return;
            }
            if (direction.Y > 0)
            {
                SetState(0);
            }
        }

        public void Border()
        {
            if (position.X < 0 + radius)
            {
                position.X = 0 + radius;
            }
            else if (position.X > Window.screenWidth - radius)
            {
                position.X = Window.screenWidth - radius;
            }

            if (position.Y < 0 + radius)
            {
                position.Y = 0 + radius;
            }
            else if (position.Y > Window.screenHeight - radius)
            {
                position.Y = Window.screenHeight - radius;
            }
        }
    }
}