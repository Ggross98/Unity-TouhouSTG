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
        public float baseSpeed;
        public MoveMode moveMode;
        public WallCheckMode wallCheckMode;
        public float deltaSpeed = 0f;
        public float deltaAngle = 0f;

        public ShotData(int index, float speed, MoveMode move = MoveMode.Straight, WallCheckMode wallCheck = WallCheckMode.Clear)
        {
            prefabIndex = index;
            baseSpeed = speed;
            moveMode = move;
            wallCheckMode = wallCheck;
        }
    }


}
