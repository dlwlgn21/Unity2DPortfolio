
public class StateMachine<T> where T : class
{
    private T _onwer;
    private State<T> _currState = null;

    public void Init(T onwer, State<T> entryState)
    {
        _onwer = onwer;
        ChangeState(entryState);
    }

    public void Excute()
    {
        if (_currState != null)
            _currState.Excute();
    }

    public void FixedExcute()
    {
        if (_currState != null)
            _currState.FixedExcute();
    }

    public void ChangeState(State<T> eNewState)
    {
        if (eNewState == null) 
            return;
        if (_currState != null)
            _currState.Exit();
        _currState = eNewState;
        _currState.Enter();
    }
}
