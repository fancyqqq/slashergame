using Microsoft.Xna.Framework;

namespace slasher;

public class GroundMoveStrategy : IMoveStrategy
{
    private PlayerStateData _stateData;
    private Player _player;

    public GroundMoveStrategy(PlayerStateData stateData, Player player)
    {
        _stateData = stateData;
        _player = player;
    }
    
    public void Move()
    {
        if (_stateData.IsLeftPressed)
        {
            _stateData.Velocity = new Vector2(-_stateData.MoveSpeed, _stateData.Velocity.Y);
            _stateData.IsFacingRight = false;
        }
        else if (_stateData.IsRightPressed)
        {
            _stateData.Velocity = new Vector2(_stateData.MoveSpeed, _stateData.Velocity.Y);
            _stateData.IsFacingRight = true;
        }
        _player.Velocity = new Vector2(_stateData.Velocity.X, _player.Velocity.Y);
    }
}