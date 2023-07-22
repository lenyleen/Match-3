using System;

public interface IHidingCellBlocker
{
      public Action<Cell, IHidingCellBlocker> DisableAction { get; set; }
}
