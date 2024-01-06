
namespace Platformer.Models
{
    class Player : Actor
    {
        public Player(double x, double y) : base(x, y, Config.PlayerWidth, Config.PlayerHeight)
        {
            hitsToKill = 1;
            Velocity = 0;
            Brush = Config.PlayerBrush;
        }


        double maxVelocity = 5;
        public void Move()
        {
            MoveLeft();
            MoveRight();
            SetX(Velocity);
            Jump();
            Falling();
        }

        private void MoveLeft()
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
        }

        private void MoveRight()
        {
            if (GoRight && Velocity < maxVelocity)
            {
                Velocity += 0.10;
            }
            else if (!GoRight && Velocity > 0)
            {
                // Nem tér vissza pontosan nullára -> folyamatos mozgás
                if (Velocity < 0.1)
                {
                    Velocity = 0;
                }
                else
                {
                    Velocity -= 0.10;
                }
            }
        }


        bool StartJumping = false;
        double jumpHeight;
        double maxJump = 5;
        public void Jump()
        {
            if (IsJumping)
            {
                if (!StartJumping)
                {
                    //actorSoundPlayer.Open(Config.JumpSound);
                    //actorSoundPlayer.Play();
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

        public void Bounce()
        {
            StartJumping = false;
            IsFalling = false;
            IsJumping = true;
            Jump();
        }
    }
}
