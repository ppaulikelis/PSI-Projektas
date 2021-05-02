using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int gold;
    public int experience;
    public float cooldown;

    [Tooltip("Chances for each unit to spawn (e.g. 0.25 = 25%)")]
    public float[] chances= new float[3];
    [Tooltip("Chance to spawn enemy every 'spawnCooldown' seconds")]
    [Range(0f, 1f)]
    public float spawnChance;
    [Tooltip("Cooldown between new chance to spawn enemy in seconds")]
    public float spawnCooldown;

    public GameObject spawner;
    public Unit[] units;

    void OnValidate()   // validation of public variables changed in inspector
    {
        spawnCooldown = Mathf.Max(spawnCooldown, 0.1f);
        if(chances.Length != 3)
        {
            Debug.LogWarning("Don't change length of chances array in EnemyAI");
            Array.Resize(ref chances, 3);
        }
    }

    void Start()
    {
        // starting automatic increase of gold/experience and unit spawning
        StartCoroutine(IncreaseValues(1, 2, 1f));
        StartCoroutine(EnemySpawner(spawnCooldown));
    }

    void Update()
    {
        if(cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
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

            bool shouldSpawn = UnityEngine.Random.Range(0f,1f) < spawnChance;   // adding randomness to spawning

            if(shouldSpawn && cooldown <= 0)
            {
                int index = GetIndexFromChances(chances, 0);    // generate random index according to chances array
                if(units[index].cost <= gold)
                {
                    Unit currentUnit = units[index];
                    cooldown = currentUnit.trainingTime;

                    // creating enemy unit GameObject and assigning various variables
                    GameObject enemy = new GameObject("Enemy" + currentUnit.name, typeof(UnitControls), typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(Animator));
                    enemy.transform.position = transform.position;
                    enemy.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                    enemy.GetComponent<SpriteRenderer>().sprite = currentUnit.artwork;

                    UnitControls enemyScript = enemy.GetComponent<UnitControls>();
                    enemyScript.unitData = currentUnit;
                    enemyScript.isEnemy = true;

                    gold -= currentUnit.cost;
                }
            }
        }
    }

    private int GetIndexFromChances(float[] chances, int defaultIndex = 0)
    {
        float current = UnityEngine.Random.Range(0f, 1f);
        if (current <= chances[0]) return 0;
        else if (current <= chances[0] + chances[1]) return 1;
        else if (current <= chances[0] + chances[1] + chances[2]) return 2;
        else return defaultIndex;
    }
}
