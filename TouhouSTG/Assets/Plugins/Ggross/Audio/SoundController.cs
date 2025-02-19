using System.Collections.Generic;
using UnityEngine;
using Ggross.Pattern;

namespace Ggross.Audio{
    public class SoundController : SingletonMonoBehaviour<SoundController>
    {
        private List<AudioSource> sourceList;
        private int count = 3;
        private bool muted = false;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);

            for(int i = 0; i<count; i++){
                var s = gameObject.AddComponent<AudioSource>();
                sourceList.Add(s);
                s.loop = false;
            }
        }

        void Update()
        {
        }

        public bool IsMuted(){
            return muted;
        }

        public void Mute(bool b){
            muted = b;
            SetVolume(b?0 :100);
        }

        public void ChangeMute(){
            Mute(!muted);
        }


        private void SetVolume(float v){
            foreach(var s in sourceList) s.volume = v;
        }

        public void Play(AudioClip clip){
            int index = 0;
            for(int i = 0; i < count; i++){
                if(!sourceList[i].isPlaying){
                    index = i;
                    break;
                }
            }
            var source = sourceList[index];
            source.clip = clip;
            source.Play();

        }
    }
}


