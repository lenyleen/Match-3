using UnityEngine;

public abstract class State
{
    protected InputManager inputManager;
    protected CandyStates stateMachine;
    protected LayerMask defaultLayer = LayerMask.NameToLayer("Default");
    protected Camera mainCamera = Camera.main;
    protected State(InputManager inputManager,CandyStates stateMachine)
    {
        this.stateMachine = stateMachine;
        this.inputManager = inputManager;
    }
    public virtual void Enter()
    {
        
    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void Exit()
    {

    }
}
