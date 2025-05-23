using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace slasher
{
    public class AttackComponent
    {
        private readonly Player _player;
        private bool _attackQueued = false;
        private bool _attackPressedLastFrame = false;
        private bool _specialStarted = false;

        private readonly Dictionary<PlayerState, (PlayerState? NextState, string Animation, bool RequiresQueue)> _attackPhases =
            new()
            {
                { PlayerState.Attack1, (PlayerState.Attack2, "attack2", true) },
                { PlayerState.Attack2, (PlayerState.Attack3, "attack3", true) },
                { PlayerState.Attack3, (PlayerState.Idle, "idle", false) },
                { PlayerState.AirAttack, (null, "jump_fall", false) },
                { PlayerState.SpecialAttack, (PlayerState.Idle, "idle", false) }
            };

        public AttackComponent(Player player)
        {
            _player = player;
        }

        public void Update(MouseState ms)
        {
            if (_player.IsJumping)
                HandleAirAttack(ms);
            else
            {
                HandleAttack(ms);
                HandleSpecialAttack(ms);    
            }
        }

        public void UpdatePhases()
        {
            if (_player.CurrentAnimation.IsFinished &&
                _attackPhases.TryGetValue(_player.State, out var transition))
            {
                if (transition.RequiresQueue && _attackQueued && transition.NextState.HasValue)
                {
                    _player.SetStateAndAnimation(transition.NextState.Value, transition.Animation, false);
                    _attackQueued = false;
                }
                else
                {
                    if (_player.State == PlayerState.AirAttack && _player.IsJumping)
                        _player.SetStateAndAnimation(PlayerState.Jump, transition.Animation);
                    else
                        _player.SetStateAndAnimation(PlayerState.Idle, transition.Animation);
                }
            }
        }

        private void HandleAttack(MouseState ms)
        {
            bool isPressed = ms.LeftButton == ButtonState.Pressed;

            if (isPressed && !_attackPressedLastFrame)
            {
                if (_player.State == PlayerState.Idle || _player.State == PlayerState.Run)
                {
                    _player.Velocity = new Vector2(0, _player.Velocity.Y);
                    _player.SetStateAndAnimation(PlayerState.Attack1, "attack1", false);
                }
                else if (_player.State == PlayerState.Attack1 || _player.State == PlayerState.Attack2)
                {
                    _attackQueued = true;
                }
            }

            _attackPressedLastFrame = isPressed;
        }

        private void HandleSpecialAttack(MouseState ms)
        {
            bool isPressed = ms.LeftButton == ButtonState.Pressed;

            if (isPressed && !_specialStarted)
            {
                if (!_player.IsJumping && !_player.IsDashing && _player.State is PlayerState.Idle or PlayerState.Run)
                {
                    _player.Velocity = new Vector2(0, _player.Velocity.Y);
                    _player.SetStateAndAnimation(PlayerState.SpecialAttack, "special_attack", false);
                    _specialStarted = true;
                }
            }

            if (ms.LeftButton == ButtonState.Released)
                _specialStarted = false;
        }

        private void HandleAirAttack(MouseState ms)
        {
            bool isPressed = ms.LeftButton == ButtonState.Pressed;

            if (isPressed && !_attackPressedLastFrame)
            {
                if (_player.State is PlayerState.Jump or PlayerState.Run or PlayerState.Idle)
                {
                    _player.Velocity = new Vector2(_player.Velocity.X, _player.Velocity.Y);
                    _player.SetStateAndAnimation(PlayerState.AirAttack, "air_attack", false);
                }
            }

            _attackPressedLastFrame = isPressed;
        }
    }
}
