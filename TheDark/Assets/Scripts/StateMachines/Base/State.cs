
public abstract class State<T> where T : class
{
    protected T _entity;

    public State(T entity) { _entity = entity;}
    public virtual void Enter() { }
    public abstract void Excute();
    public virtual void FixedExcute() { }
    public virtual void Exit() { }
}
