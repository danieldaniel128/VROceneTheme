using System.Collections.Generic;

public class State : IState
{
    public List<Transition> Transitions { get; private set; } = new List<Transition>();

    public virtual void EnterState()
    {
    }

    public virtual void ExecuteUpdateState()
    {
    }

    public virtual void ExitState()
    {
    }
}
