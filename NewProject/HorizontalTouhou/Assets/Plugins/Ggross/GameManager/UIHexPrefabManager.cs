using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ggross.GameManagement{
    /// <summary>
    /// 两种排列方式：平朝上、尖朝上
    /// </summary>
    public enum Orientation { FlapTop, PointyTop };

    /// <summary>
    /// 两种偏移方式：奇数行向右、偶数行向右
    /// </summary>
    public enum Offset { Odd, Even };

    public abstract class UIHexPrefabManager<T> : MonoBehaviour, IGridManager<T> where T: UIHexPrefab
{
    protected T[,] grid;
    protected GameObject[,] objectGrid;

    public int[,] state;
    public const int STATE_OUT = -1, STATE_BLANK = 0, STATE_FILLED = 1;

    [SerializeField] protected GameObject prefab;
    [SerializeField] protected Transform parent;

    protected int rows, columns;
    protected float size, width, height;
    public readonly static float sqrt3 = Mathf.Sqrt(3);


    
    protected Orientation orientation = Orientation.PointyTop;

    
    protected Offset offset = Offset.Odd;

    /// <summary>
    /// 六种方向
    /// </summary>
    public static readonly Vector3Int[] Directions = new Vector3Int[]{
        new Vector3Int(1,-1,0),
        new Vector3Int(0,-1,1),
        new Vector3Int(-1,0,1),
        new Vector3Int(-1,1,0),
        new Vector3Int(0,1,-1),
        new Vector3Int(1,0,-1)
    };
    public const int RIGHT = 0, TOP_RIGHT = 1, TOP_LEFT = 2, LEFT = 3, BOTTOM_LEFT = 4, BOTTOM_RIGHT = 5;
    public const int OTHER_DIRECTON = -1;


    public int GetDirection(Vector2Int a, Vector2Int b){
        if(a == b) return OTHER_DIRECTON;
        var cubeA = OffsetToCube(a);
        var cubeB = OffsetToCube(b);
        var delta = (new Vector3(cubeB.x, cubeB.y, cubeB.z)-new Vector3(cubeA.x, cubeA.y, cubeA.z)).normalized;
        for(int i = 0; i<Directions.Length; i++){
            var dir = Directions[i];
            if(new Vector3(dir.x, dir.y, dir.z).normalized == delta) return i;
        }

        return OTHER_DIRECTON;
    }

    public float GetHorizontalSpacing(){
        if(orientation == Orientation.PointyTop){
            return width;
        }   
        else if(orientation == Orientation.FlapTop){
            return width * 0.75f;
        }
        return 0f;
    }

    public float GetVerticalSpacing(){
        if(orientation == Orientation.PointyTop){
            return height * 0.75f;
        }   
        else if(orientation == Orientation.FlapTop){
            return height;
        }
        return 0f;
    }

    public Vector2 GetLocalPosition(int col, int row){
        float x = 0, y = 0;
        if(offset == Offset.Even){
            x = width/2f + GetHorizontalSpacing()*col + (1- row%2)*width/2f;
        }
        else if(offset == Offset.Odd){
            x = width/2f + GetHorizontalSpacing()*col + (row%2)*width/2f;
        } 

        y = height/2 + GetVerticalSpacing()*row;

        return new Vector2(x,y);
    }

    public Vector2 GetLocalPosition(Vector2Int offset){
        return GetLocalPosition(offset.x, offset.y);
    }

    public Vector2 GetLocalPosition(Vector3Int cube){
        return GetLocalPosition(CubeToOffset(cube));
    }

    public void ResizeParent()
    {
        
    }

    public void SetRange(int columns, int rows){
        this.columns = columns;
        this.rows = rows;

        grid = new T[columns, rows];
        objectGrid = new GameObject[columns, rows];
    }

    public void SetSize(float s){
        this.size = s;

        if(orientation == Orientation.PointyTop){
            this.width = sqrt3 * size;
            this.height = 2 * size;
        }
        else if(orientation == Orientation.FlapTop){
            this.width = 2 * size;
            this.height = sqrt3 * size;
        }
        
    }

    public T Add(){
        var obj = Instantiate(prefab, parent);
        return obj.GetComponent<T>();
    }

    public T Add(int x, int y)
    {
        var obj = Instantiate(prefab, parent);
        obj.name = string.Format("({0:d}, {1:d})", x,y);
        objectGrid[x,y] = obj;

        var hex = obj.GetComponent<T>();
        hex.SetLocalPosition(GetLocalPosition(x,y));
        hex.SetSizeDelta(new Vector2(width, height));
        grid[x,y] = hex;

        hex.x = x;
        hex.y = y;
        var cube = OffsetToCube(x,y);
        hex.q = cube.x;
        hex.r = cube.y;
        hex.s = cube.z;
        
        return hex;
    }

    public T[,] AddAll(){
        for(int i = 0; i<columns; i++){
            for(int j = 0; j<rows; j++){
                Add(i,j);
            }
        }
        return grid;
    }

    public void Clear()
    {
        for(int i = 0; i<columns; i++){
            for(int j = 0; j<rows; j++){
                DeleteAt(i,j);
            }
        }
    }

    public T GetAt(int x, int y)
    {
        if(OutOfRange(x,y) || IsEmpty(x,y)) return null;
        return grid[x,y];
    }

    public T GetAt(Vector2Int pos){
        return GetAt(pos.x, pos.y);
    }

    public GameObject GetObjectAt(int x, int y)
    {
        if(OutOfRange(x,y) || IsEmpty(x,y)) return null;
        return objectGrid[x,y];
    }

    public bool IsEmpty(int x, int y)
    {
        if(OutOfRange(x,y)) return false;
        return grid[x,y]==null;
    }

    public bool OutOfRange(int x, int y){
        return x<0 || y<0 || x>=columns || y>=rows;
    }

    public Vector3Int OffsetToCube(int x, int y){

        int q = 0, r = 0, s = 0;

        // if(offset == Offset.Odd){
        //     if(orientation == Orientation.FlapTop){
        //         q = x - (y - (y&1))/2;
        //         r = y;
        //     }
        //     else{
        //         q = x;
        //         r = y - (x - (x&1))/2;
        //     }
        // }
        // else{
        //     if(orientation == Orientation.FlapTop){
        //         q = x - (y + (y&1))/2;
        //         r = y;
        //     }
        //     else{
        //         q = x;
        //         r = y - (x + (x&1))/2;
        //     }
        // }


        s = y;
        q = x-y/2;
        r = -q-s;

        return new Vector3Int(q,r,s);

    }
    public Vector3Int OffsetToCube(Vector2Int pos){
        return OffsetToCube(pos.x, pos.y);
    }
    public Vector2Int CubeToOffset(int q, int r, int s){
        int col = 0, row = 0;
        // if(offset == Offset.Odd){
        //     if(orientation == Orientation.FlapTop){
        //         col = q + (r-(r&1))/2;
        //         row = r;
        //     }
        //     else{
        //         col = q;
        //         row = r + (q-(q&1))/2;
        //     }
        // }
        // else{
        //     if(orientation == Orientation.FlapTop){
        //         col = q + (r+(r&1))/2;
        //         row = r;
        //     }
        //     else{
        //         col = q;
        //         row = r + (q+(q&1))/2;
        //     }
        // }
        col = (q-r)/2;
        row = s;

        return new Vector2Int(col, row);
    }
    public Vector2Int CubeToOffset(Vector3Int pos){
        return CubeToOffset(pos.x, pos.y, pos.z);
    }

    public Vector3Int GetAdjacentCube(int x, int y, int dir){
        var pos = OffsetToCube(x,y);
        var targetCube = pos + Directions[dir];
        return targetCube;
    }

    public T GetAdjacent(int x, int y, int dir){
        var targetCube = GetAdjacentCube(x,y,dir);
        var targetOffset = CubeToOffset(targetCube);
        return GetAt(targetOffset);
    }

    public T GetAdjacent(T obj, int dir){
        return GetAdjacent(obj.x, obj.y, dir);
    }

    public int GetDistance(int x1, int y1, int x2, int y2){
        var cube1 = OffsetToCube(x1, y1);
        var cube2 = OffsetToCube(x2, y2);
        return (Mathf.Abs(cube1.x - cube2.x) + Mathf.Abs(cube1.y - cube2.y) + Mathf.Abs(cube1.z - cube2.z)) / 2;
    }

    public int GetDistance(T a, T b){
        return (Mathf.Abs(a.q - b.q) + Mathf.Abs(a.r - b.r) + Mathf.Abs(a.s - b.s)) / 2;
    }

    public List<T> GetInRange(T obj, int range){
        var tList = new List<T>();
        int q = obj.q;
        int r = obj.r;
        int s = obj.s;
        for(int i = 0; i<columns; i++){
            for(int j = 0; j<rows; j++){
                var hex = grid[i,j];
                if(hex == null || hex == obj) continue;
                
                int distance = GetDistance(obj, hex);
                if(distance <= range) tList.Add(hex);
            }
        }
        return tList;
    }

    public void Delete(T t)
    {
        Destroy(t.gameObject);
    }

    public void DeleteAt(int x, int y)
    {
        if(OutOfRange(x,y) || IsEmpty(x,y)) return;

        var obj = grid[x,y];
        Delete(obj);

        objectGrid[x,y] = null;
        grid[x,y] = null;
    }


    public void Regular(){

        state = new int[columns, rows];

        int edge = Mathf.Min(columns, rows) / 2;
        var centerPos = new Vector2Int(edge, edge);

        var centerHex = GetAt(centerPos);

        foreach(var hex in grid){
            if(GetDistance(centerHex, hex) <= edge){
                hex.gameObject.SetActive(true);
                state[hex.x, hex.y] = STATE_BLANK;
            }
                
            else {
                hex.gameObject.SetActive(false);
                state[hex.x, hex.y] = STATE_OUT;
                // Debug.Log(hex.x + ", " + hex.y + " is out");
            }
                
        }
    }

    public bool OutOfRange(Vector2Int offset){
        return OutOfRange(offset.x, offset.y);
    }

    public int GetState(Vector2Int offset){
        if(OutOfRange(offset)) return STATE_OUT;
        return state[offset.x, offset.y];
    }

    public int GetState(Vector3Int cube){
        var offset = CubeToOffset(cube);
        return GetState(offset);
    }

    public List<T> GetAllRegular(){

        var list = new List<T>();

        state = new int[columns, rows];

        int edge = Mathf.Min(columns, rows) / 2;
        var centerPos = new Vector2Int(edge, edge);

        var centerHex = GetAt(centerPos);

        foreach(var hex in grid){
            if(GetDistance(centerHex, hex) <= edge){
                list.Add(hex);
            }
        }

        return list;
    }

    public bool OutOfRegular(int x, int y){

        if(OutOfRange(x,y)) return true;

        // int edge = Mathf.Min(columns, rows) / 2;
        // if(GetDistance(edge,edge, x,y) >= edge){
        //     return true;
        // }
        // return false;

        return state[x,y] == STATE_OUT;
    }

    public bool OutOfRegular(T tile){
        return OutOfRange(tile.x, tile.y);
    }
}
}
