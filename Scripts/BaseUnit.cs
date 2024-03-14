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

    public float HealthCurrent
    {
        get => healthCurrent;
        set
        {
            if (SetField(ref healthCurrent, value))
                OnPropertyChanged();
        }
    }

    [Export] public float HealthMaximum { get; set; } = 100;

    public PackedScene FloatingCombatText => ResourceLoader.Load<PackedScene>("res://Scenes/floating_combat_text.tscn");
    public bool IsDead => HealthCurrent <= 0;
    public event PropertyChangedEventHandler PropertyChanged;

    public override void _Draw()
    {
        base._Draw();

        HealthCurrent = HealthMaximum;
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

    protected virtual void DieProperly()
    {
        QueueFree();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public virtual void InstatiateFloatingCombatText(int receivedDamage, Vector2 spawnPosition)
    {
        try
        {
            var floatingCombatTextInstance = FloatingCombatText.Instantiate<FloatingCombatText>();

            floatingCombatTextInstance.Display = floatingCombatTextInstance.GetNode<Label>("Label");
            floatingCombatTextInstance.Display.Text = receivedDamage <= 0 ? "Miss" : receivedDamage.ToString();
            floatingCombatTextInstance.Damage = receivedDamage;
            floatingCombatTextInstance.Position = spawnPosition;
            floatingCombatTextInstance.Show();

            // switch (hitResult)
            // {
            //     case HitResult.Good:
            //         floatingCombatTextInstance.Display.AddThemeColorOverride("font_color", Colors.Orange);
            //         floatingCombatTextInstance.Display.Text += "!";
            //         break;
            //     case HitResult.Critical:
            //         floatingCombatTextInstance.Display.AddThemeColorOverride("font_color", Colors.Red);
            //         floatingCombatTextInstance.Display.Text += "!!";
            //         break;
            // }

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