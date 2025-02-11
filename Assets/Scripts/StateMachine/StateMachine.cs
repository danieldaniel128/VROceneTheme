using System.Collections.Generic;

public class StateMachine
{
    private IState currentState;
    private List<Transition> globalTransitions = new List<Transition>();

    public void AddGlobalTransition(IState targetState, System.Func<bool> condition)
    {
        globalTransitions.Add(new Transition(targetState, condition));
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }

    public void Update()
    {
        // Check global transitions
        foreach (var transition in globalTransitions)
        {
            if (transition.Condition())
            {
                ChangeState(transition.TargetState);
                return; // Only one transition per frame
            }
        }

        // Check current state's transitions
        if (currentState != null)
        {
            foreach (var transition in currentState.Transitions)
            {
                if (transition.Condition())
                {
                    ChangeState(transition.TargetState);
                    return; // Only one transition per frame
                }
            }

            currentState.ExecuteUpdateState();
        }
    }
}
