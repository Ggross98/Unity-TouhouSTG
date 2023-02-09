using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.Dialogue{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Ggross/DialogueData", order = 0)]
    public class DialogueData : ScriptableObject {
        public SpriteList[] images;

        // public List<List<Sprite>> images;
        public List<string> names;
        public List<SentenceData> sentences;
    }

    [System.Serializable]
    public class SentenceData 
    {
        public int index;
        public int imageIndex;
        public string content;
    }

    [System.Serializable]
    public class SpriteList
    {
        public List<Sprite> sprites;
    }

}

