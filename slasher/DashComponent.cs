using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace slasher
{
    public class DashComponent
    {
        private readonly Player _player;
        private bool _isDashing = false;
        private bool _canDash = true;
        private float _dashTime = 0f;
        private float _dashDuration = 0.25f;
        private float _dashSpeed = 1400f;
        private float _dashLerp = 0.96f;
        private float _dashCooldown = 1f;
        private float _dashCooldownTimer = 0f;
        private Animation _windEffect;
        private Vector2 _windPosition;
        private bool _windPlaying = false;
        private bool _windFacingRight = true;
        
        private float _lastAPressTime = -1f;
        private float _lastDPressTime = -1f;
        private bool _aPressedLastFrame = false;
        private bool _dPressedLastFrame = false;
        private const float DoubleTapInterval = 0.5f;

        public bool IsDashing => _isDashing;

        public DashComponent(Player player)
        {
            _player = player;
        }

        public void Update(KeyboardState ks, GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float totalTime = (float)gameTime.TotalGameTime.TotalSeconds;
            
            bool aPressed = ks.IsKeyDown(Keys.A);
            bool dPressed = ks.IsKeyDown(Keys.D);

            bool doubleTapA = false;
            bool doubleTapD = false;
            
            if (aPressed && !_aPressedLastFrame)
            {
                if (_lastAPressTime >= 0 && (totalTime - _lastAPressTime) <= DoubleTapInterval)
                {
                    doubleTapA = true;
                    _lastAPressTime = -1f;
                }
                else
                {
                    _lastAPressTime = totalTime;
                }
            }

            _aPressedLastFrame = aPressed;
            
            if (dPressed && !_dPressedLastFrame)
            {
                if (_lastDPressTime >= 0 && (totalTime - _lastDPressTime) <= DoubleTapInterval)
                {
                    doubleTapD = true;
                    _lastDPressTime = -1f; 
                }
                else
                {
                    _lastDPressTime = totalTime;
                }
            }

            _dPressedLastFrame = dPressed;
            
            if (!_isDashing && _canDash && (doubleTapA || doubleTapD))
            {
                _isDashing = true;
                _canDash = false;
                _dashTime = _dashDuration;
                _dashCooldownTimer = _dashCooldown;

                _player.SetStateAndAnimation(PlayerState.Dash, "dash", false);
                
                bool dashRight = doubleTapD || _player.IsFacingRight;
                _windPosition = _player.Position + new Vector2(dashRight ? 25 : -25, 0);
                _windFacingRight = dashRight;
                _windEffect = _player.GetAnimation("wind");
                _windEffect.IsLooping = false;
                _windEffect.Reset();
                _windPlaying = true;

                _player.Velocity = new Vector2(dashRight ? _dashSpeed : -_dashSpeed, _player.Velocity.Y);
            }
            
            if (_isDashing)
            {
                _dashTime -= 1f / 60f;
                _player.Velocity = new Vector2(_player.Velocity.X * _dashLerp, _player.Velocity.Y);

                if (_dashTime <= 0f && _player.CurrentAnimation.IsFinished)
                {
                    _isDashing = false;
                    if (_player.IsJumping)
                    {
                        _player.SetStateAndAnimation(PlayerState.Jump, "jump_fall");
                    }
                    else
                    {
                        _player.SetStateAndAnimation(PlayerState.Idle, "idle");
                        _player.Velocity = new Vector2(0, _player.Velocity.Y);
                    }
                }
            }
            
            if (!_canDash)
            {
                _dashCooldownTimer -= 1f / 60f;
                if (_dashCooldownTimer <= 0f)
                    _canDash = true;
            }
            
            if (_windPlaying)
            {
                _windEffect.Update(gameTime);
                if (_windEffect.IsFinished)
                    _windPlaying = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_windPlaying)
            {
                spriteBatch.Draw(
                    _windEffect.CurrentFrame,
                    _windPosition,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    3f,
                    _windFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0f);
            }
        }
    }
}