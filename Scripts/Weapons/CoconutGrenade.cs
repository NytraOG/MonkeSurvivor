using System.Collections.Generic;
using System.Linq;
using Godot;

namespace MonkeSurvivor.Scripts.Weapons;

public partial class CoconutGrenade : BaseWeapon
{
    private Node              battleScene;
    private IEnumerable<Node> enemies;
    private Player            player;
    private float             swingTimer;

    [Export]
    public int DamageOnHit { get; set; } = 10;

    [Export]
    public float SwingCooldown { get; set; }

    public override void _Ready()
    {
        battleScene = GetTree().CurrentScene;
        player      = battleScene.GetNode<Player>(nameof(Player));

        var unitSpawner = battleScene.GetNode<UnitSpawner>(nameof(UnitSpawner));
        unitSpawner.WaveSpawned += UnitSpawnerOnWaveSpawned;
    }

    private void UnitSpawnerOnWaveSpawned()
        => enemies = battleScene.GetChildren()
                                .Where(c => c.Name == nameof(Enemies.Spider));

    public override void _Process(double delta)
    {
        if (swingTimer >= SwingCooldown)
        {
            //ATTACK!!!!!

            swingTimer = 0;
        }
        else
            swingTimer += (float)delta;
    }
}