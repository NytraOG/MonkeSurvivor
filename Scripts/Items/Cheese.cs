namespace MonkeSurvivor.Scripts.Items;

public partial class Cheese : BaseItem
{
    public override void _Ready() { }

    public override void _Process(double delta) { }

    public override string GetTooltipDescription() => "Dieser Käse stinkt nicht.";
    public override void ApplyEffectTo(Player player)
    {
        
    }

    public override void DeductEffectFrom(Player player)
    {
        
    }
}