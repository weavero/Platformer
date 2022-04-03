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

        public Player(double x, double y) : base(x, y, Config.playerWidth, Config.playerHeight)
        {
            lives = 2;
            Velocity = 0;
            Brush = Config.playerBrush;
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
                Velocity += 0.10;
            }

            if (GoRight && Velocity < maxVelocity)
            {
                Velocity += 0.10;
            }
            else if (!GoRight && Velocity > 0 && Velocity != 0)
            {
                Velocity -= 0.10;
            }

            SetX(Math.Round(Velocity));

            Jump();
            Falling();
        }

        public void Bounce()
        {
            i = 0;
        }

        int i = 0;
        double jumpHeight;
        double maxJump = 7;
        public void Jump()
        {
            if (IsJumping)
            {
                if (i < 1)
                {
                    jumpHeight = -maxJump;
                    i++;
                }
                SetY(jumpHeight += 0.1);
            }
            else
            {
                i = 0;
            }
        }

        int j = 0;
        double fallSpeed;
        public void Falling()
        {
            if (j < 1)
            {
                fallSpeed = 0.10;
                j++;
            }

            if (IsFalling)
            {
                if (fallSpeed < 10)
                {
                    fallSpeed += 0.10;
                }
                SetY(fallSpeed);
            }
            else
            {
                j--;
            }
        }
    }
}
