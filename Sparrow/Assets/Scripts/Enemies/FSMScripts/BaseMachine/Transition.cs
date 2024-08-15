using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/Transition")]
    public class Transition : ScriptableObject
    {
        public Decision Decision;
        public BaseState TrueState;
        public BaseState FalseState;

        public void Execute(BaseStateMachine stateMachine)
        {
            bool decisionResult = Decision.Decide(stateMachine);
            Debug.Log($"Transition: {Decision.name} Decision result: {decisionResult}");

            if (Decision.Decide(stateMachine) && !(TrueState is RemainInState))
            {
                Debug.Log($"Transitioning to {TrueState.name}");
                stateMachine.CurrentState = TrueState;
            }
            else if (!decisionResult && !(FalseState is RemainInState))
            {
                Debug.Log($"Transitioning to {FalseState.name}");
                stateMachine.CurrentState = FalseState;
            }

        }
    }
}
