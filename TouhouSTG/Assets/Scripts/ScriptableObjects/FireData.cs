using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Danmaku.Data
{

    public enum FireType
    {
        
        /// <summary>
        /// 单发射击
        /// </summary>
        // Single,

        /// <summary>
        /// 圆周射击
        /// </summary>
        Round,

        /// <summary>
        /// 扇形射击
        /// </summary>
        Sector,

        /// <summary>
        /// 霰弹型
        /// </summary>
        Spray
    }

    public enum DirectionType
    {
        Fixed,

        Aimed,

        Random
    }

    public enum ShotOperationType
    {
        ChangeDirectionAndSpeed
    }

    [CreateAssetMenu(fileName = "Fire Data", menuName = "DanmakuData/FireData", order = 0)]
    public class FireData : ScriptableObject
    {   
        /// <summary>
        /// 子弹初始朝向
        /// </summary>
        public Vector3 startDir = Vector3.left;

        /// <summary>
        /// 是否为自机狙，若为true，则startDir会变为指向自机方向
        /// </summary>
        // public bool aimed = false;

        /// <summary>
        /// 方向类型
        /// </summary>
        public DirectionType directionType = DirectionType.Fixed;

        /// <summary>
        /// 距初始朝向的偏移角度
        /// </summary>
        public float startAngle = 0f;
        
        /// <summary>
        /// 延发射方向，离发射点的距离
        /// </summary>
        public float startDistance = 0f;
        
        /// <summary>
        /// 单次射击数量
        /// </summary>
        [Range(1, 50)]public int count = 1;

        public Vector3 posDir;
        public float posStartDistance;
        

        /// <summary>
        /// 根据射击次数计算偏移角度的方法，默认值为0（即不偏移）
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        // public delegate float OffsetAngle(int count);
        // public OffsetAngle offsetAngle;
        // public float DefaultOffsetAngle(int count)
        // {
        //     return 0f;
        // }

        /// <summary>
        /// 对子弹延迟执行的事件，默认为无
        /// </summary>
        /// <param name="shotList"></param>
        // public delegate void ShotOperation(List<Shot> shotList);
        // public ShotOperation shotOperation;
        // public float shotOperationDelay;
        // public void DefaultShotOperation(List<Shot> shotList)
        // {
        //     return;
        // }
        public List<DelayOperation> delayOperations;
        // public float sectorDeltaAngle = 0f;

        public FireType type = FireType.Round;

        public FireRound round;
        public FireSector sector;
        public FireSpray spray;
        

        /// <summary>
        /// 连续射击设定
        /// </summary>
        public FireGroupData group;


        






        // public FireData(Vector3 dir, float dist = 0f)
        // {
        //     this.startDir = dir;
        //     aimed = dir == Vector3.zero;

        //     // offsetAngle = DefaultOffsetAngle;
        //     shotOperation = DefaultShotOperation;

        //     this.startDistance = dist;
        // }


    }

    [System.Serializable]
    public class FireGroupData
    {
        /// <summary>
        /// 单词发射组数，默认为1
        /// </summary>
        [Range(1, 20)]public int num = 1;

        /// <summary>
        /// 组内发射时间间隔
        /// </summary>
        public float interval = 0.1f;

        /// <summary>
        /// 组内距离偏移
        /// </summary>
        public float deltaDistance = 0f;

        /// <summary>
        /// 组内角度偏移
        /// </summary>
        public float deltaAngle = 0f;

        
        
        public float posDeltaAngle = 0f;
        public float posDeltaDistance = 0f;





        public FireGroupData()
        {
            num = 1;
            interval = 0f;
        }

        public FireGroupData(int count, float interval)
        {
            this.num = count;
            this.interval = interval;
        }
    }

    [System.Serializable]
    public class FireRound{
        
    }

    [System.Serializable]
    public class FireSector{
        public float deltaAngle;
    }

    [System.Serializable]
    public class FireSpray{
        public LimitedValue fire;
        public LimitedValue angle;
        // public float rate;
    }



    [System.Serializable]
    public class DelayOperation{
        public ShotOperationType type = ShotOperationType.ChangeDirectionAndSpeed;
        public float delay;
        

        public DirectionType directionType;
        public Vector2 direction;

        public LimitedValue speed;
        public float deltaSpeed;

        public LimitedValue angle;
        public float deltaAngle;
    }


    // public class FireRoundData : FireData
    // {

    //     // public float startAngle;
    //     // public int count;

    //     public FireRoundData(Vector3 dir, float dist, float startAngle, int fire)
    //         : base(dir, dist)
    //     {
    //         this.startAngle = startAngle;
    //         this.count = fire;

    //         this.type = FireType.Round;
    //     }
    // }

    // public class FireTurbineData : FireData
    // {

    //     // public float startAngle, deltaAngle;
    //     // public float deltaDistance;
    //     // public int count;
    //     // public float delay;
    //     public FireRoundData frd;


    //     public FireTurbineData(Vector3 dir, float dist, float deltaDist, float startAngle, float deltaAngle, int fire, float delay, FireRoundData frd)
    //         : base(dir, dist)
    //     {
    //         this.group.deltaDistance = deltaDist;

    //         this.startAngle = startAngle;
    //         this.group.deltaAngle = deltaAngle;

    //         this.count = fire;
    //         this.shotOperationDelay = delay;

    //         this.frd = frd;

    //         this.type = FireType.Turbine;
    //     }
    // }
}