using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.GameManagement{
    public abstract class UIGridPrefabManager<T> : MonoBehaviour, IGridManager<T> where T : UIGridPrefab
{
    protected T[,] prefabGrid;
    protected GameObject[,] objectGrid;

    protected int columns, rows;
    protected int type;
    protected Vector2 prefabSize, prefabSpacing;

    [SerializeField] protected T prefab;
    [SerializeField] protected RectTransform prefabParent;

    public const int BOTTOM_LEFT = 0, TOP_LEFT = 1;

    private void Awake() {
        // prefabParent.pivot = new Vector2(0,0);
    }

    public virtual void Init(int w, int h, Vector2 size, Vector2 spacing, int type = BOTTOM_LEFT){
        this.columns = w;
        this.rows = h;
        this.prefabSize = size;
        this.prefabSpacing = spacing;
        this.type = type;

        prefabGrid = new T[columns,rows];
        objectGrid = new GameObject[columns, rows];
    }

    public void ResizeParent(){
        prefabParent.GetComponent<RectTransform>().sizeDelta = new Vector2(columns * (prefabSize.x + prefabSpacing.x), rows * (prefabSize.y + prefabSpacing.y));
    }

    public T Add(int x, int y){

        if(x<0 || y<0 || x>=columns || y>=rows) return null;

        var t = Instantiate(prefab, prefabParent);
        var obj = t.gameObject;
        obj.name = "(" + x + ", " + y + ")";

        SetObjectLocalPosition(obj, x, y);
        
        prefabGrid[x,y] = t;
        objectGrid[x,y] = obj;

        t.SetPosition(x,y);

        return t;
    }

    public T[,] AddAll(){
        for(int i = 0; i<columns; i++){
            for(int j = 0; j<rows; j++){
                Add(i,j);
            }
        }
        return prefabGrid;
    }

    protected virtual void SetObjectLocalPosition(GameObject obj, int x, int y){
        var rect = obj.GetComponent<RectTransform>();
        switch(type){
            case BOTTOM_LEFT:
                // 从左下开始
                rect.localPosition = new Vector3( x*prefabSpacing.x + prefabSpacing.x/2 + prefabSpacing.x * x + prefabSpacing.x/2, y*prefabSpacing.y + prefabSpacing.y/2 + prefabSpacing.y * y + prefabSpacing.y/2,0); 
                break;
            case TOP_LEFT:
                // 从左上开始
                rect.localPosition = new Vector3( x*prefabSpacing.x + prefabSpacing.x/2 + prefabSpacing.x * x+ prefabSpacing.x/2, -(y*prefabSpacing.y + prefabSpacing.y/2 + prefabSpacing.y * y+ prefabSpacing.y/2),0); 
                break;
        }
    }

    public T AddNoSave(){
        return Instantiate(prefab, prefabParent);
    }

    public T GetAt(int x, int y){
        if(IsEmpty(x,y)) return null;
        return prefabGrid[x,y];
    }

    public GameObject GetObjectAt(int x, int y){
        if(x<0 || y<0 || x>=columns || y>=rows) return null;
        return objectGrid[x,y];
    }

    public void DeleteAt(int x, int y){
        if(IsEmpty(x,y)) return;

        var obj = GetObjectAt(x,y);
        Destroy(obj);

        prefabGrid[x,y] = null;
        objectGrid[x,y] = null;
    }

    public bool IsEmpty(int x, int y){
        if(x<0 || y<0 || x >= columns || y >= rows) return true;
        return prefabGrid[x,y] == null;
    }

    public void Clear(){
        for(int i = 0; i<columns; i++){
            for(int j  = 0; j<rows; j++){
                DeleteAt(i,j);
            }   
        }
    }

        public T Add()
        {
            throw new System.NotImplementedException();
        }

        public void Delete(T t)
        {
            throw new System.NotImplementedException();
        }
    }

}
