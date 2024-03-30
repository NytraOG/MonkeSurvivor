using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ItemTooltip : BaseTooltip
{
    private RichTextLabel itemNameLabel;
    private RichTextLabel itemDescriptionLabel;

    public override void _Ready()
    {
        var shopScene = GetTree().CurrentScene;

        SubscribeToShopCards(shopScene);

        itemNameLabel        = GetNode<RichTextLabel>("%ItemName");
        itemDescriptionLabel = GetNode<RichTextLabel>("%ItemDescription");
    }

    private void SubscribeToShopCards(Node shopScene)
    {
        var shopPanel = shopScene.GetNode<ShopPanel>("%" + nameof(ShopPanel));
        var shopCards = shopPanel.GetShopCards();

        foreach (var shopCard in shopCards)
        {
            shopCard.OnMouseEvent -= ShopCardOnOnMouseEvent;
            shopCard.OnMouseEvent += ShopCardOnOnMouseEvent;
        }
    }

    private void ShopCardOnOnMouseEvent(bool entered, ShopCard shopcard)
    {
        if (entered && !shopcard.Disabled)
        {
            itemNameLabel.Text = itemNameLabel.Text.Replace("ItemName", $"[u]{shopcard.Item.Displayname}[/u]");

            itemDescriptionLabel.Text = $"{System.Environment.NewLine}" +
                                        $"{shopcard.Item.GetTooltipDescription()}{System.Environment.NewLine}" +
                                        $"{System.Environment.NewLine}";

            SetPositionByShopCard(shopcard);
        }
        else
        {
            Position = new Vector2(-900, -900);

            if(shopcard.Item is not null)
                itemNameLabel.Text = itemNameLabel.Text.Replace($"{shopcard.Item.Displayname}", "ItemName");
        }
    }

    public void SetPositionByShopCard(ShopCard shopCard)
    {
        var xPosition = shopCard.Position.X - Size.X + 10;

        if (xPosition <= 0)
        {
            xPosition = shopCard.Position.X + shopCard.Size.X + 20;
        }

        Position = new Vector2(xPosition, 27);
    }
}