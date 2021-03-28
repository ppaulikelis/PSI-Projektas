using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuperAttack : MonoBehaviour
{
    public GameObject superAttackObjectPrefab;
    public Button superAttackButton;
    public int numberOfObjects = 10;
    public float objectRespawnTime = 1.0f;
    public float attackRespawnTime = 30.0f;
    public bool isReady = true;

    IEnumerator SuperAttackPreparation(float time)
    {
        yield return new WaitForSeconds(time);
        isReady = true;
        superAttackButton.interactable = true;
    }

    private void SpawnObject()
    {
        GameObject a = Instantiate(superAttackObjectPrefab) as GameObject;
        a.transform.position = new Vector2(Random.Range(-5, 35), 10);
    }

    IEnumerator ObjectWave()
    {
        int spawnedObjects = 0;
        while(spawnedObjects != numberOfObjects)
        {
            yield return new WaitForSeconds(objectRespawnTime);
            SpawnObject();
            spawnedObjects++;
        }
    }

    public void StartAttack()
    {
        if (isReady)
        {
            superAttackButton.interactable = false;
            isReady = false;
            StartCoroutine(ObjectWave());
            StartCoroutine(SuperAttackPreparation(attackRespawnTime));
        }
    }
}
