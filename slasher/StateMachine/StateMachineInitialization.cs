namespace slasher;

public class StateMachineInitialization
{
    public StateMachine PlayerStateMachine { get; private set; }
    public TestPlayer TestPlayer { get; private set; }
    public PlayerStateData PlayerStateData { get; private set; }

    public StateMachineInitialization()
    {
        System.Diagnostics.Debug.WriteLine("попа1");
        Initialize();
    }


    private void Initialize()
    {
        PlayerStateMachine = new StateMachine(new Idle(PlayerStateData, TestPlayer, this),
            new Run(PlayerStateData, TestPlayer, this));
        TestPlayer = new TestPlayer();
        PlayerStateData = new PlayerStateData();
        
        PlayerStateMachine.SwitchStates<Idle>();
    }
}