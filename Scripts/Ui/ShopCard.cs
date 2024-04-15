using Godot;
using MonkeSurvivor.Scripts.Items;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ShopCard : PanelContainer
{
    public delegate void ItemBoughtEventHandler(BaseItem boughtItem);
    public delegate void PurchaseFailedEventHandler(BaseItem boughtItem);

    public delegate void MouseEventHandler(bool entered, ShopCard shopCard);

    private Label itemCostLabel;
    private TextureRect itemImage;
    private Label itemNameLabel;

    [Export] public int CardId { get; set; }

    public bool Disabled { get; set; }
    public BaseItem Item { get; set; }
    public event MouseEventHandler OnMouseEvent;
    public event ItemBoughtEventHandler ItemBought;
    public event PurchaseFailedEventHandler OnPurchaseFailed;

    public override void _Ready()
    {
        EnsureNodesExist();
    }

    private void EnsureNodesExist()
    {
        itemNameLabel ??= GetNode<Label>("%ItemName");
        itemCostLabel ??= GetNode<Label>("%ItemCost");
        itemImage ??= GetNode<TextureRect>("%ItemImage");
    }

    public void SetItem(BaseItem itemToSet)
    {
        EnsureNodesExist();

        itemNameLabel.Text = itemToSet.Displayname;
        itemCostLabel.Text = itemToSet.Price.ToString();
        itemImage.Texture = itemToSet.ItemImage;

        Item = itemToSet;
    }

    public void _on_buy_pressed()
    {
        if (Item is null)
            return;

        var fundsSufficient = StaticMemory.Player.XpCurrent >= Item.Price;
        
        if(fundsSufficient)
        {
            ItemBought?.Invoke(Item);

            Item = null;
            Modulate = new Color(Modulate, 0);
            Disabled = true;

            _on_mouse_exited_shopCard();
        }
        else
        {
            OnPurchaseFailed?.Invoke(Item);

            var animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
            
            animationPlayer.Play("shake");
        }
    }

    public void _on_mouse_entered_shopCard()
    {
        OnMouseEvent?.Invoke(true, this);
    }

    public void _on_mouse_exited_shopCard()
    {
        OnMouseEvent?.Invoke(false, this);
    }
}