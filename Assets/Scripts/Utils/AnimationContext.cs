using System;
using UnityEngine;

public class AnimationContext : MonoBehaviour
{
    public event Action AttackAnimationEvent;

    public void OnAttackAnimationEvent()
    {
        // Debug.Log("Animation Attack");
        AttackAnimationEvent?.Invoke();
    }
}
