using UnityEngine;
using UnityEngine.AI;
using ProjectNahual.FSM;
using ProjectNahual.Utils;

namespace ProjectNahual.Enemies
{
    public class HunterBehaviour : BaseBehaviour
    {
        [Header("Animations")]
        [SerializeField] private AnimationContext animationContext;
        [SerializeField] private Axe homingAxe;
        [SerializeField] private Transform handBone;
        private ChaseState chaseState;
        private AttackState attackState;
        private PatrolState patrolState;
        private DetectionLogic detectionLogic;
        private Transform playerTransform;

        public void Awake() => playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        private void OnEnable() => animationContext.AttackAnimationEvent += AttackAnimationEvent;
        private void OnDisable() => animationContext.AttackAnimationEvent -= AttackAnimationEvent;

        public override void Init()
        {
            // base.Init();
            detectionLogic = new DetectionLogic(agent.transform, playerTransform, 10, 45);

            // Initialize the state machine
            stateMachine = new StateMachine<IState>();
            
            // Create state instances
            patrolState = new PatrolState(agent, transform.position, patrolArea, detectionLogic, stateMachine);
            chaseState = new ChaseState(agent, playerTransform, detectionLogic, stateMachine);
            attackState = new AttackState(agent.transform, playerTransform, animator, stateMachine);

            // Add states to the state machine
            stateMachine.AddState("Patrol", patrolState);
            stateMachine.AddState("Chase", chaseState);
            stateMachine.AddState("Attack", attackState);

            // Set initial state
            stateMachine.SetInitialState("Patrol");
            Debug.Log($"Initial state: {stateMachine.CurrentStateName}");
            canTick = true;

            // Invoke(nameof(TransitionToChase), 2f);
            // Invoke(nameof(TransitionToAttack), 4f);
            // Invoke(nameof(RevertToPreviousState), 6f);

        }

        public void AttackAnimationEvent()
        {
            // Debug.Log("Should spawn axe");
            Axe axe = Instantiate(homingAxe, handBone.position, Quaternion.identity);
            axe.transform.LookAt(playerTransform);
            axe.Init(playerTransform);
        }
    }
}