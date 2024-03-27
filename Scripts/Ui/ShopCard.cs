using Godot;
using MonkeSurvivor.Scripts.Items;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ShopCard : PanelContainer
{
    public delegate void MouseEventHandler(bool entered, ShopCard shopCard);

    private Label       itemCostLabel;
    private TextureRect itemImage;
    private Label       itemNameLabel;

    [Export]
    public int CardId { get; set; }

    public bool                    Disabled { get; set; }
    public BaseItem                Item     { get; set; }
    public event MouseEventHandler OnMouseEvent;

    public override void _Ready() => EnsureNodesExist();

    private void EnsureNodesExist()
    {
        itemNameLabel ??= GetNode<Label>("%ItemName");
        itemCostLabel ??= GetNode<Label>("%ItemCost");
        itemImage     ??= GetNode<TextureRect>("%ItemImage");
    }

    public void SetItem(BaseItem itemToSet)
    {
        EnsureNodesExist();

        itemNameLabel.Text = itemToSet.Displayname;
        itemCostLabel.Text = itemToSet.Price.ToString();
        itemImage.Texture  = itemToSet.ItemImage;

        Item = itemToSet;
    }

    public void _on_buy_pressed()
    {
        Modulate = new Color(Modulate, 0);
        Disabled = true;

        _on_mouse_exited_shopCard();
    }

    public void _on_mouse_entered_shopCard() => OnMouseEvent?.Invoke(true, this);

    public void _on_mouse_exited_shopCard() => OnMouseEvent?.Invoke(false, this);
}