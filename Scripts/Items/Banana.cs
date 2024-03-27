using System;

namespace MonkeSurvivor.Scripts.Items;

public partial class Banana : BaseItem
{
    public override void _Ready() { }

    public override void _Process(double delta) { }

    public override string GetTooltipDescription() => "Mjam mjam, lecker Banane.";
}