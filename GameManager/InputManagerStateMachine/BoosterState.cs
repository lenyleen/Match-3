
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BoosterState : State
{
    private List<Candy> hitCandies;
    public BoosterState(InputManager inputManager, CandyStates stateMachine) : base(inputManager, stateMachine)
    {
    }
    public override void Enter()
    {
        hitCandies = new List<Candy>();
    }

    public override async void LogicUpdate()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        inputManager.StopAllCoroutines();
        var firstHitCollider = Physics2D
            .Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, defaultLayer)
            .collider;
        if (firstHitCollider == null || firstHitCollider.GetComponent<Candy>() == null) return;
        var firstHitCandy = firstHitCollider.GetComponent<Candy>();
        if (inputManager.BoosterType != BoosterType.CandiesSwitcher)
        {
            await inputManager.BoosterActionHandler.DoBoosterAction(inputManager.BoosterType,firstHitCandy);
            await inputManager.CandySwitcher.CheckAfterShuffleOrBonus();
            EventBus.RaiseEvent<IMoveEndedObserver>(listener => listener.MoveEnded());
        }
        hitCandies.Add(firstHitCandy);
        if (hitCandies.Count >= 2)
        {
            inputManager.CandySwitcher.SwitchPositions(hitCandies[0],hitCandies[1]);
        }
    }
}
