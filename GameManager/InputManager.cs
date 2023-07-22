using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class InputManager : MonoBehaviour, IGamePause, IBoosterActivated
{
        
        private CandyStates stateMachine;
        public Candy CashedHitCandy { get; private set; }
        public CandySwitcher CandySwitcher { get; private set; }
        public Selected SelectedState { get; private set; }
        public NotSelected NotSelectedState { get; private set; }
        private BoosterState BoosterState { get; set;}
        public IntermediateState IntermediateState { get; private set; }
        public BoosterType BoosterType { get; private set; }
        private SuggestedMatchFinder suggestedMatchFinder;
        public BoosterActionHandler BoosterActionHandler { get; private set; }
        public CellGrid CellGrid { get; private set; }
        public void Initialize(CandySwitcher candySwitcher,CellGrid cellGrid)
        {
                EventBus.Subscribe(this);
                this.CellGrid = cellGrid;
                this.CandySwitcher = candySwitcher;
                this.suggestedMatchFinder = suggestedMatchFinder;
                BoosterActionHandler = new BoosterActionHandler(cellGrid);
                suggestedMatchFinder = new SuggestedMatchFinder(cellGrid);
                InitializeStates();
        }
        private void InitializeStates()
        {
                stateMachine = new CandyStates();
                SelectedState = new Selected(this, stateMachine);
                NotSelectedState = new NotSelected(this, stateMachine);
                IntermediateState = new IntermediateState(this, stateMachine);
                BoosterState = new BoosterState(this, stateMachine);
                stateMachine.Initialize(NotSelectedState);
                StartCoroutine(ShowSuggestedMatch());
        }

        private async void Update()
        {
                stateMachine.currentState.LogicUpdate();
                if (Input.GetKeyDown(KeyCode.F))
                        await CellGridShuffler.ShuffleCellGrid(CellGrid);
        }

        public void BoosterActivated(BoosterType boosterType, Action boosterWasActivated)
        {
                if (stateMachine.currentState != NotSelectedState) return;
                this.BoosterType = boosterType;
                stateMachine.ChangeState(BoosterState);
                boosterWasActivated?.Invoke();
        }
        public async void Pause(object sender,bool stop)
        {
                if(stop)
                {
                        stateMachine.ChangeState(IntermediateState);
                        return;
                }
                if(sender is SuggestedMatchFinder)
                {
                        await CandySwitcher.CheckAfterShuffleOrBonus();
                        return;
                }
                stateMachine.ChangeState(NotSelectedState);
                StartCoroutine(ShowSuggestedMatch());
        }
        public void SetHitCandy(Candy candy)
        {
                CashedHitCandy = candy;
        }

        private IEnumerator ShowSuggestedMatch()
        {
                yield return new WaitForSeconds(10f);
                suggestedMatchFinder.PotentialMatches();
                StartCoroutine(ShowSuggestedMatch());
        }

        private void OnDisable()
        {
               EventBus.Unsubscribe(this); 
        }
}
