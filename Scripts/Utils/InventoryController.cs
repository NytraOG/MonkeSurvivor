using Godot;

namespace MonkeSurvivor.Scripts.Utils;

public partial class InventoryController : Node
{
    private bool   itemsApplied;
    private Player player;
    private bool   playerFound;

    public override void _Process(double delta)
    {
        if (!playerFound)
        {
            player      = GetTree().CurrentScene.GetNode<Player>(nameof(Player));
            playerFound = player != null;
        }
        else if (playerFound && !itemsApplied)
        {
            foreach (var item in StaticMemory.ItemsHeldByPlayer)
            {
                if (item is not null && !item.IsApplied)
                {
                    item.ApplyEffectTo(player);
                    //item.IsApplied = true;  Evtl später besser implementieren, wird aber gerade nicht benötigt. Lasse ich erstmal als reminder so stehen
                }
            }

            itemsApplied = true;
        }
    }
}