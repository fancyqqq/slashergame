using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher;

public class Attack2State : PlayerBaseState
{
    public Attack2State(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Attack2, "attack2", false);
        Data.Velocity = new Vector2(0, Data.Velocity.Y);
    }

    public override void OnExit()
    {
        Data.AttackQueued = false;
    }

    public override void OnUpdateBehaviour(KeyboardState ks)
    {
        if (Player.CurrentAnimation.IsFinished)
        {
            if (Data.AttackQueued)
                MachineInitialization.PlayerStateMachine.SwitchStates<Attack3State>();
            else
                MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
        }
    }
}