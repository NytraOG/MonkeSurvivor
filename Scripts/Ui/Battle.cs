using Godot;
using MonkeSurvivor.Scripts.Monkeys;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Battle : Node
{
    [Export] public PauseMenu PauseMenu { get; set; }

    public override void _Ready()
    {
        base._Ready();

        var orangutan = new Orangutan();
        GetNode<Player>(nameof(Player)).SetMonkeyClass(orangutan);
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            PauseMenu.Visible = true;
            GetTree().Paused = true;
        }
    }
}