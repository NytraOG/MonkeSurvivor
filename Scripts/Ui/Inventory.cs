using Godot;
using MonkeSurvivor.Scripts.Items;
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

    public void SetItem(BaseItem item)
    {
        var slot = FindFirstEmptySlot();
        slot?.SetItem(item);
    }

    private InventorySlot FindFirstEmptySlot()
    {
        var slots = GetNode<GridContainer>("%ItemGrid").GetAllChildren<InventorySlot>();
        
        foreach (var slot in slots)
            if (slot.ContainedItem is null)
                return slot;

        return null;
    }
}