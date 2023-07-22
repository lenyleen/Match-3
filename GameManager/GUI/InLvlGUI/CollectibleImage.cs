using System;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class CollectibleImage : MonoBehaviour
{
      [SerializeField] private Text countText;
      [SerializeField] private Image doneImage;
      [SerializeField] private Image itemImage;
      private float sizeOfImage;
      public string NameOfCollectible { get; private set; }
      public void Initialize(int count,Texture texture)
      {
            this.enabled = true;
            sizeOfImage = Constants.StandardSizeOfGUIImage;
            CalculateSizeOfTexture(texture);
            this.gameObject.SetActive(true);
            NameOfCollectible = texture.name;
            countText.text = $"{count}";
      }
      private void CalculateSizeOfTexture(Texture texture)
      {
            Texture2D texture2D = (Texture2D) texture;
            Rect rec = new Rect(0, 0, texture2D.width, texture2D.height);
            itemImage.sprite = Sprite.Create(texture2D, rec, new Vector2(0, 0), 1f);
            float width = texture.width;
            float height = texture.height;
            float aspectRatio = width / height;
            if (width > height)
            {
                  width = sizeOfImage;
                  height = width / aspectRatio;
            }
            else
            {
                  height = sizeOfImage;
                  width = height * aspectRatio;
            }
            itemImage.rectTransform.sizeDelta = new Vector2(width, height);
      }

      public void UpdateState(int remainingCount)
      {
            SoundManager.instance.PlaySound("BoosterAward");
            countText.text = remainingCount.ToString();
            if (remainingCount > 0)
                  return;
            doneImage.gameObject.SetActive(true);
            countText.gameObject.SetActive(false);
      }
}
