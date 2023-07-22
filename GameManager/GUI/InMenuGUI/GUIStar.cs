using System;
using UnityEngine;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(Animator))]
public class GUIStar : MonoBehaviour
{
     private Animator animator => this.GetComponent<Animator>();
     [SerializeField] private Sprite earnedImage;
     private Image image => this.GetComponent<Image>();
     private RectTransform rectTransform => this.GetComponent<RectTransform>();
     private const string activateTrigger = "Activate";

     private void OnEnable()
     {
          rectTransform.anchorMax = Vector2.zero;
          rectTransform.anchorMin = Vector2.zero;
     }

     public void Activate()
     {
          animator.SetTrigger(activateTrigger);
          image.sprite = earnedImage;
     }
}
