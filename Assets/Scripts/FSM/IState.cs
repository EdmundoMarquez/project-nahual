using UnityEngine;

namespace ProjectNahual.FSM
{
    public interface IState
    {
        void Awake();
        void Start();
        void Tick();
        void Stop();
    }
}