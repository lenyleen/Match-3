public class IntermediateState : State
{
    public IntermediateState(InputManager inputManager, CandyStates stateMachine) : base(inputManager, stateMachine)
    {
    }

    public override void Enter()
    {
        inputManager.StopAllCoroutines();
    }

    public override void LogicUpdate()
    {}
}
