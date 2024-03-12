using Godot;

namespace MonkeSurvivor.Scripts;

public partial class UnitSpawner : Control
{
    private TextureRect background;
    private Node        battleScene;
    private Player      player;
    private double      waveTimer;

    [Export]
    public PackedScene UnitToSpawn { get; set; }

    [Export]
    public int AmountPerWave { get; set; } = 5;

    [Export]
    public int WaveCooldown { get; set; } = 5;

    [Export]
    public float WaveCooldownModifier { get; set; } = 1;

    private float ModifiedCooldown => WaveCooldown * WaveCooldownModifier;

    public override void _Ready()
    {
        var sceneTree = GetTree();

        battleScene = sceneTree.CurrentScene;
        player      = battleScene.GetNode<Player>(nameof(Player));
        background  = battleScene.GetNode<TextureRect>("Background");
    }

    public override void _Process(double delta)
    {
        if ((waveTimer += delta) >= ModifiedCooldown)
            SpawnWave<Spider>();
    }

    private void SpawnWave<T>()
            where T : Enemy
    {
        for (var i = 0; i < AmountPerWave; i++)
        {
            SpawnUnit<T>(i);
        }

        waveTimer = 0;
    }

    private void SpawnUnit<T>(int offset)
            where T : Enemy
    {
        var enemyInstance = UnitToSpawn.Instantiate<T>();
        enemyInstance.MoveAndSlide();
        battleScene.AddChild(enemyInstance);
    }
}