using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Inventory : PanelContainer
{
    private PackedScene inventorySlot;

    private PackedScene InventorySlot
    {
        get
        {
            inventorySlot ??= ResourceLoader.Load<PackedScene>("res://Scenes/Ui/inventory_slot.tscn");

            return inventorySlot;
        }
    }

    [Export] public int SlotAmount { get; set; }

    public override void _Ready()
    {
        var itemGrid = GetNode<GridContainer>("%ItemGrid");

        for (var i = 0; i < SlotAmount; i++)
        {
            var slot = InventorySlot.Instantiate<InventorySlot>();
            itemGrid.AddChild(slot);
        }
    }

    public InventorySlot FindFirstEmptySlot()
    {
        foreach (var slot in this.GetAllChildren<InventorySlot>())
            if (slot.ContainedItem is null)
                return slot;

        return null;
    }
}