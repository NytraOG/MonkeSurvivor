using Godot;

namespace MonkeSurvivor.Scripts.Items;

public abstract partial class BaseItem : Node2D
{
    [Export] public CompressedTexture2D ItemImage { get; set; }

    [Export] public int Price { get; set; }

    [Export] public string Displayname { get; set; }

    public abstract string GetTooltipDescription();
    public abstract void ApplyEffectTo(Player player);
    public abstract void DeductEffectFrom(Player player);
}