using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
        public List<AudioClip> sounds;
        public static SoundManager instance;
        private readonly Dictionary<string, AudioClip> nameToSound = new Dictionary<string, AudioClip>();
        private AudioClip playingSound;
        [SerializeField]private AudioSource bgMusic;
        [SerializeField] private AudioMixerGroup mixerGroup;
        [SerializeField]private AudioSource soundFXSource;
        private const float SoundMaxVolume = 0;
        private const float SoundMinVolume = -80;
        public bool BgEnabled { get; private set; }
        public bool FXEnabled { get; private set; }
        public float BgVolume { get; private set; }
        public float FXVolume { get; private set; }
        private void Awake()
        {
            if (!PlayerPrefs.HasKey("music_enabled"))
            {
                PlayerPrefs.SetInt("music_enabled",1);
                PlayerPrefs.SetInt("sound_enabled",1);
                PlayerPrefs.SetFloat("sound_volume", SoundMaxVolume);
                PlayerPrefs.SetFloat("music_volume", SoundMaxVolume);
            }
            BgEnabled = PlayerPrefs.GetInt("music_enabled") == 1;
            FXEnabled = PlayerPrefs.GetInt("sound_enabled") == 1;
            BgVolume = PlayerPrefs.GetFloat("music_volume");
            FXVolume = PlayerPrefs.GetFloat("sound_volume");
            mixerGroup.audioMixer.SetFloat("sound_volume", FXVolume);
            mixerGroup.audioMixer.SetFloat("music_volume", BgVolume);
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            foreach (var sound in sounds)
            {
                nameToSound.Add(sound.name, sound);
            }
            bgMusic.Play();
        }
        public void PlaySound(string soundName, bool loop = false)
        {
            
            var sound = PlayerPrefs.GetInt("sound_enabled");
            if (sound != 1)
            {
                return;
            }
            var clip = nameToSound[soundName];
            if (clip == null || playingSound == clip) return;
            soundFXSource.clip = clip;
            soundFXSource.loop = loop;
            playingSound = clip;
            soundFXSource.Play();
        }

        private void ChangeSoundVolume(bool enabled, string mixerName,string prefName)
        {
            if (enabled)
                mixerGroup.audioMixer.SetFloat(mixerName, PlayerPrefs.GetFloat(mixerName));
            else
                mixerGroup.audioMixer.SetFloat(mixerName, SoundMinVolume); 
            PlayerPrefs.SetInt(prefName, enabled ? 1 : 0);
        }

        private void ChangeSoundVolume(float volume, string mixerName)
        {
            mixerGroup.audioMixer.SetFloat(mixerName, Mathf.Lerp(SoundMinVolume, SoundMaxVolume, volume));
            PlayerPrefs.SetFloat(mixerName,volume);
        }
        public void SetSoundFxEnabled(bool soundEnabled)
        {
            ChangeSoundVolume(soundEnabled, "sound_volume","sound_enabled");
            FXEnabled = soundEnabled;
        }

        public void SetMusicEnabled(bool musicEnabled)
        {
            ChangeSoundVolume(musicEnabled, "music_volume","music_enabled");
            BgEnabled = musicEnabled;
        }
        
        public void ToggleSound(float volume)
        {
           ChangeSoundVolume(volume,"sound_volume");
           FXVolume = volume;
           PlayerPrefs.SetFloat("sound_volume", volume);
        }

        public void ToggleMusic(float volume)
        {
            ChangeSoundVolume(volume,"music_volume");
            BgVolume = volume;
            PlayerPrefs.SetFloat("music_volume", volume);
        }
}