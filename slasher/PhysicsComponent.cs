using System;
using Microsoft.Xna.Framework;

namespace slasher
{
    public class PhysicsComponent
    {
        private readonly Player _player;
        private readonly CollisionMap _collisionMap;
        private const float Gravity = 1200f;
        private const float FallGravityMultiplier = 2f;
        private readonly int _groundY;
        private bool _isOnGround = false;
        
        public PhysicsComponent(Player player, CollisionMap collisionMap, int groundY)
        {
            _collisionMap = collisionMap;
            _player = player;
            _groundY = groundY;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 velocity = _player.Velocity;

            if (!_isOnGround)
            {
                if (velocity.Y > 0)
                    velocity.Y += Gravity * FallGravityMultiplier * dt;
                else
                    velocity.Y += Gravity * dt;
            }

            Vector2 proposedPosition = _player.Position + velocity * dt;
            Rectangle current = _player.BoundingBox;

            Rectangle futureBoxX = new Rectangle(
                (int)(current.X + velocity.X * dt),
                current.Y,
                current.Width,
                current.Height
            );

            Rectangle futureBoxY = new Rectangle(
                current.X,
                (int)(current.Y + velocity.Y * dt),
                current.Width,
                current.Height
            );

            Rectangle groundSensor = new Rectangle(
                current.X,
                current.Bottom,
                current.Width,
                2
            );

            bool isGroundBeneath = _collisionMap.IsColliding(groundSensor);

            if (_collisionMap.IsColliding(futureBoxX))
            {
                if (velocity.X > 0)
                {
                    float tileX = (float)Math.Floor((futureBoxX.Right) / 32f) * 32;
                    float newBoxRight = tileX;
                    float offsetX = current.Right - _player.Position.X;
                    float correctedX = newBoxRight - offsetX;
                    _player.Position = new Vector2(correctedX, _player.Position.Y);
                }
                else if (velocity.X < 0)
                {
                    float tileX = (float)Math.Ceiling((futureBoxX.Left) / 32f) * 32;
                    float offsetX = current.Left - _player.Position.X;
                    float correctedX = tileX - offsetX;
                    _player.Position = new Vector2(correctedX, _player.Position.Y);
                }

                velocity.X = 0;
            }
            else
            {
                _player.Position = new Vector2(proposedPosition.X, _player.Position.Y);
            }

            if (_collisionMap.IsColliding(futureBoxY))
            {
                if (velocity.Y > 0)
                {
                    float tileY = (float)Math.Floor((futureBoxY.Bottom) / 32f) * 32;
                    float newBoxTop = tileY - current.Height;
                    float offsetY = current.Top - _player.Position.Y;
                    float correctedY = newBoxTop - offsetY;

                    _player.Position = new Vector2(_player.Position.X, correctedY);
                    velocity.Y = 0;
                    _isOnGround = true;

                    _player.IsJumping = false;
                    if (!_player.IsDashing)
                        _player.SetStateAndAnimation(PlayerState.Idle, "idle");
                }
                else if (velocity.Y < 0)
                {
                    float tileY = (float)Math.Ceiling((futureBoxY.Top) / 32f) * 32;
                    float offsetY = current.Top - _player.Position.Y;
                    float correctedY = tileY - offsetY;

                    _player.Position = new Vector2(_player.Position.X, correctedY);
                    velocity.Y = 0;
                }
            }
            else
            {
                _player.Position = new Vector2(_player.Position.X, proposedPosition.Y);
                _isOnGround = isGroundBeneath;
            }

            _player.Velocity = velocity;
        }

        public bool CheckCollisionBox(Rectangle rect)
        {
            return _collisionMap.IsColliding(rect);
        }
    }
}