using System.Globalization;
using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class CharacterSheet : PanelContainer
{
    public delegate void AttributeRaisedEventHandler(string attributeName);

    private Label                            attackspeedValue;
    private Label                            criticalDamageValue;
    private Label                            criticalHitValue;
    private Label                            damagereductionValue;
    private Label                            dexterityLabel;
    private Label                            flatDamageValue;
    private Label                            flatHealthValue;
    private Label                            healthregenerationValue;
    private Label                            increasedDamageValue;
    private Label                            increasedHealthValue;
    private Label                            intelligenceLabel;
    private Label                            leechValue;
    private Label                            rangeValue;
    private Label                            strengthLabel;
    private Label                            vigorLabel;
    public event AttributeRaisedEventHandler OnAttributeRaised;

    public override void _Ready()
    {
        vigorLabel              ??= GetNode<Label>("%VigorValue");
        strengthLabel           ??= GetNode<Label>("%StrengthValue");
        dexterityLabel          ??= GetNode<Label>("%DexterityValue");
        intelligenceLabel       ??= GetNode<Label>("%IntelligenceValue");
        increasedDamageValue    ??= GetNode<Label>("%IncreasedDamageValue");
        flatDamageValue         ??= GetNode<Label>("%FlatDamageValue");
        attackspeedValue        ??= GetNode<Label>("%AttackspeedValue");
        criticalHitValue        ??= GetNode<Label>("%CriticalHitValue");
        criticalDamageValue     ??= GetNode<Label>("%CriticalDamageValue");
        rangeValue              ??= GetNode<Label>("%RangeValue");
        damagereductionValue    ??= GetNode<Label>("%DamagereductionValue");
        leechValue              ??= GetNode<Label>("%LeechValue");
        increasedHealthValue    ??= GetNode<Label>("%IncreasedHealthValue");
        flatHealthValue         ??= GetNode<Label>("%FlatHealthValue");
        healthregenerationValue ??= GetNode<Label>("%HealthregenerationValue");

        vigorLabel.Text        = StaticMemory.Vigor.ToString();
        strengthLabel.Text     = StaticMemory.Strength.ToString();
        dexterityLabel.Text    = StaticMemory.Dexterity.ToString();
        intelligenceLabel.Text = StaticMemory.Intelligence.ToString();
    }

    public void SetDisplayedValues(Player player)
    {
        if (player is null)
            return;

        SetDisplayedValue(increasedDamageValue, player.FinalDamage, "%IncreasedDamageValue");
        SetDisplayedValue(flatDamageValue, player.FinalFlatDamage, "%FlatDamageValue");
        SetDisplayedValue(attackspeedValue, player.FinalAttackspeed, "%AttackspeedValue");
        SetDisplayedValue(criticalHitValue, player.CriticalHitChance, "%CriticalHitValue");
        SetDisplayedValue(criticalDamageValue, player.CriticalHitDamage, "%CriticalDamageValue");
        SetDisplayedValue(rangeValue, player.FinalRange, "%RangeValue");
        SetDisplayedValue(damagereductionValue, player.FinalDamagereduction, "%DamagereductionValue");
        SetDisplayedValue(leechValue, player.FinalLeech, "%LeechValue");
        SetDisplayedValue(increasedHealthValue, player.FinalHealth, "%IncreasedHealthValue");
        SetDisplayedValue(flatHealthValue, player.FinalHealthFlat, "%FlatHealthValue");
        SetDisplayedValue(healthregenerationValue, player.FinalHealthregeneration, "%HealthregenerationValue");
    }

    private void SetDisplayedValue(Label label, float value, string labelUniqueName)
    {
        label ??= GetNode<Label>(labelUniqueName);

        if(label is null)
            return;

        if (value > 0)
            label.AddThemeColorOverride("font_color", Colors.LawnGreen);
        else if (value < 0)
            label.AddThemeColorOverride("font_color", Colors.Red);

        label.Text = value.ToString("N0", CultureInfo.CurrentCulture);
    }

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