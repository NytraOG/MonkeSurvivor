using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Enemies;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseWeapon : StaticBody2D
{
    public delegate void DamageDealtEventHandler(float totalDamage);

    private Area2D                  impactArea;
    public  DamageDealtEventHandler OnDamageDealt;
    private Area2D                  splashArea;
    private BaseEnemy               target;
    public  IEnumerable<BaseEnemy>  Enemies { get; set; }

    [Export]
    public int DamageOnHit { get; set; } = 10;

    [Export]
    public float SwingCooldown { get; set; }

    [Export(PropertyHint.Range, "0, 1")]
    public float SplashDamage { get; set; } = 0.75f;

    [Export]
    public float Speed { get; set; } = 50;

    public float TotalDamageDealt { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        ExecuteBehaviour();

        splashArea ??= GetNode<Area2D>("SplashArea");
        impactArea ??= GetNode<Area2D>("ImpactArea");

        var overlappingBodies = impactArea.GetOverlappingBodies()
                                          .Where(b => b.Name != nameof(Player))
                                          .ToList();

        if (!overlappingBodies.Any()) return;

        var luckyBastard = overlappingBodies.First();

        ExecuteAttack(luckyBastard);
    }

    protected abstract void ExecuteBehaviour();

    protected BaseEnemy FindTargetOrDefault()
    {
        if (target is not null)
            return target;

        var eligebleTargets = Enemies?.Where(e => !e.IsDead)
                                      .ToList();

        if (eligebleTargets == null)
            return null;

        var enemyCount   = eligebleTargets.Count();
        var rng          = new Random();
        var randomNumber = rng.Next(enemyCount);

        if(eligebleTargets.Count == enemyCount)
            target = eligebleTargets[randomNumber];

        return target;
    }

    public void DealDamageTo(BaseEnemy enemy, float multiplier = 1)
    {
        var damage = DamageOnHit * multiplier;
        TotalDamageDealt    += damage;
        enemy.HealthCurrent -= damage;
        enemy.InstatiateFloatingCombatText((int)damage, enemy.Position);
    }

    private void ExecuteAttack(Node node)
    {
        if (node is not BaseEnemy enemy)
            return;

        DealDamageTo(enemy);
        DealSplashDamageAround(enemy);

        OnDamageDealt?.Invoke(TotalDamageDealt);

        QueueFree();
    }

    private void DealSplashDamageAround(BaseEnemy enemy)
    {
        var overlappingBodies = splashArea.GetOverlappingBodies().Where(b => b.Name != nameof(Player));

        foreach (var hitEnemy in overlappingBodies.Where(b => b is BaseEnemy).Cast<BaseEnemy>())
        {
            if (hitEnemy != enemy)
                DealDamageTo(hitEnemy, SplashDamage);
        }
    }
}