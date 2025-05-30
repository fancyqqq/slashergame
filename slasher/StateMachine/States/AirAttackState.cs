using Microsoft.Xna.Framework.Input;

namespace slasher;

public class AirAttackState : PlayerBaseState
{
    public AirAttackState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.AirAttack, "air_attack", false);
    }

    public override void OnUpdateBehaviour(KeyboardState ks)
    {
        if (Player.CurrentAnimation.IsFinished)
        {
            MachineInitialization.PlayerStateMachine.SwitchStates<JumpState>();
        }
    }
}