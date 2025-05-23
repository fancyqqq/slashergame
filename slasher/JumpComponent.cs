using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher
{
    public class JumpComponent
    {
        public enum JumpPhase
        {
            Start,
            Transition,
            Fall
        }

        private readonly Player _player;
        private bool _isJumping = false;
        private JumpPhase _jumpPhase;
        private float _jumpForce = -550f;
        private float _groundY = 300f;

        public bool IsJumping
        {
            get => _isJumping;
            set => _isJumping = value;
        }

        public JumpComponent(Player player)
        {
            _player = player;
        }

        public void Update(KeyboardState ks, GameTime gameTime)
        {
            if (TryStartJump(ks)) return;

            if (DetectLateJumpStart()) return;

            if (_isJumping && !_player.IsDashing)
                UpdateJumpPhase();
        }

        private bool TryStartJump(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Space) && !_isJumping && !_player.IsDashing &&
                !_player.State.ToString().StartsWith("Attack") && !_player.IsBlockedAbove())
            {
                _isJumping = true;
                _jumpPhase = JumpPhase.Start;
                _player.Velocity = new Vector2(_player.Velocity.X, _jumpForce);
                _player.SetAnimation("jump_start");
                return true;
            }
            return false;
        }

        private bool DetectLateJumpStart()
        {
            if (!_isJumping && _player.Velocity.Y > 0 && _player.Position.Y < _groundY)
            {
                _isJumping = true;
                _jumpPhase = JumpPhase.Transition;
                TrySetAnimation("jump_trans", false);
                return true;
            }
            return false;
        }

        private void UpdateJumpPhase()
        {
            switch (_jumpPhase)
            {
                case JumpPhase.Start:
                    if (_player.Velocity.Y > -75f)
                    {
                        _jumpPhase = JumpPhase.Transition;
                        if (_player.State != PlayerState.AirAttack)
                            TrySetAnimation("jump_trans", false);
                    }
                    break;

                case JumpPhase.Transition:
                    if (_player.CurrentAnimation == _player.GetAnimation("jump_trans") &&
                        _player.CurrentAnimation.IsFinished)
                    {
                        _jumpPhase = JumpPhase.Fall;
                        _player.SetAnimation("jump_fall");
                    }
                    break;
            }
        }

        private void TrySetAnimation(string name, bool loop = false)
        {
            var anim = _player.GetAnimation(name);
            if (_player.CurrentAnimation != anim)
                _player.SetAnimation(name, loop);
        }
    }
}
