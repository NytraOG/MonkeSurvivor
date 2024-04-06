using System;
using System.Linq;
using MonkeSurvivor.Scripts.Enemies;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseRangedWeapon : BaseWeapon
{
    protected BaseEnemy FindTargetOrDefault()
    {
        if (Target is not null)
            return Target;

        var eligebleTargets = Enemies?.Where(e => !e.IsDead)
            .ToList();

        if (eligebleTargets == null)
            return null;

        EnemyCount = eligebleTargets.Count();
        Rng ??= new Random();
        var randomNumber = Rng.Next(EnemyCount);

        if (eligebleTargets.Count == EnemyCount)
            Target = eligebleTargets[randomNumber];

        return Target;
    }
}