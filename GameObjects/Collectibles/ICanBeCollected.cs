using System;
using System.Threading.Tasks;
using UnityEngine;

public interface ICanBeCollected
{
      public string collectibleId { get; }
      public CollectiblesManager CollectiblesManager { get; set; }
      public Task Collect(Path path);
      public void InitializeAsCollectible(CollectiblesManager collectiblesManager);
}
