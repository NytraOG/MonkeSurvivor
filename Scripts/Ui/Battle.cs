using Godot;
using MonkeSurvivor.Scripts.Monkeys;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Battle : Node
{
    private Player      player;
    public  PackedScene MonkeyType => ResourceLoader.Load<PackedScene>("res://Scenes/Classes/orangutan.tscn");

    [Export]
    public PauseMenu PauseMenu { get; set; }

    [Export]
    public WaveTimer WaveTimer { get; set; }

    [Export]
    public EndOfWavePanel EndOfWavePanel { get; set; }

    public override void _Ready()
    {
        base._Ready();

        var monkey = MonkeyType.Instantiate<BaseMonkey>();

        player = GetNode<Player>(nameof(Player));
        player.SetMonkeyClass(monkey);

        player.Vigor        = StaticMemory.Vigor;
        player.Strength     = StaticMemory.Strength;
        player.Dexterity    = StaticMemory.Dexterity;
        player.Intelligence = StaticMemory.Intelligence;

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
        EndOfWavePanel.Visible = true;
        GetTree().Paused       = true;
    }
}