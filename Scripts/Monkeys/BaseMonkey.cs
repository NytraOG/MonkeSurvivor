﻿using Godot;

namespace MonkeSurvivor.Scripts.Monkeys;

public abstract partial class BaseMonkey : Node2D
{
    public abstract PackedScene StartingWeapon { get; }
}