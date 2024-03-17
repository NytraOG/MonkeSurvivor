using System.Collections.Generic;
using Godot;
using MonkeSurvivor.Scripts.Enemies;

namespace MonkeSurvivor.Scripts;

public partial class UnitSpawner : Control
{
    public delegate void WaveSpawnedEventHandler();

    private TextureRect     background;
    private Node            battleScene;
    private Player          player;
    private double          waveTimer;
    public  List<BaseEnemy> SpawnedEnemies { get; set; } = new();

    [Export]
    public PackedScene UnitToSpawn { get; set; }

    [Export]
    public int AmountPerWave { get; set; } = 5;

    [Export]
    public float WaveCooldown { get; set; } = 5;

    [Export]
    public float WaveCooldownModifier { get; set; } = 1;

    private float                        ModifiedCooldown => WaveCooldown * WaveCooldownModifier;
    public event WaveSpawnedEventHandler WaveSpawned;

    public override void _Ready()
    {
        var sceneTree = GetTree();

        battleScene = sceneTree.CurrentScene;
        player      = battleScene.GetNode<Player>(nameof(Player));
        background  = battleScene.GetNode<TextureRect>("Background");
    }

    public override void _PhysicsProcess(double delta)
    {
        foreach (var enemy in SpawnedEnemies)
            enemy.ChasePlayer();
    }

    public override void _Process(double delta)
    {
        if ((waveTimer += delta) >= ModifiedCooldown)
            SpawnWave<Spider>();
    }

    private void SpawnWave<T>()
            where T : BaseEnemy
    {
        for (var i = 0; i < AmountPerWave; i++)
            SpawnUnit<T>(i);

        waveTimer = 0;

        WaveSpawned?.Invoke();
    }

    private void SpawnUnit<T>(int offset)
            where T : BaseEnemy
    {
        var enemyInstance = UnitToSpawn.Instantiate<T>();

        battleScene.AddChild(enemyInstance);
        SpawnedEnemies.Add(enemyInstance);

        enemyInstance.Position = player.Position + new Vector2(offset * 50, -400);
        enemyInstance.StartChasingPlayer(player);
    }
}