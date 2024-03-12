using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Enemies;

namespace MonkeSurvivor.Scripts.Weapons;

public partial class CoconutGrenade : BaseWeapon
{
    private Node              battleScene;
    private IEnumerable<Node> enemies;
    private Player            player;

    public override void _Ready()
    {
        battleScene = GetTree().CurrentScene;
        player      = battleScene.GetNode<Player>(nameof(Player));

        var unitSpawner = battleScene.GetNode<UnitSpawner>(nameof(UnitSpawner));
        unitSpawner.WaveSpawned += UnitSpawnerOnWaveSpawned;
    }

    private void UnitSpawnerOnWaveSpawned()
        => enemies = battleScene.GetChildren()
                                .Where(c => c.Name == nameof(Spider));

    public override void _Process(double delta) { }
}