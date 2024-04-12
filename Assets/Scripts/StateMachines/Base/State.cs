
public abstract class State<T> where T : class
{
    protected T mEntity;

    public State(T entity) { mEntity = entity;}
    public virtual void Enter() { }
    public abstract void Excute();
    public virtual void FixedExcute() { }
    public virtual void Exit() { }
}
