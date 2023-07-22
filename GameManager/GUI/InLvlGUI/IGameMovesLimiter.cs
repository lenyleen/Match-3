using System;

public interface IGameMovesLimiter
{
        public event Action onOutOfLimit;
        public void Initialize(int limitCountOfMoves);
        public void AddMovesAfterPayment(); 
}
