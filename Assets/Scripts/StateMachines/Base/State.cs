
public abstract class State<T> where T : class
{
    public virtual void Enter(T entity) { }
    public abstract void Excute(T entity);
    public virtual void FixedExcute(T entity) { }
    public virtual void Exit(T entity) { }
}
