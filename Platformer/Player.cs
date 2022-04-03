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

        public double Velocity;

        MediaPlayer sound = new MediaPlayer();

        public Player(double x, double y) : base(x, y, Config.playerWidth, Config.playerHeight)
        {
            lives = 2;
            Velocity = 0;
            Brush = Config.playerBrush;
            sound.Volume = 0.1;
        }

        public void Move()
        {
            if (GoLeft)
            {
                SetX(Velocity -= 0.1);
            }
            else if (GoRight)
            {
                SetX(Velocity += 0.1);
            }
            else if (!GoLeft && Velocity < 0)
            {
                Velocity += 0.1;
            }
            else if (!GoRight && Velocity > 0)
            {
                Velocity -= 0.1;
            }

            if (IsJumping)
            {
                Jump();
            }
            else if (IsFalling)
            {
                Falling();
            }
        }

        bool StartJumping = false;
        double jumpHeight;
        double maxJump = 7;
        public void Jump()
        {
            if (!StartJumping)
            {
                jumpHeight = -maxJump;
                StartJumping = true;
                sound.Open(Config.JumpSound);
                sound.Play();
            }

            if (IsJumping)
            {
                SetY(jumpHeight += 0.1);
            }
            else
            {
                StartJumping = false;
            }
        }

        bool StartFalling = false;
        double fallSpeed;
        public void Falling()
        {
            if (!StartFalling)
            {
                fallSpeed = 0.1;
                StartFalling = true;
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
                StartFalling = false;
            }
        }

        public void Bounce()
        {
            StartJumping = false;
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
