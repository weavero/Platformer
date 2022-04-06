using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Platformer
{
    class Player : Actor
    {
        public bool WasDamaged { get; set; }

        public Player(double x, double y) : base(x, y, Config.PlayerWidth, Config.PlayerHeight)
        {
            lives = 2;
            Velocity = 0;
            Brush = Config.PlayerBrush;
        }

        double maxVelocity = 5;
        public void Move()
        {
            if (GoLeft && Velocity > -maxVelocity)
            {
                Velocity -= 0.10;
            }
            else if (!GoLeft && Velocity < 0)
            {
                // Megelőzi, hogy pontatlan legyen a változó
                if (Velocity > -0.1)
                {
                    Velocity = 0;
                }
                else
                {
                    Velocity += 0.10;
                }
            }

            if (GoRight && Velocity < maxVelocity)
            {
                Velocity += 0.10;
            }
            else if (!GoRight && Velocity > 0)
            {
                // Megelőzi, hogy pontatlan legyen a változó
                if (Velocity < 0.1)
                {
                    Velocity = 0;
                }
                else
                {
                    Velocity -= 0.10;
                }
            }

            SetX(Velocity);

            Jump();
            Falling();
        }

        public void Bounce()
        {
            StartJumping = false;
        }

        bool StartJumping = false;
        double jumpHeight;
        double maxJump = 7;
        public void Jump()
        {
            if (IsJumping)
            {
                if (!StartJumping)
                {
                    jumpHeight = -maxJump;
                    StartJumping = true;
                }
                SetY(jumpHeight += 0.1);
            }
            else
            {
                StartJumping = false;
            }
        }

        int StartFalling = 0;
        double fallSpeed;
        public void Falling()
        {
            if (IsFalling)
            {
                if (StartFalling < 1)
                {
                    fallSpeed = 0.10;
                    StartFalling++;
                }
                if (fallSpeed < 10)
                {
                    fallSpeed += 0.10;
                }
                SetY(fallSpeed);
            }
            else
            {
                StartFalling = 0;
            }
        }
    }
}
