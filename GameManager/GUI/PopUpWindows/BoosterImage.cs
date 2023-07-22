using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BoosterImage", menuName = "GameBoosters")]
public class BoosterImage : ScriptableObject
{
        [field: SerializeField] public Sprite sprite { get; private set; }
        [field:SerializeField] public int price { get; private set; }
        [HideInInspector]public string description;
        [field:SerializeField] public BoosterType boosterType { get; private set;}
        public string Name => boosterType.ToString();
}
