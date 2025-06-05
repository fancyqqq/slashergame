using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher;

public interface IState
{
    void OnEnter();
    void OnExit();
    void OnUpdateBehavior(KeyboardState ks);
}