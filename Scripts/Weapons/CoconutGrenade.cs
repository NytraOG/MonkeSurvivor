using System.Linq;
using Godot;

namespace MonkeSurvivor.Scripts.Weapons;

public partial class CoconutGrenade : BaseRangedWeapon
{
    protected override void ExecuteBehaviour(double delta)
    {
        var target = FindTargetOrDefault();

        if (target is null || target.IsDead)
            return;

        var direction = (target.Position - Position).Normalized();

        MoveAndCollide(direction * Speed);

        var overlappingBodies = GetOverlappingBodies();

        if (!overlappingBodies.Any()) return;

        var luckyBastard = overlappingBodies.First();

        ExecuteAttack(luckyBastard);
    }

    protected override void ExecuteAttack(Node node)
    {
        base.ExecuteAttack(node);

        QueueFree();
    }
}