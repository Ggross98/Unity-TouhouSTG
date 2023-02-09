using UnityEngine;
using System;
using System.Collections.Generic;

namespace Danmaku.Data
{

    public enum DanmakuType
    {
        SpellCard,
        SurvivalCard,
        NonSpellCard
    }

    public enum DanmakuMoveType
    {
        None,
        RandomMove
    }

    public enum DanmakuAnimation
    {
        None,
        AttackBeforeFire
    }

    [CreateAssetMenu(fileName = "Danmaku Data", menuName = "DanmakuData/DanmakuData", order = 0)]
    public class DanmakuData: ScriptableObject
    {
        public string danmakuName;

        public List<BarrageData> data;

        // public DanmakuMoveType moveType = DanmakuMoveType.None;
        public DanmakuType type = DanmakuType.SpellCard;
        public DanmakuAnimation animation = DanmakuAnimation.None;

        public DanmakuMove move;
        
        /// <summary>
        /// For normal SC and non SC
        /// </summary>
        public int hp;
        /// <summary>
        /// For Survival Card
        /// </summary>
        public float survivalTime;


        

        // public float minX, maxX, minY, maxY;


        public void AddBarrage(BarrageData bd)
        {
            data.Add(bd);
        }

    }
    
    [System.Serializable]
    public class DanmakuMove
    {
        public DanmakuMoveType type;
        public Vector3 startPosition;
        public float startDelay;
        public float minX, maxX, minY, maxY;
        public float interval, delay, duration;
    }

}
