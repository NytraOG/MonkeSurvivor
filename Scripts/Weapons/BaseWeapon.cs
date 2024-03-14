using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Enemies;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseWeapon : RigidBody2D
{
    public IEnumerable<BaseEnemy> Enemies { get; set; }

    [Export]
    public int DamageOnHit { get; set; } = 10;

    [Export]
    public float SwingCooldown { get; set; }

    [Export(PropertyHint.Range, "0, 1")]
    public float SplashDamage { get; set; } = 0.75f;

    [Export]
    public float Speed { get; set; } = 50;

    public override void _PhysicsProcess(double delta) => ExecuteBehaviour();

    protected abstract void ExecuteBehaviour();

    protected BaseEnemy FindTargetOrDefault() => Enemies?.Where(e => !e.IsDead).FirstOrDefault();

    public void DealDamageTo(BaseEnemy enemy) => enemy.HealthCurrent -= DamageOnHit;

    public void _on_body_entered(Node node)
    {
        if (node is not BaseEnemy enemy)
            return;

        DealDamageTo(enemy);
        var areaNode          = GetNode<Area2D>(nameof(Area2D));
        var overlappingBodies = areaNode.GetOverlappingBodies().Where(b => b.Name != nameof(Player));

        //ContactMonitor = false;
        foreach (var hitEnemy in overlappingBodies.Cast<BaseEnemy>())
            hitEnemy.HealthCurrent -= DamageOnHit * SplashDamage;
        QueueFree();
    }
}