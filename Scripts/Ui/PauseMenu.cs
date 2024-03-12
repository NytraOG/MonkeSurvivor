using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class PauseMenu : Control
{
    public void _on_button_pressed()
    {
        Visible          = false;
        GetTree().Paused = false;
    }
}