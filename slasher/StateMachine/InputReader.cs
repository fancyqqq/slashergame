using Microsoft.Xna.Framework.Input;

namespace slasher;

public class InputReader
{
    private readonly PlayerStateData _stateData;

    public InputReader(PlayerStateData stateData)
    {
        _stateData = stateData;
    }

    public void Read(KeyboardState ks, MouseState ms)
    {
        _stateData.IsLeftPressed = ks.IsKeyDown(Keys.A);
        _stateData.IsRightPressed = ks.IsKeyDown(Keys.D);
        _stateData.IsJumpPressed = ks.IsKeyDown(Keys.Space);
        _stateData.IsAttackPressed = ms.LeftButton == ButtonState.Pressed;
        _stateData.IsDefendPressed = ms.RightButton == ButtonState.Pressed;
        _stateData.IsSpecialPressed = ms.LeftButton == ButtonState.Pressed;
        
        System.Diagnostics.Debug.WriteLine($"Input: Left={_stateData.IsLeftPressed}, Right={_stateData.IsRightPressed}, Jump={_stateData.IsJumpPressed}");
    }
}