using Godot;

namespace MonkeSurvivor.Scripts.Interfaces;

public interface ITooltipConsumable
{
    [Export]
    public string TooltipName { get; set; }
    public string GetTooltipDescription();
}