using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class RessourceIndicator : PanelContainer
{
    private Label amountLabel;

    public override void _Ready()
    {
        amountLabel = GetNode<Label>("%Amount");

        SetBananaAmount();
    }

    public void SetBananaAmount()
    {
        var result      = string.Empty;
        var newXpValues = StaticMemory.HeldBananas.ToString().ToCharArray();

        result += newXpValues.Length switch
        {
            1 => "00",
            2 => "0",
            _ => string.Empty
        };

        foreach (var xpValue in newXpValues)
        {
            if (int.TryParse(xpValue.ToString(), out var parsedInteger))
                result += $"{parsedInteger}";
        }

        amountLabel.Text = result + "x";
    }
}