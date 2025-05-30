using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher;

public class RunState : PlayerBaseState
{
    private readonly float _moveSpeed = 400f;

    public RunState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Run, "run");
    }

    public override void OnUpdateBehaviour(KeyboardState ks)
    {
        if (Data.IsLeftPressed)
        {
            Data.Velocity = new Vector2(-_moveSpeed, Data.Velocity.Y);
            Data.IsFacingRight = false;
        }
        else if (Data.IsRightPressed)
        {
            Data.Velocity = new Vector2(_moveSpeed, Data.Velocity.Y);
            Data.IsFacingRight = true;
        }
        Player.Velocity = new Vector2(Data.Velocity.X, Player.Velocity.Y);
    }
}