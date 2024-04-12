
public class StateMachine<T> where T : class
{
    private T mOnwer;
    private State<T> mCurrState = null;

    public void Init(T onwer, State<T> entryState)
    {
        mOnwer = onwer;
        ChangeState(entryState);
    }

    public void Excute()
    {
        if (mCurrState != null)
            mCurrState.Excute();
    }

    public void FixedExcute()
    {
        if (mCurrState != null)
            mCurrState.FixedExcute();
    }

    public void ChangeState(State<T> eNewState)
    {
        if (eNewState == null) 
            return;
        if (mCurrState != null)
            mCurrState.Exit();
        mCurrState = eNewState;
        mCurrState.Enter();
    }
}
