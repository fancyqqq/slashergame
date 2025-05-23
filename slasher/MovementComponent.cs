using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher
{
    public class MovementComponent
    {
        private readonly Player _player;
        private float _moveSpeed = 400f;
        private bool _isFacingRight = true;
        public bool IsFacingRight => _isFacingRight;
        public Vector2 Velocity { get; private set; } = Vector2.Zero;

        public MovementComponent(Player player)
        {
            _player = player;
        }

        public void Update(KeyboardState ks)
        {
            if (_player.State is not (PlayerState.Attack1 or PlayerState.Attack2 or PlayerState.Attack3 or
                PlayerState.Defend or PlayerState.HurtBlock or PlayerState.Dash or PlayerState.SpecialAttack))
            {
                if (ks.IsKeyDown(Keys.A))
                {
                    Velocity = new Vector2(-_moveSpeed, Velocity.Y);
                    _isFacingRight = false;
                }
                else if (ks.IsKeyDown(Keys.D))
                {
                    Velocity = new Vector2(_moveSpeed, Velocity.Y);
                    _isFacingRight = true;
                }
                else
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }

                _player.Velocity = new Vector2(Velocity.X, _player.Velocity.Y);
            }
        }
    }
}