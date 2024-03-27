using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ItemTooltip : BaseTooltip
{
    public override void _Ready()
    {
        var shopScene = GetTree().CurrentScene;
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
            SetPositionByShopCard(shopcard);
        else
            Position = new Vector2(-900, -900);
    }

    public override void _Process(double delta) { }

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