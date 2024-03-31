using Godot;
using MonkeSurvivor.Scripts.Items;

namespace MonkeSurvivor.Scripts.Ui;

public partial class InventorySlot : PanelContainer
{
    public delegate void MouseEnteringEventHandler(bool entered, InventorySlot inventorySlot);

    private TextureRect itemImage;
    public BaseItem ContainedItem { get; set; }

    public event MouseEnteringEventHandler MouseEntering;

    public override void _Ready()
    {
        itemImage = GetNode<TextureRect>("%ItemImage");
    }

    public void SetItem(BaseItem item)
    {
        ContainedItem = item;
        itemImage.Texture = item.ItemImage;
    }

    public BaseItem RetrieveItem()
    {
        var item = ContainedItem;
        ContainedItem = null;
        itemImage.Texture = null;

        return item;
    }

    public void _on_mouse_left_InventorySLot()
    {
        if(ContainedItem is not null)
            MouseEntering?.Invoke(false,this);
    }

    public void _on_mouse_entered_InventorySLot()
    {
        if(ContainedItem is not null)
            MouseEntering?.Invoke(true, this);
    }
}