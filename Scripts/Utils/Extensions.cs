using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace MonkeSurvivor.Scripts.Utils;

public static class Extensions
{
    public static int GetAttributeUpgradeCost(this BaseUnit unit, string attributeName) => attributeName switch
    {
        nameof(BaseUnit.Vigor) => GetAttributeXpTotal(unit.XpBaseAttribut, unit.Vigor),
        nameof(BaseUnit.Strength) => GetAttributeXpTotal(unit.XpBaseAttribut, unit.Strength),
        nameof(BaseUnit.Dexterity) => GetAttributeXpTotal(unit.XpBaseAttribut, unit.Dexterity),
        nameof(BaseUnit.Intelligence) => GetAttributeXpTotal(unit.XpBaseAttribut, unit.Intelligence),
        _ => throw new ArgumentOutOfRangeException(attributeName)
    };

    public static int GetXpToSpendForLevelUp(this BaseUnit unit) => GetXpTotalForLevelup(unit.Level + 1);

    private static int GetAttributeXpTotal(int xpBase, int inputLevel)
    {
        if (inputLevel == 0)
            return inputLevel;

        return GetAttributeXpTotal(xpBase, inputLevel - 1) + GetAttributeXpDelta(xpBase, inputLevel);
    }

    private static int GetAttributeXpDelta(int xpBase, int inputLevel)
    {
        if (inputLevel == 0)
            return inputLevel;

        return GetAttributeXpDelta(xpBase, inputLevel - 1) + GetAttributeXpDeltaPlus(xpBase, inputLevel);
    }

    private static int GetAttributeXpDeltaPlus(int xpBase, int inputLevel)
    {
        var deltaPlus = inputLevel * xpBase;

        deltaPlus += inputLevel switch
        {
            7 => 150,
            12 or 17 or 22 or 27 or 32 or 37 or 42 => 150,
            _ => 0
        };

        return deltaPlus;
    }

    private static int GetXpTotalForLevelup(int inputLevel)
    {
        if (inputLevel == 0)
            return inputLevel;

        return GetXpTotalForLevelup(inputLevel - 1) + GetHeroXpDelta(inputLevel);
    }

    private static int GetHeroXpDelta(int inputLevel)
    {
        if (inputLevel == 0)
            return inputLevel;

        return GetHeroXpDeltaPlus(inputLevel) + GetHeroXpDelta(inputLevel - 1);
    }

    private static int GetHeroXpDeltaPlus(int inputLevel) => inputLevel switch
    {
        <= 1 => 0,
        <= 2 => 500,
        _ => 1000
    };

    private static int GetXpDelta(int inputLevel, int xpBase)
    {
        if (inputLevel == 0)
            return inputLevel;

        var xpDelta = inputLevel * xpBase + GetXpDelta(inputLevel - 1, xpBase);

        return xpDelta;
    }

    private static int GetXpTotal(int inputLevel, int xpBase)
    {
        if (inputLevel == 0)
            return inputLevel;

        var xpTotal = GetXpDelta(inputLevel, xpBase) + GetXpTotal(inputLevel - 1, xpBase);

        return xpTotal;
    }

    private static int GetGoldDelta(int inputLevel, int xpBase) => (int)(GetXpDelta(inputLevel, xpBase) * 0.9);

    private static int GetGoldTotal(int inputLevel, int xpBase)
    {
        if (inputLevel == 0)
            return inputLevel;

        var goldTotal = GetGoldDelta(inputLevel, xpBase) + GetGoldTotal(inputLevel - 1, xpBase);

        return goldTotal;
    }

    public static T[] GetAllChildren<T>(this Node node)
            where T : Node
    {
        var myChildren = node.GetChildren()
                             .Where(mc => mc is T)
                             .Cast<T>()
                             .ToList();

        var retVal = new List<T>();

        foreach (var child in myChildren)
        {
            var grandChildren = child.GetAllChildren<T>().ToList();

            retVal.AddRange(grandChildren);
        }

        retVal.AddRange(myChildren);

        return retVal.ToArray();
    }
}