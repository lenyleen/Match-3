using UnityEngine.UI;

public class SoundedButton : Button
{
    protected override void Awake()
    {
        onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        SoundManager.instance.PlaySound("Button");
    }
}
