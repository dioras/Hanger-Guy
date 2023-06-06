using System;

namespace _1.Scripts.Game
{
    [Flags] 
    public enum GameStateEnum
    {
        None        = 0,
        Init        = 1,
        Play        = 2,
        Result      = 4,
        Lose        = 8,
        Continue    = 16
    }
}