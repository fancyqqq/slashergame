using Microsoft.Xna.Framework.Input;

namespace slasher;

public class InputReader
{
    private bool _attackPressedLastFrame;
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
            bool isAttackPressed = ms.LeftButton == ButtonState.Pressed;
            _stateData.UpdateEvent(isAttackPressed);
        _stateData.IsDefendPressed = ms.RightButton == ButtonState.Pressed;
        _stateData.IsSpecialPressed = ks.IsKeyDown(Keys.R);

    }
}