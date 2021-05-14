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
    [Tooltip("Chance to spawn tower(0)/turret(1)")]
    public float[] towerTurretChances = new float[2];
    [Tooltip("Chance to spawn enemy every 'spawnCooldown' seconds")]
    [Range(0f, 1f)]
    public float spawnChance;
    [Tooltip("Chance to spawn enemy tower(0)/turret(1) every 'spawnCooldown' seconds")]
    [Range(0f, 1f)]
    public float towerTurretSpawnChance;
    [Tooltip("Cooldown between new chance to spawn enemy in seconds")]
    public float spawnCooldown;

    int towerCount = 0;
    int turretCount = 0;

    public GameObject spawner;
    public Tower tower;
    public GameObject towerBase;
    public GameObject bullet;
    public Turret turretData;
    public Unit[] units;

    void OnValidate()   // validation of public variables changed in inspector
    {
        spawnCooldown = Mathf.Max(spawnCooldown, 0.1f);
        if (chances.Length != 3)
        {
            Debug.LogWarning("Don't change length of chances array in EnemyAI");
            Array.Resize(ref chances, 3);
        }
        if(towerTurretChances.Length != 2)
        {
            Debug.LogWarning("Don't change length of towerChances array in EnemyAI");
            Array.Resize(ref towerTurretChances, 2);
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

            if (UnityEngine.Random.Range(0f, 1f) < towerTurretSpawnChance)  // adding random tower/turret spawning
            {
                SpawnTowerOrTurret(towerTurretChances);
            }

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

    private void SpawnTowerOrTurret(float[] chances)
    {
        float current = UnityEngine.Random.Range(0f, 1f);
        if (current <= chances[0])  // build tower
        {
            if(towerCount < 3 && gold >= tower.cost[towerCount])
            {
                gold -= tower.cost[towerCount];

                GameObject newTower = new GameObject("Enemy Tower" + towerCount, typeof(SpriteRenderer));     // tower creation
                newTower.GetComponent<SpriteRenderer>().sprite = tower.artwork;
                newTower.transform.position = new Vector3(towerBase.transform.position.x, tower.yPlacement[towerCount], 0);
                newTower.transform.SetParent(towerBase.gameObject.transform);
                towerCount++;
            }
         
        }
        else if(current <= chances[0] + chances[1]) // build turret (if tower exists)
        {
            if(gold >= turretData.price && towerBase.transform.childCount > turretCount && turretCount < 3)
            {
                gold -= turretData.price;

                GameObject turret = new GameObject("Enemy turret", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(TurretController));  // turret creation
                turret.transform.SetParent(towerBase.transform.GetChild(turretCount));
                TurretController script = turret.GetComponent<TurretController>();  // setting up TurretController script using turretData
                script.turretData = turretData;
                script.UpdateVariables();
                Transform newShootpoint = new GameObject("Shootpoint").transform;
                newShootpoint.SetParent(turret.transform);
                script.shootPoint = newShootpoint;
                script.shootPoint.localPosition = new Vector2(-turretData.shootpointOffsetX, 0);
                script.targetTag = "Friendly";
                script.bullet = bullet;

                turret.transform.localPosition = Vector2.zero;  // moving turret to middle of tower block

                SpriteRenderer rend = turret.GetComponent<SpriteRenderer>();    // setting up sprite (half transparent, sorting order)
                rend.sprite = turretData.artwork;
                rend.sortingOrder = towerCount + 1;
                rend.flipX = true;
                turret.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                turret.GetComponent<BoxCollider2D>().isTrigger = true;

                turretCount++;
            }
        }
        else
        {
            return;
        }
    }
}
