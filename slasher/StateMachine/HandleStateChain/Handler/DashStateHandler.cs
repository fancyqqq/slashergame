using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class DashStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public DashStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle()
    {
        float totalTime = _stateData.TotalTime;
        bool doubleTapA = false;
        bool doubleTapD = false;

        if (_stateData.IsLeftPressed && !_stateData.APressedLastFrame)
        {
            if (_stateData.LastAPressTime >= 0 && (totalTime - _stateData.LastAPressTime) <= PlayerStateData.DoubleTapInterval)
            {
                doubleTapA = true;
                _stateData.LastAPressTime = -1f;
            }
            else
            {
                _stateData.LastAPressTime = totalTime;
            }
        }
        _stateData.APressedLastFrame = _stateData.IsLeftPressed;

        if (_stateData.IsRightPressed && !_stateData.DPressedLastFrame)
        {
            if (_stateData.LastDPressTime >= 0 && (totalTime - _stateData.LastDPressTime) <= PlayerStateData.DoubleTapInterval)
            {
                doubleTapD = true;
                _stateData.LastDPressTime = -1f;
            }
            else
            {
                _stateData.LastDPressTime = totalTime;
            }
        }
        _stateData.DPressedLastFrame = _stateData.IsRightPressed;
        
        if (doubleTapA) _stateData.DashDirection = -1;
        if (doubleTapD) _stateData.DashDirection = 1;
        
        return _stateData.CanDash && (doubleTapA || doubleTapD) &&
               StateMachine.Player.State is not (PlayerState.Attack1 or PlayerState.Attack2 or PlayerState.Attack3 or
                   PlayerState.AirAttack or PlayerState.SpecialAttack or PlayerState.Defend or PlayerState.HurtBlock);
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<DashState>();
    }
}