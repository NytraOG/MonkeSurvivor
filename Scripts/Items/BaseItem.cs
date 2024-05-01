using Godot;
using MonkeSurvivor.Scripts.Interfaces;

namespace MonkeSurvivor.Scripts.Items;

public abstract partial class BaseItem : Node2D,
                                         ITooltipConsumable
{
    [Export] public CompressedTexture2D ItemImage { get; set; }

    [Export] public int Price { get; set; }
    public bool IsApplied { get; set; }

    [Export] public string TooltipName { get; set; }

    public abstract string GetTooltipDescription();
    public abstract void ApplyEffectTo(Player player);
    public abstract void DeductEffectFrom(Player player);
}