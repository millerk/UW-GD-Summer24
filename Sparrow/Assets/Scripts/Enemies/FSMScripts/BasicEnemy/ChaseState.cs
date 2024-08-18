using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/ChaseState", fileName = "ChaseState")]
    public class ChaseState : State
    {
        private void OnEnable()
        {
            //Action.Add(ScriptableObject.CreateInstance<ChaseAction>());
        }
    }
}
