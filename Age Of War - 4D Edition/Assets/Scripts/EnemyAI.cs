using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int gold;
    public int experience;

    public Unit[] units;

    [Tooltip("Chance to spawn enemy every 'spawnCooldown' seconds")]
    [Range(0f, 1f)]
    public float spawnChance;
    [Tooltip("Cooldown between new chance to spawn enemy in seconds")]
    public float spawnCooldown;

    public GameObject spawner;

    void OnValidate()
    {
        spawnCooldown = Mathf.Max(spawnCooldown, 0.1f);
    }

    void Start()
    {
        StartCoroutine(IncreaseValues(1, 2, 1f));
        StartCoroutine(EnemySpawner(spawnCooldown));
    }

    void Update()
    {
        
    }

    IEnumerator IncreaseValues(int goldIncrease, int expIncrease, float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            gold += goldIncrease;
            experience += expIncrease;
        }
    }

    IEnumerator EnemySpawner(float time)
    {
        while(true)
        {
            yield return new WaitForSeconds(time);

            bool shouldSpawn = Random.Range(0f,1f) < spawnChance;

            if(shouldSpawn)
            {
                int index = Random.Range(0, units.Length);
                if(units[index].cost <= gold)
                {
                    gold -= units[index].cost;
                    Unit currentUnit = units[index];

                    GameObject enemy = new GameObject("Enemy" + currentUnit.name, typeof(UnitControls), typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Animator));
                    enemy.transform.position = transform.position;
                    enemy.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                    enemy.GetComponent<SpriteRenderer>().sprite = currentUnit.artwork;

                    UnitControls enemyScript = enemy.GetComponent<UnitControls>();
                    enemyScript.unitData = currentUnit;
                    enemyScript.isEnemy = true;
                }
            }
        }
    }
}
