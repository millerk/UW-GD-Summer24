using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(menuName = "FSM/State")]
    public class State : BaseState
    {
        public List<FSMAction> Action = new List<FSMAction>() ?? new List<FSMAction>();
        public List<Transition> Transitions = new List<Transition>() ?? new List<Transition>();

        public override void Execute(BaseStateMachine machine)
        {
            foreach (var action in Action)
                action.Execute(machine);

            foreach (var transition in Transitions)
                transition.Execute(machine);
        }
    }
}
