using System;
using UnityEngine;

public class Selected : State
{
    public Selected(InputManager inputManager,CandyStates stateMachine) : base(inputManager,stateMachine)
    { }

    public override void LogicUpdate()
    {
        if (!Input.GetMouseButton(0)) return;
        var firstHitCandy = inputManager.CashedHitCandy;
        var secondHitCollider = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,defaultLayer).collider;
        if (secondHitCollider == null) return;
        var secondHitCandy = secondHitCollider.GetComponent<Candy>();
        if(secondHitCandy == null || secondHitCandy == firstHitCandy) return;
        var firstCandyCell = firstHitCandy.cell;
        var secondCandyCell = secondHitCandy.cell;
        if (!firstCandyCell.IsNearestCell(secondCandyCell))
        {
            stateMachine.ChangeState(inputManager.NotSelectedState);
            return;
        }
        firstHitCandy.PlayAnimation(CandyAnimationTriggers.Unpressed);
        inputManager.CandySwitcher.SwitchPositions(firstHitCandy, secondHitCandy);
        stateMachine.ChangeState(inputManager.IntermediateState);
    }
}
/*var checkCandieForСlosenessByColumn = secondCandyCell.Row == firstCandyCell.Row &&
                                      (secondCandyCell.Column == firstCandyCell.Column + 1 ||
                                       secondCandyCell.Column == firstCandyCell.Column - 1);
        var checkCandieForСlosenessByRows = secondCandyCell.Column == firstCandyCell.Column &&
                                            (secondCandyCell.Row == firstCandyCell.Row + 1 ||
                                             secondCandyCell.Row == firstCandyCell.Row - 1);*/
