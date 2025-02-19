using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ggross.Pattern;

public class BGMManager : SingletonMonoBehaviour<BGMManager>
{
    private AudioSource source;
    private AudioClip[] clips;
    private int index = -1;
    private bool playing = false;
    private bool muted = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (source == null) source = gameObject.AddComponent<AudioSource>();
        source.loop = false;
    }

    void Update()
    {
        if (playing)
        {
            if (!source.isPlaying)
            {
                index++;
                if (index >= clips.Length) index = 0;

                PlayAt(index);
            }
        }
    }
    public bool IsMuted()
    {
        return muted;
    }
    public void Mute(bool b)
    {
        muted = b;
        SetVolume(b ? 0 : 100);
    }

    public void ChangeMute()
    {
        Mute(!muted);
    }

    private void SetVolume(float v)
    {
        source.volume = v;
    }

    public void SetClips(AudioClip[] clips)
    {
        this.clips = clips;
    }

    public int Count()
    {
        if (clips == null) return 0;
        else return clips.Length;
    }

    private void Play(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
        playing = true;
    }

    public void PlayAt(int i, bool restart = true)
    {
        if (clips == null) return;
        if (i < 0 || i >= clips.Length) return;

        var clip = clips[i];
        if(!restart && (clip == source.clip)) return;
        Play(clips[i]);
        index = i;

    }

    public void Stop(){
        source.Stop();
    }
}
