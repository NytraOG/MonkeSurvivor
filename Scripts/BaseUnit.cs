using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;

namespace MonkeSurvivor.Scripts;

public abstract partial class BaseUnit : CharacterBody2D,
                                         INotifyPropertyChanged
{
    private float healthCurrent;

    [Export]
    public float HealthCurrent
    {
        get => healthCurrent;
        set
        {
            if (SetField(ref healthCurrent, value))
                OnPropertyChanged(nameof(IsDead));
        }
    }

    [Export]
    public float HealthMaximum { get; set; } = 100;

    public bool                              IsDead => HealthCurrent <= 0;
    public event PropertyChangedEventHandler PropertyChanged;

    public override void _Draw()
    {
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
            QueueFree();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}