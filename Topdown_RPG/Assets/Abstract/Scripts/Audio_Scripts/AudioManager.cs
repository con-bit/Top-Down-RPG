using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;
    private AudioData audioData = null;
    private AudioSource audioSourceMusic;
    private AudioSource audioSourceSFX;

    /// <summary>
    /// creates a audio manager gameobject first time its used.
    ///     Use the instance to get to the methods to play the sounds.
    /// </summary>
    public static AudioManager Instance
    {
        get
        {
            if (AudioManager.instance == null)
            {
                GameObject AudioManagerGameObject = new GameObject("AudioManager");

                AudioManager.instance = AudioManagerGameObject.AddComponent<AudioManager>();

                AudioManager.instance.audioData = Resources.Load<AudioData>("Abstract/Audio/AudioData");
                AudioManager.instance.audioData.AudioDataInspectorChanged += AudioManager.instance.OnAudioDataInspectorChanged;

                AudioManager.instance.audioSourceMusic = AudioManagerGameObject.AddComponent<AudioSource>();
                AudioManager.instance.audioSourceSFX = AudioManagerGameObject.AddComponent<AudioSource>();

                DontDestroyOnLoad(AudioManagerGameObject);
            }

            return instance;
        }
    }

    /// <summary>
    /// stops music.
    /// </summary>
    public void StopMusic()
    {
        if (audioSourceMusic.isPlaying)
        {
            audioSourceMusic.Stop();
        }
    }

    /// <summary>
    /// plays  music by MusicAudio enum.
    /// </summary>
    /// <param name="audioMusic"> background audio enum name. </param>
    public void PlayMusic(MusicAudio audioMusic)
    {
        bool isClipFound = false;
        for (int index = 0; index < audioData.audioMusicClipPairs.Count && !isClipFound; index++)
        {
            if (audioMusic == audioData.audioMusicClipPairs[index].audioTypeSound)
            {
                // whenever current audioclip is the same wanting to be played and it is currently being played dont do anything
                if (audioSourceMusic.clip != audioData.audioMusicClipPairs[index].audioClip || !audioSourceMusic.isPlaying)
                {
                    // if previous audio is being played stop
                    if (audioSourceMusic.isPlaying)
                    {
                        audioSourceMusic.Stop();
                    }

                    audioSourceMusic.clip = audioData.audioMusicClipPairs[index].audioClip;
                    audioSourceMusic.loop = true;
                    audioSourceMusic.Play();
                }
            }
        }
    }

    /// <summary>
    /// Plays a oneshot of a soundeffect.
    /// </summary>
    /// <param name="audioSFX"></param>
    public void PlaySFX(SFXAudio audioSFX)
    {
        bool isClipFound = false;
        for (int index = 0; index < audioData.audioSFXClipPairs.Count && !isClipFound; index++)
        {
            if (audioSFX == audioData.audioSFXClipPairs[index].audioTypeSound)
            {
                audioSourceSFX.PlayOneShot(audioData.audioSFXClipPairs[index].audioClip);
            }
        }
    }

    /// <summary>
    ///     when changing the AudioData inspector update the static audio.
    /// </summary>
    /// <param name="sender"> audiodata inspector changed. </param>
    /// <param name="eventArg"> null. </param>
    private void OnAudioDataInspectorChanged(AudioData sender)
    {
        audioData = sender;
    }
}
