using UnityEngine;
using UnityEngine.AI;

namespace ProjectNahual.FSM
{
    public class AttackState : IState
    {
        private Animator _animator;
        private StateMachine<IState> _stateMachine;
        private Transform _agentTransform;
        private Transform _target;
        private float _waitTimer;
        private float _waitDuration = 3f;
        private bool canTick = false;
        public AttackState(Transform agent, Transform target, Animator animator, StateMachine<IState> stateMachine)
        {
            _agentTransform = agent;
            _target = target;
            _animator = animator;
            _stateMachine = stateMachine;
        }

        public void Awake() => Debug.Log("Awake attack state");

        public void Start()
        {
            Debug.Log("Started attack state");
            _animator.SetTrigger("Attack");
            _agentTransform.LookAt(_target);

            canTick = true;
        }

        public void Tick()
        {
            if(!canTick) return;
            // Debug.Log("Attack state executing...");

            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _waitDuration)
            {
                _waitTimer = 0f;
                _stateMachine.ChangeState("Chase");
            }
        }

        public void Stop()
        {
            canTick = false;
        }
    }
}
