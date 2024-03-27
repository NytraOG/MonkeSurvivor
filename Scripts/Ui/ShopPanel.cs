using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ShopPanel : PanelContainer
{
    public override void _Ready() { }

    public override void _Process(double delta) { }

    public ShopCard[] GetShopCards()
    {
        var boxContainer = GetNode<MarginContainer>(nameof(MarginContainer)).GetNode<HBoxContainer>(nameof(HBoxContainer));
        var cards        = boxContainer.GetAllChildren<ShopCard>().ToArray();

        return cards;
    }
}