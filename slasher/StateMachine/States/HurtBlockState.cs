using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher;

public class HurtBlockState : PlayerBaseState
{
    public HurtBlockState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.HurtBlock, "defend-hurt", false);
        Data.Velocity = new Vector2(0, Data.Velocity.Y);
    }

    public override void OnUpdateBehavior()
    {
        if (Player.CurrentAnimation.IsFinished)
        {
            MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
        }
    }
}