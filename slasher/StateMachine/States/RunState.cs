using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

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
        System.Diagnostics.Debug.WriteLine("зашли в ран");
        Player.SetStateAndAnimation(PlayerState.Run, "run");
    }

    public override void OnExit()
    {
        Player.Velocity = Vector2.Zero;
    }
    
    public override void OnUpdateBehavior(KeyboardState ks)
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
        
        MachineInitialization.StateHandleChain.HandleState<IdleStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<Attack1StateHandler>();
        MachineInitialization.StateHandleChain.HandleState<JumpStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<DashStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<DefendStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<SpecialAttackStateHandler>();
    }
}