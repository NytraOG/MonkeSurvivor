using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Enemies;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseWeapon : StaticBody2D
{
    public delegate void DamageDealtEventHandler(float totalDamage);

    protected Area2D                  ImpactArea;
    public    DamageDealtEventHandler OnDamageDealt;
    private   Player                  player;
    protected Random                  Rng;
    protected Area2D                  SplashArea;
    protected BaseEnemy               Target;
    public    IEnumerable<BaseEnemy>  Enemies { get; set; }

    [Export]
    public int DamageOnHit { get; set; } = 10;

    [Export]
    public float KnockbackForce { get; set; }

    public HitResult FinalDamage
    {
        get
        {
            Rng ??= new Random();

            player ??= GetTree()
                      .CurrentScene
                      .GetNode<Player>(nameof(Player));

            var roll   = Rng.Next(0, 101);
            var isCrit = player.CriticalHitChance >= roll;

            var dealtDamage = isCrit ? DamageOnHit * (1 + player.CriticalHitDamage / 100) : DamageOnHit;

            return new HitResult(dealtDamage, isCrit);
        }
    }

    [Export]
    public float SwingCooldown { get; set; }

    [Export]
    public bool DealsSplashDamag { get; set; }

    [Export(PropertyHint.Range, "0, 1")]
    public float SplashDamage { get; set; } = 0.75f;

    [Export]
    public float Speed { get; set; } = 50;

    public float TotalDamageDealt { get; set; }
    public int   EnemyCount       { get; set; }

    public override void _PhysicsProcess(double delta) => ExecuteBehaviour(delta);

    protected virtual List<Node2D> GetOverlappingBodies()
    {
        ImpactArea ??= GetNode<Area2D>("%ImpactArea");

        var overlappingBodies = ImpactArea.GetOverlappingBodies()
                                          .Where(b => b.Name != nameof(Player))
                                          .ToList();
        
        return overlappingBodies;
    }

    protected abstract void ExecuteBehaviour(double delta);

    public void DealDamageTo(BaseEnemy enemy, float multiplier = 1)
    {
        var damage = FinalDamage.DamageDealt * multiplier;
        TotalDamageDealt    += damage;
        enemy.HealthCurrent -= damage;
        enemy.InstatiateFloatingCombatText((int)damage, enemy.Position, FinalDamage.IsCritical, false);
    }

    protected virtual void ExecuteAttack(Node node)
    {
        if (node is not BaseEnemy enemy)
            return;

        DealDamageTo(enemy);

        if (DealsSplashDamag)
            DealSplashDamageAround(enemy);

        OnDamageDealt?.Invoke(TotalDamageDealt);
    }

    protected virtual BaseEnemy FindRandomTargetOrDefault()
    {
        if (Target is not null)
            return Target;

        var eligebleTargets = Enemies?.Where(e => !e.IsDead)
                                      .ToList();

        if (eligebleTargets == null)
            return null;

        EnemyCount =   eligebleTargets.Count;
        Rng        ??= new Random();

        var randomNumber = Rng.Next(EnemyCount);

        if (eligebleTargets.Count == EnemyCount)
            Target = eligebleTargets[randomNumber];

        return Target;
    }

    protected virtual BaseEnemy FindClosestTargetOrDefault()
    {
        if (Enemies is null || !Enemies.Any())
            return null;

        var kek = Enemies.Select(e => new
        {
            e,
            e.Position
        });

        kek = kek.OrderBy(e => (e.Position - Position).Length());

        return kek.FirstOrDefault()?.e;
    }

    protected void DealSplashDamageAround(BaseEnemy enemy)
    {
        SplashArea ??= GetNode<Area2D>("SplashArea");

        var overlappingBodies = SplashArea.GetOverlappingBodies().Where(b => b.Name != nameof(Player));

        foreach (var hitEnemy in overlappingBodies.Where(b => b is BaseEnemy).Cast<BaseEnemy>())
        {
            if (hitEnemy != enemy)
                DealDamageTo(hitEnemy, SplashDamage);
        }
    }
}