using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class CharacterSheet : PanelContainer
{
    public delegate void AttributeRaisedEventHandler(string attributeName);

    private Label                            dexterityLabel;
    private Label                            intelligenceLabel;
    private Label                            strengthLabel;
    private Label                            vigorLabel;
    public event AttributeRaisedEventHandler OnAttributeRaised;

    public override void _Ready()
    {
        vigorLabel        = GetNode<Label>("%VigorValue");
        strengthLabel     = GetNode<Label>("%StrengthValue");
        dexterityLabel    = GetNode<Label>("%DexterityValue");
        intelligenceLabel = GetNode<Label>("%IntelligenceValue");

        vigorLabel.Text        = StaticMemory.Vigor.ToString();
        strengthLabel.Text     = StaticMemory.Strength.ToString();
        dexterityLabel.Text    = StaticMemory.Dexterity.ToString();
        intelligenceLabel.Text = StaticMemory.Intelligence.ToString();
    }

    public override void _Process(double delta) { }

    public void _on_Vigor_Raised()
    {
        var upgradeCost = StaticMemory.Player.GetAttributeUpgradeCost(nameof(BaseUnit.Vigor));

        StaticMemory.Player.Vigor++;
        StaticMemory.Vigor++;

        vigorLabel.Text = StaticMemory.Vigor.ToString();

        OnAttributeRaised?.Invoke(nameof(BaseUnit.Vigor));
    }

    public void _on_Strength_Raised()
    {
        var upgradeCost = StaticMemory.Player.GetAttributeUpgradeCost(nameof(BaseUnit.Strength));

        StaticMemory.Player.Strength++;
        StaticMemory.Strength++;

        strengthLabel.Text = StaticMemory.Strength.ToString();

        OnAttributeRaised?.Invoke(nameof(BaseUnit.Strength));
    }

    public void _on_Dexterity_Raised()
    {
        var upgradeCost = StaticMemory.Player.GetAttributeUpgradeCost(nameof(BaseUnit.Dexterity));

        StaticMemory.Player.Dexterity++;
        StaticMemory.Dexterity++;

        dexterityLabel.Text = StaticMemory.Dexterity.ToString();

        OnAttributeRaised?.Invoke(nameof(BaseUnit.Dexterity));
    }

    public void _on_Intelligence_Raised()
    {
        var upgradeCost = StaticMemory.Player.GetAttributeUpgradeCost(nameof(BaseUnit.Intelligence));

        StaticMemory.Player.Intelligence++;
        StaticMemory.Intelligence++;

        intelligenceLabel.Text = StaticMemory.Intelligence.ToString();

        OnAttributeRaised?.Invoke(nameof(BaseUnit.Intelligence));
    }
}