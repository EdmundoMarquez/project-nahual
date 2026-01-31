using System;
using UnityEngine;

namespace ProjectNahual.Input
{
    public interface IPlayerInput
    {
        Vector2 Move { get; }
        Vector2 Look { get; }
        event Action JumpPressed;
        event Action CrouchPressed;
        bool SprintHold { get; }
    }
}
