using Godot;
using MonkeSurvivor.Scripts.Items;

namespace MonkeSurvivor.Scripts.Ui;

public partial class InventorySlot : PanelContainer
{
    private TextureRect itemImage;
    public BaseItem ContainedItem { get; set; }

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
}