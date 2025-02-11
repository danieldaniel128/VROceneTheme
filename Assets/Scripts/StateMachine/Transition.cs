public class Transition
{
    public IState TargetState { get; private set; }
    public System.Func<bool> Condition { get; private set; }

    public Transition(IState targetState, System.Func<bool> condition)
    {
        TargetState = targetState;
        Condition = condition;
    }
}