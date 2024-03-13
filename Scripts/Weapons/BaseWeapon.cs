using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Enemies;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseWeapon : RigidBody2D
{
    private readonly Node              battleScene;
    protected        IEnumerable<Node> Enemies;
    private          Player            player;

    protected BaseWeapon()
    {
        battleScene = GetTree().CurrentScene;
        player      = battleScene.GetNode<Player>(nameof(Player));

        var unitSpawner = battleScene.GetNode<UnitSpawner>(nameof(UnitSpawner));
        unitSpawner.WaveSpawned += UnitSpawnerOnWaveSpawned;
    }

    [Export]
    public int DamageOnHit { get; set; } = 10;

    [Export]
    public float SwingCooldown { get; set; }

    public abstract BaseEnemy FindTargetOrDefault();

    private void UnitSpawnerOnWaveSpawned()
        => Enemies = battleScene.GetChildren()
                                .Where(c => c.Name == nameof(Spider))
                                .ToList();
}