using System.ComponentModel;
using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Battle : Node
{
    private Player player;
    private RessourceIndicator ressourceIndicator;
    public PackedScene MonkeyType => ResourceLoader.Load<PackedScene>("res://Scenes/Classes/mandrill.tscn");
    public PackedScene PlayerScene => ResourceLoader.Load<PackedScene>("res://Scenes/player.tscn");

    [Export] public PauseMenu PauseMenu { get; set; }

    [Export] public WaveTimer WaveTimer { get; set; }

    [Export] public EndOfWavePanel EndOfWavePanel { get; set; }

    [Export] public Deathscreen Deathscreen { get; set; }

    public override void _Ready()
    {
        ressourceIndicator ??= GetNode<CanvasLayer>("UI").GetNode<RessourceIndicator>(nameof(RessourceIndicator));

        InstantiatePlayer();

        var unitSpawner = GetNode<UnitSpawner>(nameof(UnitSpawner));
        unitSpawner.Initialize(this, player);

        WaveTimer.OnWaveEnded += WaveTimerOnWaveEnded;

        TreeExiting += OnTreeExiting;
    }

    private void InstantiatePlayer()
    {
        var newPlayer = PlayerScene.Instantiate<Player>();
        var monkey = StaticMemory.SelectedMonkey;

        if (StaticMemory.Player is not null)
        {
            newPlayer.HealthCurrent = StaticMemory.Player.HealthCurrent;
            newPlayer.BananasHeld = StaticMemory.Player.BananasHeld;
            newPlayer.BananasSpent = StaticMemory.Player.BananasSpent;
        }

        newPlayer.Speed = 400;
        newPlayer.Position = new Vector2(900, 500);
        newPlayer.SetMonkeyClass(monkey);

        newPlayer.PropertyChanged += PlayerOnPropertyChanged;
        newPlayer.PlayerDied += OnPlayerDied;

        ressourceIndicator.SetBananaAmount(newPlayer.BananasHeld);
        player = newPlayer;

        AddChild(player);
    }

    private void OnPlayerDied()
    {
        Deathscreen.Visible = true;
        GetTree().Paused = true;
    }

    private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Player.BananasHeld) || sender is not Player playerObject)
            return;

        if (!IsInstanceValid(ressourceIndicator) && IsInstanceValid(this))
            ressourceIndicator = GetNode<CanvasLayer>("UI").GetNode<RessourceIndicator>(nameof(RessourceIndicator));

        ressourceIndicator?.SetBananaAmount(playerObject.BananasHeld);
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Escape))
        {
            PauseMenu.Visible = true;
            GetTree().Paused = true;
        }
    }

    private void WaveTimerOnWaveEnded()
    {
        EndRound();
    }

    private void EndRound()
    {
        EndOfWavePanel.Visible = true;
        GetTree().Paused = true;
    }

    private void OnTreeExiting()
    {
        player.PropertyChanged -= PlayerOnPropertyChanged;
        WaveTimer.OnWaveEnded -= WaveTimerOnWaveEnded;
        TreeExiting -= OnTreeExiting;
    }
}