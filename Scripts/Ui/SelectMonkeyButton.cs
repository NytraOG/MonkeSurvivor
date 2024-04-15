using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class SelectMonkeyButton : PanelContainer
{
    public delegate void SelectionPressedEventHandler();

    public event SelectionPressedEventHandler OnSelectPressed;

    public void _on_select_monkey_pressed()
    {
        OnSelectPressed?.Invoke();
    }
}