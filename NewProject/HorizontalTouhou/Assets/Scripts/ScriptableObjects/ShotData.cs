using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Danmaku.Data
{


    public enum MoveMode
    {
        Straight, Circular, Ball
    };

    public enum WallCheckMode
    {
        Clear, Ignore, Bounce
    }

    [CreateAssetMenu(fileName = "Shot Data", menuName = "DanmakuData/ShotData", order = 0)]
    public class ShotData : ScriptableObject
    {
        public int prefabIndex;
        public float lifetime = 5f;


        public LimitedValue speed = new LimitedValue(1,1,1);
        public float deltaSpeed = 0f;

        public LimitedValue angle = new LimitedValue(0,0,0);
        public float deltaAngle = 0f;


        public MoveMode moveMode =MoveMode.Straight;
        public WallCheckMode wallCheckMode = WallCheckMode.Clear;
        
        public ShotData(int index, float baseSpeed)
        {
            prefabIndex = index;
            speed = new LimitedValue(baseSpeed, baseSpeed, baseSpeed);
            
            // this.baseSpeed = baseSpeed;
        }
    }


}
