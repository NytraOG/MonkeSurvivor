using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Enemies;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseWeapon : RigidBody2D
{
    public IEnumerable<BaseEnemy> Enemies { get; set; }
    [Export] public int DamageOnHit { get; set; } = 10;

    [Export] public float SwingCooldown { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        ExecuteBehaviour();
    }

    protected abstract void ExecuteBehaviour();

    protected BaseEnemy FindTargetOrDefault()
    {
        return Enemies?.Where(e => !e.IsDead).FirstOrDefault();
    }

    public void DealDamageTo(BaseEnemy enemy)
    {
        enemy.HealthCurrent -= DamageOnHit;
    }

    public void _on_body_entered(Node node)
    {
        if (node is BaseEnemy enemy)
        {
            DealDamageTo(enemy);
            var areaNode = GetNode<Area2D>(nameof(Area2D));
            var overlappingBodies = areaNode.GetOverlappingBodies().Where(b => b.Name != nameof(Player));
            //ContactMonitor = false;
            foreach (var hitEnemy in overlappingBodies.Cast<BaseEnemy>())
                hitEnemy.HealthCurrent -= (float)DamageOnHit * 2 / 3;
            QueueFree();
        }
    }
}