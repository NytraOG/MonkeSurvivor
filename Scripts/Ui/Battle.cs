using Godot;
using MonkeSurvivor.Scripts.Monkeys;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Battle : Node
{
    private Player      player;
    public  PackedScene MonkeyType  => ResourceLoader.Load<PackedScene>("res://Scenes/Classes/mandrill.tscn");
    public  PackedScene PlayerScene => ResourceLoader.Load<PackedScene>("res://Scenes/player.tscn");

    [Export]
    public PauseMenu PauseMenu { get; set; }

    [Export]
    public WaveTimer WaveTimer { get; set; }

    [Export]
    public EndOfWavePanel EndOfWavePanel { get; set; }

    public override void _Ready()
    {
        base._Ready();

        InstantiatePlayer();

        AddChild(player);

        WaveTimer.OnWaveEnded += WaveTimerOnWaveEnded;
    }

    private void InstantiatePlayer()
    {
        if (IsInstanceValid(StaticMemory.Player))
            player = StaticMemory.Player;

        var newPlayer = PlayerScene.Instantiate<Player>();
        var monkey    = MonkeyType.Instantiate<BaseMonkey>();

        newPlayer.Speed    = 300;
        newPlayer.Position = new Vector2(900, 500);
        newPlayer.SetMonkeyClass(monkey);

        player = newPlayer;
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            PauseMenu.Visible = true;
            GetTree().Paused  = true;
        }
    }

    private void WaveTimerOnWaveEnded() => EndRound();

    private void EndRound()
    {
        EndOfWavePanel.Visible = true;
        GetTree().Paused       = true;
    }
}