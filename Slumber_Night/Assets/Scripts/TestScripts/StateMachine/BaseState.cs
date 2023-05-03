 public abstract class BaseState
{
    protected PlayerController playerController;
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
