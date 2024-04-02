using Godot;

namespace MonkeSurvivor.Scripts.Utils;

public partial class InventoryController : Node
{
    private Player player;

    public override void _Ready()
    {
        player = GetTree().CurrentScene.GetNode<Player>(nameof(Player));

        foreach (var item in StaticMemory.ItemsHeldByPlayer)
            if (item is not null && !item.IsApplied)
            {
                item.ApplyEffectTo(player);
                //item.IsApplied = true;  Evtl später besser implementieren, wird aber gerade nicht benötigt. Lasse ich erstmal als reminder so stehen
            }
    }
}