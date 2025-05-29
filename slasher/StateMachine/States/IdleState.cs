using Microsoft.Xna.Framework.Input;

namespace slasher;

public class Idle : PlayerBaseState
{
    public Idle(PlayerStateData data, TestPlayer testPlayer, StateMachineInitialization machineInitialization) : base(data, testPlayer, machineInitialization)
    {
    }
    
    public override void OnEnter()
    {
        System.Diagnostics.Debug.WriteLine("check1");
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdateBehaviour(KeyboardState ks)
    {
        if (ks.IsKeyDown(Keys.Space))
        {
            _machineInitialization.PlayerStateMachine.SwitchStates<Run>();
        }
        System.Diagnostics.Debug.WriteLine("попа2");
    }
}