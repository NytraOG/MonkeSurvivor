using Godot;
using Godot.Interfaces;
using MonkeSurvivor.Scripts.Items;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ShopCard : PanelContainer,
                                ITooltipObjectContainer
{
    public delegate void ItemBoughtEventHandler(BaseItem boughtItem);

    public delegate void MouseEventHandler(bool entered, ShopCard shopCard);

    public delegate void PurchaseFailedEventHandler(BaseItem boughtItem);

    private Label       itemCostLabel;
    private TextureRect itemImage;
    private Label       itemNameLabel;

    [Export]
    public int CardId { get; set; }

    public bool            Disabled      { get; set; }
    public ITooltipObject? ContainedItem { get; set; }

    //public BaseItem                         Item          { get; set; }
    public event MouseEventHandler          OnMouseEvent;
    public event ItemBoughtEventHandler     ItemBought;
    public event PurchaseFailedEventHandler OnPurchaseFailed;

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

        itemNameLabel.Text = itemToSet.TooltipName;
        itemCostLabel.Text = itemToSet.Price.ToString();
        itemImage.Texture  = itemToSet.ItemImage;

        ContainedItem = itemToSet;
    }

    public void _on_buy_pressed()
    {
        if (ContainedItem is null || ContainedItem is not BaseItem item)
            return;

        var fundsSufficient = StaticMemory.Player.BananasHeld >= item.Price;

        if (fundsSufficient)
        {
            ItemBought?.Invoke(item);

            ContainedItem = null;
            Modulate      = new Color(Modulate, 0);
            Disabled      = true;

            _on_mouse_exited_shopCard();
        }
        else
        {
            OnPurchaseFailed?.Invoke(item);

            var animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));

            animationPlayer.Play("shake");
        }
    }

    public void _on_mouse_entered_shopCard() => OnMouseEvent?.Invoke(true, this);

    public void _on_mouse_exited_shopCard() => OnMouseEvent?.Invoke(false, this);
}