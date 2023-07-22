using System;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundFx
{
       private readonly AudioSource audioSource;

       public SoundFx(AudioSource audioSource)
       {
              this.audioSource = audioSource;
       }
       public void Play()
       {
              audioSource.Play();
       }
       private void DisableSoundFx()
       {
              audioSource.Stop();
       }
}
