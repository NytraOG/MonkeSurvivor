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

    private void ShopCardOnOnMouseEvent(bool entered, ShopCard shopcard) { }

    public override void _Process(double delta) { }

    public void SetPositionByShopCard(ShopCard shopCard)
    {
        var xPosition = shopCard.Position.X + Size.X;

        // Position = shopCardId switch
        // {
        //     1 => new Vector2(531, Position.Y),
        //     2 => new Vector2(182, Position.Y),
        //     3 => new Vector2(456, Position.Y),
        //     4 => new Vector2(730, Position.Y),
        //     5 => new Vector2(1003, Position.Y),
        //     6 => new Vector2(1277, Position.Y),
        //     _ => throw new ArgumentOutOfRangeException(nameof(shopCardId), shopCardId, null)
        // };
    }
}