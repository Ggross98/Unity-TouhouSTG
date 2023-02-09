using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Danmaku.Data
{



    public enum OffsetType{
        None,
        Linear,
        Sin
    }


    [CreateAssetMenu(fileName = "Barrage Data", menuName = "DanmakuData/BarrageData", order = 0)]
    public class BarrageData: ScriptableObject
    {
        public ShotData shotData;

        /// <summary>
        /// 从符卡开始后到释放第一枚弹幕的延迟
        /// </summary>
        public float startDelay;

        /// <summary>
        /// 两轮射击之间的间隔
        /// </summary>
        public float interval;

        /// <summary>
        /// 每次射击的信息
        /// </summary>
        public FireData fireData;

        /// <summary>
        /// 射击角度变化的周期
        /// </summary>
        // public int fireOffsetCycle;

        public FireOffset fireOffset;

        


        public BarrageData(ShotData shotData, FireData fd, float interval, float delay = 0f)
        {
            this.shotData = shotData;
            this.fireData = fd;
            this.interval = interval;
            this.startDelay = delay;
        }

    }

    [System.Serializable]
    public class FireOffset{

        public int cycle;

        public int startCycleIndex;

        public float range = 360f;
        // public float startAngle


        /// <summary>
        /// 射击角度变化的计算方法
        /// </summary>
        public OffsetType type = OffsetType.Linear;

        /// <summary>
        /// 射击角度变化往复或回原位
        /// </summary>
        public bool reciprocate = false;
    }

}