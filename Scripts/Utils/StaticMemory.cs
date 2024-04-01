using System.Collections.Generic;
using MonkeSurvivor.Scripts.Items;

namespace MonkeSurvivor.Scripts.Utils;

public static class StaticMemory
{
    public static bool AlreadyReadied { get; set; }
    public static int WaveNumber { get; set; } = 1;
    public static Player Player { get; set; }
    public static float CurrentHealth { get; set; }
    public static int HeldMoney { get; set; }
    public static int Vigor { get; set; } = 1;
    public static int Strength { get; set; } = 1;
    public static int Dexterity { get; set; } = 1;
    public static int Intelligence { get; set; } = 1;
    public static List<BaseItem> ItemsHeldByPlayer { get; set; } = new();
}