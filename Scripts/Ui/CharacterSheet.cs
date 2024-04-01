using System.Globalization;
using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class CharacterSheet : PanelContainer
{
    public delegate void AttributeRaisedEventHandler(string attributeName);

    private Label attackspeedValue;
    private Label criticalDamageValue;
    private Label criticalHitValue;
    private Label damagereductionValue;

    private Label dexterityLabel;
    private Label flatDamageValue;
    private Label flatHealthValue;
    private Label increasedDamageValue;
    private Label increasedHealthValue;
    private Label intelligenceLabel;
    private Label leechValue;
    private Label rangeValue;
    private Label strengthLabel;
    private Label vigorLabel;
    private Label healthregenerationValue;
    public event AttributeRaisedEventHandler OnAttributeRaised;

    public override void _Ready()
    {
        vigorLabel = GetNode<Label>("%VigorValue");
        strengthLabel = GetNode<Label>("%StrengthValue");
        dexterityLabel = GetNode<Label>("%DexterityValue");
        intelligenceLabel = GetNode<Label>("%IntelligenceValue");
        increasedDamageValue = GetNode<Label>("%IncreasedDamageValue");
        flatDamageValue = GetNode<Label>("%FlatDamageValue");
        attackspeedValue = GetNode<Label>("%AttackspeedValue");
        criticalHitValue = GetNode<Label>("%CriticalHitValue");
        criticalDamageValue = GetNode<Label>("%CriticalDamageValue");
        rangeValue = GetNode<Label>("%RangeValue");
        damagereductionValue = GetNode<Label>("%DamagereductionValue");
        leechValue = GetNode<Label>("%LeechValue");
        increasedHealthValue = GetNode<Label>("%IncreasedHealthValue");
        flatHealthValue = GetNode<Label>("%FlatHealthValue");
        healthregenerationValue = GetNode<Label>("%HealthregenerationValue");

        vigorLabel.Text = StaticMemory.Vigor.ToString();
        strengthLabel.Text = StaticMemory.Strength.ToString();
        dexterityLabel.Text = StaticMemory.Dexterity.ToString();
        intelligenceLabel.Text = StaticMemory.Intelligence.ToString();
    }

    public void SetDisplayedValues(Player player)
    {
        if(player is null)
            return;
        
        increasedDamageValue.Text = player.FinalDamage.ToString("N1", CultureInfo.CurrentCulture);
        flatDamageValue.Text = player.FinalFlatDamage.ToString("N1", CultureInfo.CurrentCulture);
        attackspeedValue.Text = player.FinalAttackspeed.ToString("N1", CultureInfo.CurrentCulture);
        criticalHitValue.Text = player.CriticalHitChance.ToString("N1", CultureInfo.CurrentCulture);
        criticalDamageValue.Text = player.CriticalHitDamage.ToString("N1", CultureInfo.CurrentCulture);
        rangeValue.Text = player.FinalRange.ToString("N1", CultureInfo.CurrentCulture);
        damagereductionValue.Text = player.FinalDamagereduction.ToString("N1", CultureInfo.CurrentCulture);
        leechValue.Text = player.FinalLeech.ToString("N1", CultureInfo.CurrentCulture);
        increasedHealthValue.Text = player.FinalHealth.ToString("N1", CultureInfo.CurrentCulture);
        flatHealthValue.Text = player.FinalHealthFlat.ToString("N1", CultureInfo.CurrentCulture);
        healthregenerationValue.Text = player.FinalHealthregeneration.ToString("N1", CultureInfo.CurrentCulture);
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