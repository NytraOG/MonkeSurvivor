using Godot;
using MonkeSurvivor.Scripts.Monkeys;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Battle : Node
{
    private Player      player;
    public  PackedScene MonkeyType => ResourceLoader.Load<PackedScene>("res://Scenes/Classes/orangutan.tscn");

    [Export]
    public PauseMenu PauseMenu { get; set; }

    [Export]
    public int WaveTimeSeconds { get; set; }

    public override void _Ready()
    {
        base._Ready();

        var monkey = MonkeyType.Instantiate<BaseMonkey>();

        player = GetNode<Player>(nameof(Player));
        player.SetMonkeyClass(monkey);
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            PauseMenu.Visible = true;
            GetTree().Paused  = true;
        }
    }
}