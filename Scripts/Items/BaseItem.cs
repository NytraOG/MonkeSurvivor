using Godot;

namespace MonkeSurvivor.Scripts.Items;

public partial class BaseItem : Node2D
{
    [Export]
    public CompressedTexture2D ItemImage { get; set; }

    [Export]
    public int Price { get; set; }
}