using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ggross.Pattern;

public class DanmakuTest : SingletonMonoBehaviour<DanmakuTest>
{
    [SerializeField] PlayerController player;
    [SerializeField] EnemyController enemy;

    private void Start() {
        player.StartBattle();
        enemy.StartBattle();
    }
}
