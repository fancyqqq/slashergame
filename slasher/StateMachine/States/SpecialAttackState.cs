using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher;

public class SpecialAttackState : PlayerBaseState
{
    public SpecialAttackState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.SpecialAttack, "special_attack", false);
        Data.Velocity = new Vector2(0, Data.Velocity.Y);
        Data.SpecialStarted = true;
    }

    public override void OnExit()
    {
        Data.SpecialStarted = false;
    }

    public override void OnUpdateBehaviour(KeyboardState ks)
    {
        if (Player.CurrentAnimation.IsFinished)
        {
            MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
        }
    }
}