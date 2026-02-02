using UnityEngine;
using UnityEngine.AI;

namespace ProjectNahual.FSM
{
    public class PatrolState : IState
    {
        private Vector3 _origin;
        private NavMeshAgent _agent;
        private float maxDistanceFromOrigin = 10f;
        private Collider _patrolArea;
        private DetectionLogic _detectionLogic;
        private StateMachine<IState> _stateMachine;
        private bool canTick;
        private bool isWaiting;
        private float _waitTimer;
        private float _waitDuration = 10f;
        private Vector3 _nextDestination;
        
        public PatrolState(NavMeshAgent agent, Vector3 origin, Collider patrolArea, DetectionLogic detectionLogic, StateMachine<IState> stateMachine)
        {
            _agent = agent;
            _origin = origin;
            _patrolArea = patrolArea;
            _detectionLogic = detectionLogic;
            _stateMachine = stateMachine;
        }

        public void Awake() => Debug.Log("Awake patrol state");

        public void Start()
        {
            Debug.Log("Started patrol state");
            _agent.SetDestination(MoveToNextPoint());
            canTick = true;
            isWaiting = false;
        }

        public void Tick()
        {
            if(!canTick) return;
            // Debug.Log("Patrol state executing...");
            
            if(_detectionLogic.Project())
            {
                _stateMachine.ChangeState("Chase");
            }

            if(isWaiting)
            {
                _waitTimer += Time.deltaTime;
                if(_waitTimer >= _waitDuration)
                {
                    isWaiting = false;
                    _waitTimer = 0f;
                    _agent.SetDestination(_nextDestination);
                }
            }
            else if(_agent.remainingDistance < 1.5f)
            {
                isWaiting = true;
                _nextDestination = MoveToNextPoint();
            }
        }

        public void Stop()
        {
            canTick = false;
            _agent.destination = _agent.transform.position;
        } 

        private Vector3 MoveToNextPoint()
        {
            float distance = Vector3.Distance(_agent.transform.position, _origin);
            
            if(distance >= maxDistanceFromOrigin)
                return _origin;

            return GetRandomPointInBounds();
        }

        public Vector3 GetRandomPointInBounds()
        {
            if (_patrolArea == null)
            {
                Debug.LogError("Bounds Collider not assigned!");
                return Vector3.zero;
            }

            Bounds bounds = _patrolArea.bounds;

            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(randomX, 0, randomZ);
        }
    }
}
