using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ggross.Pattern;

namespace Ggross.Dialogue{
    public class DialogueManager: SingletonMonoBehaviour<DialogueManager>
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private Text contentText;
        [SerializeField] private Text nameText;
        // [SerializeField] private Image[] images;
        [SerializeField] private Image portrait;

        public System.Action endAction = null;

        private DialogueData data;
        private int index;

        private bool showing = false;


        public void LoadDialogue(DialogueData data){
            this.data = data;
        }

        public void StartDialogue(){
            showing = true;
            SetActive(true);
            index = 0;
            ShowSentenceAtIndex(index);
        }

        public void SetActive(bool a){
            dialoguePanel.SetActive(a);
        }

        public void ShowNextSentence(){
            if(!showing) return;

            index++;
            if(index >= data.sentences.Count){
                EndSentence();
            }
            else{
                ShowSentenceAtIndex(index);
            }
        }

        public void EndSentence(){
            SetActive(false);
            showing = false;

            if(endAction != null){
                endAction.Invoke();
            }
        }

        public bool IsShowing(){
            return showing;
        }

        private void ShowSentenceAtIndex(int index){
            ShowSentence(data.sentences[index]);
        }

        private void ShowSentence(SentenceData sentence){
            

            int pos = sentence.index % 2 == 0 ? 0 : 1;
            ShowPortrait(pos, sentence.imageIndex);
            
            string name = data.names[sentence.index];
            ShowName(name);

            ShowContent(sentence.content);
            
        }

        private void ShowPortrait(int index, int spriteIndex){
            // images[index].sprite = sprite;
            // images[index].gameObject.SetActive(true);
            // images[1-index].gameObject.SetActive(false);
            var sprite = data.images[index].sprites[spriteIndex];
            portrait.sprite = sprite;
        }

        private void ShowContent(string content){

            content = System.Text.RegularExpressions.Regex.Unescape(content);

            contentText.text = content;
        }

        private void ShowName(string name){
            // nameText.text = name;
        }

        
    }
}

