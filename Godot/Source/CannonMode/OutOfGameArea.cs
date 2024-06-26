﻿using Godot;

namespace TicTacGodot.CannonMode;

public partial class OutOfGameArea : Area3D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        body.QueueFree();
    }
}