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
        StaticMemory.Vigor++;
        vigorLabel.Text = StaticMemory.Vigor.ToString();
        OnAttributeRaised?.Invoke(nameof(BaseUnit.Vigor));
    }

    public void _on_Strength_Raised()
    {
        StaticMemory.Strength++;
        strengthLabel.Text = StaticMemory.Strength.ToString();
        OnAttributeRaised?.Invoke(nameof(BaseUnit.Strength));
    }

    public void _on_Dexterity_Raised()
    {
        StaticMemory.Dexterity++;
        dexterityLabel.Text = StaticMemory.Dexterity.ToString();
        OnAttributeRaised?.Invoke(nameof(BaseUnit.Dexterity));
    }

    public void _on_Intelligence_Raised()
    {
        StaticMemory.Intelligence++;
        intelligenceLabel.Text = StaticMemory.Intelligence.ToString();
        OnAttributeRaised?.Invoke(nameof(BaseUnit.Intelligence));
    }
}