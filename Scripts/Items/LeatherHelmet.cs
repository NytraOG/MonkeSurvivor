using Godot;

namespace MonkeSurvivor.Scripts.Items;

public partial class LeatherHelmet : BaseItem
{
    [Export] public float IncreasedDamagereduction { get; set; }

    public override string GetTooltipDescription()
    {
        return "Passt gut und dämpft leichte Treffer";
    }

    public override void ApplyEffectTo(Player player)
    {
        player.IncreasedDamagereduction += IncreasedDamagereduction;
    }

    public override void DeductEffectFrom(Player player)
    {
        player.IncreasedDamagereduction -= IncreasedDamagereduction;
    }
}