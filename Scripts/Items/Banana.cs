namespace MonkeSurvivor.Scripts.Items;

public partial class Banana : BaseItem
{
    public override string GetTooltipDescription()
    {
        return "Mjam mjam, lecker Banane.";
    }

    public override void ApplyEffectTo(Player player)
    {
    }

    public override void DeductEffectFrom(Player player)
    {
    }
}