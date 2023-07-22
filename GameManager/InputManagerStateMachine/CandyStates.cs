using System;
public class CandyStates
{
       public State currentState { get; private set; }

       public void Initialize(State startingState)
       {
              currentState = startingState;
       }

       public void ChangeState(State newState)
       {
              currentState.Exit();
              currentState = newState;
              currentState.Enter();
       }
}
