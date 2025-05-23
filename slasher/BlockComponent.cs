using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher
{
    public class BlockComponent
    {
        private readonly Player _player;

        public BlockComponent(Player player)
        {
            _player = player;
        }

        public void Update(MouseState ms)
        {
            if (_player.IsDashing || _player.IsJumping)
                return;

            if (_player.State != PlayerState.Defend && ms.RightButton == ButtonState.Pressed)
            {
                _player.SetStateAndAnimation(PlayerState.Defend, "defend");
                _player.Velocity = new Vector2(0, _player.Velocity.Y);
            }
            else if (_player.State == PlayerState.Defend && ms.RightButton == ButtonState.Released)
            {
                _player.SetState(PlayerState.Idle);
            }
        }

        public void UpdatePhases()
        {
            if (_player.State == PlayerState.HurtBlock && _player.CurrentAnimation.IsFinished)
            {
                _player.SetStateAndAnimation(PlayerState.Idle, "idle");
            }
        }

        public void TriggerBlockHurt()
        {
            _player.SetStateAndAnimation(PlayerState.HurtBlock, "defend-hurt", false);
        }
    }
}