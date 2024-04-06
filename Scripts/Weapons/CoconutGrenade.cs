namespace MonkeSurvivor.Scripts.Weapons;

public partial class CoconutGrenade : BaseRangedWeapon
{
    public override void _Process(double delta) { }

    protected override void ExecuteBehaviour()
    {
        var target = FindTargetOrDefault();

        if (target is null || target.IsDead)
            return;

        var direction = (target.Position - Position).Normalized();

        MoveAndCollide(direction * Speed);
    }
}