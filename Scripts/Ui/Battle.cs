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
    public WaveTimer WaveTimer { get; set; }

    public override void _Ready()
    {
        base._Ready();

        var monkey = MonkeyType.Instantiate<BaseMonkey>();

        player = GetNode<Player>(nameof(Player));
        player.SetMonkeyClass(monkey);

        WaveTimer.OnWaveEnded += WaveTimerOnOnWaveEnded;
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            PauseMenu.Visible = true;
            GetTree().Paused  = true;
        }
    }

    private void WaveTimerOnOnWaveEnded() => EndRound();

    private void EndRound()
    {
        //Show "Wave slaughtered" Label
        //Show "Continue" Button
    }
}