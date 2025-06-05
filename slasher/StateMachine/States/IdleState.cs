using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Idle, "idle");
        Data.Velocity = new Vector2(0, Data.Velocity.Y);
    }

    public override void OnExit()
    {
        
    }
    
    public override void OnUpdateBehavior(KeyboardState ks)
    {
        //System.Diagnostics.Debug.WriteLine(Data.IsAttackPressed);
        MachineInitialization.StateHandleChain.HandleState<RunStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<JumpStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<Attack1StateHandler>();
        MachineInitialization.StateHandleChain.HandleState<DashStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<DefendStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<SpecialAttackStateHandler>();
    }
}