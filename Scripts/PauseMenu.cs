using Godot;

namespace MonkeSurvivor.Scripts;

public partial class PauseMenu : Control
{
    public void _on_button_pressed()
    {
        Visible          = false;
        GetTree().Paused = false;
    }
}