using System;
using UnityEngine;

namespace ProjectNahual.Input
{
    public interface IMenuInput
    {
        event Action PausePressed;
        event Action BackPressed;
        event Action AcceptPressed;
    }
}
