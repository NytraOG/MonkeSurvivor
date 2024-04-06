namespace MonkeSurvivor.Scripts.Items;

public partial class Cheese : BaseItem
{
    private float healthRegValue = 0.5f;

    public override string GetTooltipDescription()
    {
        return "Dieser Käse stinkt nicht.";
    }

    public override void ApplyEffectTo(Player player)
    {
        player.IncreasedHealthregeneration += healthRegValue;
    }

    public override void DeductEffectFrom(Player player)
    {
        player.IncreasedHealthregeneration -= healthRegValue;
    }
}