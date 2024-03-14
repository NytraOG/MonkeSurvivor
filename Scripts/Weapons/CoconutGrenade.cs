namespace MonkeSurvivor.Scripts.Weapons;

public partial class CoconutGrenade : BaseWeapon
{
    public override void _Process(double delta) { }

    protected override void ExecuteBehaviour()
    {
        var target = FindTargetOrDefault();

        if (target is null)
            return;

        var direction = (target.Position - Position).Normalized();
        var collisions = MoveAndCollide(direction * 300);
    }
}