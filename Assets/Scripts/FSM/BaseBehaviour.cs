using UnityEngine;
using UnityEngine.AI;

namespace ProjectNahual.FSM
{
    public class BaseBehaviour : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent agent;
        [SerializeField] protected Animator animator;
        [SerializeField] protected Collider patrolArea;
        protected StateMachine<IState> stateMachine;
        protected bool canTick = false;
        public virtual void Init() => canTick = true;

        public virtual void Tick()
        {
            if(!canTick) return;
            // Update the current state
            stateMachine.Update();
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        public virtual void Resume()
        {
            agent.enabled = true;
        }

        public virtual void Stop()
        {
            agent.enabled = false;
            stateMachine.Clear();
        }

        public virtual void Reset() {}

        public virtual void SetOrigin(Vector3 origin)
        {
            agent.Warp(origin);
        }
    }
}

