using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectNahual.FSM
{
    public class ChaseState : IState
    {
        private float distanceToAttack = 8f;
        private float distanceToLoseSight = 25f;
        private NavMeshAgent _agent = null;
        private Transform _target = null;
        private StateMachine<IState> _stateMachine;
        private DetectionLogic _detectionLogic;
        private bool canTick = false;

        public ChaseState(NavMeshAgent agent, Transform target, DetectionLogic detectionLogic, StateMachine<IState> stateMachine)
        {
            _agent = agent;
            _target = target;
            _stateMachine = stateMachine;
            _detectionLogic = detectionLogic;
        }

        public void Awake()
        {
            Debug.Log("Awake chase state");
        }

        public void Start()
        {
            Debug.Log("Started chase state");
            canTick = true;
        }

        public void Tick()
        {
            if(!canTick) return;
            Debug.Log("Chase state executing...");
            _agent.SetDestination(_target.position);

            float distance = Vector3.Distance(_agent.transform.position, _target.position);
            if(distance < distanceToAttack)
            {
                _stateMachine.ChangeState("Attack");
            }
            else if(distance > distanceToLoseSight && !_detectionLogic.Project())
            {
                _stateMachine.ChangeState("Patrol");
            }

        }

        public void Stop()
        {
            canTick = false;
            _agent.SetDestination(_agent.transform.position);
        }
    }
}
