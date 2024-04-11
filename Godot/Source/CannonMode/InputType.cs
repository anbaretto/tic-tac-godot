using System;
using TicTacToe;

namespace TicTacToe_Godot.CannonMode;

public static class InputType
{
    private static readonly IPlayerInputType PlayerXInputType = new Player1();
    private static readonly IPlayerInputType PlayerOInputType = new Player2();

    public static IPlayerInputType ForPlayer(Player player)
    {
        return player switch
        {
            Player.X => PlayerXInputType,
            Player.O => PlayerOInputType,
            _ => throw new ArgumentOutOfRangeException(nameof(player), player, null)
        };
    }

    public interface IPlayerInputType
    {
        public string CannonFire { get; }
        public string CannonRotateUp { get; }
        public string CannonRotateDown { get; }
        public string CannonRotateLeft { get; }
        public string CannonRotateRight { get; }
    }

    private sealed class Player1 : IPlayerInputType
    {
        public string CannonFire => "Player1CannonFire";
        public string CannonRotateUp => "Player1CannonRotateUp";
        public string CannonRotateDown => "Player1CannonRotateDown";
        public string CannonRotateLeft => "Player1CannonRotateLeft";
        public string CannonRotateRight => "Player1CannonRotateRight";
    }

    private sealed class Player2 : IPlayerInputType
    {
        public string CannonFire => "Player2CannonFire";
        public string CannonRotateUp => "Player2CannonRotateUp";
        public string CannonRotateDown => "Player2CannonRotateDown";
        public string CannonRotateLeft => "Player2CannonRotateLeft";
        public string CannonRotateRight => "Player2CannonRotateRight";

    }
}