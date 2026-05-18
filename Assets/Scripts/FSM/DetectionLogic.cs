using UnityEngine;

namespace ProjectNahual.FSM
{
    public class DetectionLogic
    {
        public Transform _agent = null;
        public Transform _target = null;
        public float _viewRadius = 20f;
        public float _viewAngle = 120f;

        public DetectionLogic(Transform agent, Transform target, float radius, float angle)
        {
            _viewRadius = radius;
            _viewAngle = angle;
            _agent = agent;
            _target = target;
        }

        public bool Project()
        {
            if (Vector3.Distance(_agent.position, _target.position) < _viewRadius)
            {
                Vector3 dirToTarget = (_target.position - _agent.position).normalized;

                if (Vector3.Angle(_agent.forward, dirToTarget) < _viewAngle / 2f)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

