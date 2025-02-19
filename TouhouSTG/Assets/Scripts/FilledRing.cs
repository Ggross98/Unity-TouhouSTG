using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilledRing : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color[] colors;
    // [SerializeField] private EnemyController enemy;
    private RectTransform rect;


    private bool filling = false;

    private void Awake() {
        rect = GetComponent<RectTransform>();
    }


    private bool showing = false;

    private void Update() {
        
    }

    public void Refresh(EnemyController enemy){
        RefreshPosition(enemy);
        if(showing && !filling){
            // transform.position = enemy.position;
            RefreshValue(enemy);
        }
    }

    public void Show(int color = 0){

        // RefreshPosition();
        showing = true;
        image.color = colors[color];
        gameObject.SetActive(true);
    }

    public void Hide(){
        showing = false;
        gameObject.SetActive(false);
    }

    private void RefreshPosition(EnemyController enemy){
        // var screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
        // Debug.Log(enemy.transform.position + ", " + screenPos);
        // rect.localPosition = screenPos;


        Vector3 wPos = enemy.transform.position;
        Vector2 uPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), wPos, null, out uPos);
        // Debug.Log(wPos + ", " + uPos);
        rect.localPosition = uPos;
    
    }

    private void RefreshValue(EnemyController enemy){
        image.fillAmount = (float)enemy.spellLife / (float)enemy.spellMaxLife;
    }

    public void Fill(float duration = 0.5f){
        if(filling) return;

        image.fillAmount = 0f;
        StartCoroutine(FillAnimation(duration));
    }

    private IEnumerator FillAnimation(float duration){
        filling = true;

        float timer = 0;
        while(timer < duration){
            timer += Time.deltaTime;
            image.fillAmount = timer/duration;
            yield return new WaitForEndOfFrame();
        }

        image.fillAmount = 1;
        filling = false;

        yield return null;
    }



}
