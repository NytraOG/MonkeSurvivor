using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ShopCard : PanelContainer
{
    private Label itemCostLabel;
    private Label itemDescriptionLabel;
    private Label itemNameLabel;

    public override void _Ready()
    {
        itemNameLabel        = GetNode<Label>("%ItemName");
        itemDescriptionLabel = GetNode<Label>("%ItemDescription");
        itemCostLabel        = GetNode<Label>("%ItemCost");
    }

    public override void _Process(double delta) { }

    public void _on_buy_pressed() => Modulate = new Color(Modulate, 0);
}