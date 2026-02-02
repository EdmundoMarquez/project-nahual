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
        public virtual void Init() {}

        public virtual void Tick()
        {
            // Update the current state
            stateMachine.Update();
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }
}

