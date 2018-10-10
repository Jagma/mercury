using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public static EnemyManager instance;

    private void Awake() {
        instance = this;
    }

    List<Enemy> enemyList = new List<Enemy>();
	
	void Update () {
		for (int i=0; i < enemyList.Count; i ++) {
            if (enemyList[i] == null) {
                enemyList.RemoveAt(i);
                i--;
            }
        }
	}

    public void AddEnemy (Enemy enemy) {
        enemyList.Add(enemy);
    }

    public int GetEnemyCount () {
        return enemyList.Count;
    }
}
