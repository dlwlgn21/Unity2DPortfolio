
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
            mCurrState.Excute(mOnwer);
    }
    public void ChangeState(State<T> eNewState)
    {
        if (eNewState == null) 
            return;
        if (mCurrState != null)
            mCurrState.Exit(mOnwer);
        mCurrState = eNewState;
        mCurrState.Enter(mOnwer);
    }
}
