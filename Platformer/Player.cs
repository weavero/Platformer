using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Platformer
{
    class Player : Actor
    {
        public bool WasDamaged { get; set; }

        public Player(double x, double y) : base(x, y, Config.playerWidth, Config.playerHeight)
        {
            lives = 2;
        }

        public void Move()
        {
            if (GoLeft)
            {
                Velocity -= 0.1;
            }
            else if (!GoLeft && Velocity < 0)
            {
                Velocity += 0.1;
            }

            if (GoRight)
            {
                Velocity += 0.1;
            }
            else if (!GoRight && Velocity > 0)
            {
                Velocity -= 0.1;
            }

            SetX(Velocity);

            Jump();
            Falling();
        }

        int i = 0;
        double jumpHeight;
        double maxJump = 7;
        public void Jump()
        {
            if (i < 1)
            {
                jumpHeight = -maxJump;
                i++;
            }

            if (IsJumping)
            {
                SetY(jumpHeight += 0.1);
            }
            else
            {
                i--;
            }
        }

        int j = 0;
        double fallSpeed;
        public void Falling()
        {
            if (j < 1)
            {
                fallSpeed = 0.1;
                j++;
            }

            if (IsFalling)
            {
                if (fallSpeed < 10)
                {
                    fallSpeed += 0.1;
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
