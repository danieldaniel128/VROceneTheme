using System.Collections.Generic;

public interface IState
{
    List<Transition> Transitions { get; }
    void EnterState();
    void ExecuteUpdateState();
    void ExitState();
}
