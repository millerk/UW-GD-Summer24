using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/IdleState", fileName = "IdleState")]
    public class IdleState : BaseState
    {
        public override void Execute(BaseStateMachine machine)
        {
            // Implementation of idle behavior
            // For example, the enemy could just wait and do nothing
            // Debug.Log("Enemy is idling...");
        }
    }
}