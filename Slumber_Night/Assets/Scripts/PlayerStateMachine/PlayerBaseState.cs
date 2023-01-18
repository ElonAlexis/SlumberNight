
public abstract class PlayerBaseState 
{
    protected PlayerStateMachine _ctx; 
    protected PlayerStateFactory _factory;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext; 
        _factory = playerStateFactory;
    }


    public abstract void EnterState();
    
    public abstract void Updatestate();
    
    public abstract void ExitState();
    
    public abstract void InitializeSubstate();

    public abstract void CheckSwitchState();
    

    void UpdateStates()
    {

    }
    protected void SwitchState(PlayerBaseState newState)
    {
        // Current State Exits State
        ExitState(); 

        // new State enters state 
        newState.EnterState();

        //switch current state of context 
        _ctx.CurrentState = newState;
    }
    protected void SetSuperState(PlayerBaseState newSuperState)
    {

    }
    protected void SetSubState(PlayerBaseState newSubState)
    {

    }
}
