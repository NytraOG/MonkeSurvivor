using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using MonkeSurvivor.Scripts.Ui;

namespace MonkeSurvivor.Scripts;

public abstract partial class BaseUnit : CharacterBody2D,
                                         INotifyPropertyChanged
{
    private float healthCurrent;
    public  int   Level { get; set; }

    [Export]
    public float HealthMaximum { get; set; } = 12;

    public float HealthCurrent
    {
        get => healthCurrent;
        set
        {
            if (SetField(ref healthCurrent, value))
                OnPropertyChanged();
        }
    }

    [Export]
    public int XpBaseAttribut { get; set; } = 50;

    //Attributes
    [Export]
    public int Vigor { get; set; } = 1;

    [Export]
    public int Strength { get; set; } = 1;

    [Export]
    public int Dexterity { get; set; } = 1;

    [Export]
    public int Intelligence { get; set; } = 1;

    //Secondary Stats

    //Defense
    [Export]
    public float IncreasedDamagereduction { get; set; }

    [Export]
    public float DecreasedDamagereduction { get; set; }

    [Export]
    public float IncreasedLeech { get; set; }

    [Export]
    public float DecreasedLeech { get; set; }

    [Export]
    public float IncreasedHealth { get; set; }

    [Export]
    public float DecreasedHealth { get; set; }

    [Export]
    public float IncreasedDodgeChance { get; set; }

    [Export]
    public float DecreasedDodgeChance { get; set; }

    [Export]
    public float IncreasedHealthregeneration { get; set; }

    [Export]
    public float DecreasedHealthregeneration { get; set; }

    //Offense
    [Export]
    public float IncreasedDamage { get; set; }

    [Export]
    public float DecreasedDamage { get; set; }

    [Export]
    public float IncreasedFlatDamage { get; set; }

    [Export]
    public float DecreasedFlatDamage { get; set; }

    [Export]
    public float IncreasedAttackspeed { get; set; }

    [Export]
    public float DecreasedAttackspeed { get; set; }

    [Export]
    public float IncreasedRange { get; set; }

    [Export]
    public float DecreasedRange { get; set; }

    public PackedScene                       FloatingCombatText => ResourceLoader.Load<PackedScene>("res://Scenes/floating_combat_text.tscn");
    public bool                              IsDead             => HealthCurrent <= 0;
    public event PropertyChangedEventHandler PropertyChanged;

    public override void _Draw()
    {
        base._Draw();

        HealthCurrent   =  HealthMaximum;
        PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(HealthCurrent))
            return;
    }

    public override void _Process(double delta)
    {
        if (IsDead)
            DieProperly();
    }

    protected virtual void DieProperly() => QueueFree();

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public virtual void InstatiateFloatingCombatText(int value, Vector2 spawnPosition, bool isCritical, bool isHeal)
    {
        try
        {
            var floatingCombatTextInstance = FloatingCombatText.Instantiate<FloatingCombatText>();
            floatingCombatTextInstance.Display  = floatingCombatTextInstance.GetNode<Label>("Label");
            floatingCombatTextInstance.Value    = value;
            floatingCombatTextInstance.Position = spawnPosition;

            if (isHeal)
            {
                floatingCombatTextInstance.Display.AddThemeColorOverride("font_color", Colors.LimeGreen);

                if (isCritical)
                {
                    floatingCombatTextInstance.Display.AddThemeColorOverride("font_color", Colors.DarkGreen);
                    floatingCombatTextInstance.Display.AddThemeFontSizeOverride("font_size", 42);
                }

                floatingCombatTextInstance.Display.Text = value.ToString();
            }
            else
            {
                floatingCombatTextInstance.Display.Text = value <= 0 ? "Miss" : value.ToString();

                if (isCritical)
                {
                    floatingCombatTextInstance.Display.AddThemeColorOverride("font_color", Colors.DarkOrange);
                    floatingCombatTextInstance.Display.AddThemeFontSizeOverride("font_size", 42);
                }
            }

            floatingCombatTextInstance.Show();

            GetTree()
                   .CurrentScene
                   .AddChild(floatingCombatTextInstance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}