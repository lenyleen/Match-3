using UnityEngine;

public class NotSelected : State
{
    public NotSelected(InputManager inputManager, CandyStates stateMachine) : base(inputManager, stateMachine)
    {
    }
    public override void LogicUpdate()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        inputManager.StopAllCoroutines();
        var firstHitCollider = Physics2D
            .Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, defaultLayer)
            .collider;
        if (firstHitCollider == null || firstHitCollider.GetComponent<Candy>() == null) return;
        var firstHitCandy = firstHitCollider.GetComponent<Candy>();
        firstHitCandy.PlayAnimation(CandyAnimationTriggers.Reset);
        firstHitCandy.PlayAnimation(CandyAnimationTriggers.Pressed);
        inputManager.SetHitCandy(firstHitCandy);
        stateMachine.ChangeState(inputManager.SelectedState);
    }
}
