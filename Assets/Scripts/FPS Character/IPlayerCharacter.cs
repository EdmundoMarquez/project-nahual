using UnityEngine;

namespace ProjectNahual.FPCharacter
{
    public interface IPlayerCharacter
    {
        bool Initialized {get;}
        Vector3 Position {get;}
        void Init(MonoBehaviour weaponBehaviour);
        void Reset();
    }
}