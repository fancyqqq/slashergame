using Microsoft.Xna.Framework.Input;

namespace slasher;

public class Run : PlayerBaseState
{
    public Run(PlayerStateData data, TestPlayer testPlayer, StateMachineInitialization machineInitialization) : base(data, testPlayer, machineInitialization)
    {
        
    }
    
    public override void OnEnter()
    {
        System.Diagnostics.Debug.WriteLine("попа");
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdateBehaviour(KeyboardState ks)
    {
        System.Diagnostics.Debug.WriteLine("RUN");
    }
}