using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Timer Decision", fileName = "TimerDecision")]
    public class TimerDecision : Decision
    {
        [SerializeField] private float _duration;
        private float _elapsedTime;

        public override bool Decide(BaseStateMachine stateMachine)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _duration)
            {
                _elapsedTime = 0f; // Reset elapsed time
                return true; // Condition met
            }

            return false; // Condition not met
        }

        public void ResetTimer()
        {
            _elapsedTime = 0f;
        }
    }
}