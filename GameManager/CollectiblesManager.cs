using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
public class CollectiblesManager : MonoBehaviour
{
     [SerializeField] private GameObject anchorPrefab;
     private List<BezierAnchor> anchors;
     [SerializeField]private PathCreator[] bezierPaths;
     public void Initialize(List<Element<Texture>> collectiblesData)
     {
          anchors = new List<BezierAnchor>();
          for (int i = 0; i < collectiblesData.Count(); i++)
          {
               var newAnchor = Instantiate(anchorPrefab, Constants.BezierAnchorsPositions[i], quaternion.identity,this.transform).GetComponent<BezierAnchor>(); 
               anchors.Add(newAnchor);
               newAnchor.Initialize(collectiblesData[i].Index0,collectiblesData[i]._element.name,bezierPaths);
               newAnchor.OnAllCollectedAction += RemoveAnchor;
          }
     }

     private void RemoveAnchor(BezierAnchor anchor)
     {
          for (int i = 0; i < anchors.Count; i++)
          {
               if (anchors[i] == anchor)
               {
                    anchors.Remove(anchors[i]);
                    anchor.OnAllCollectedAction -= RemoveAnchor;
               }
          }
     }
     public BezierAnchor CheckItemForCollectibility(GameObject item)
     {
          if (item.GetComponent<ICanBeCollected>() == null) return null;
          foreach (var anchor in anchors)
          {
               if (item.GetComponent<SpriteRenderer>().sprite.name == anchor.name)
               {
                    return anchor;
               }
          }
          return null;
     } 
}
