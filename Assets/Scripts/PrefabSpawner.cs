using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float destroyAfterSeconds = 10f;
    public Vector3 spawnOffset;

    public void InstatiatePrefab()
    {
        GameObject go = Instantiate(prefab, transform.position + spawnOffset, Quaternion.identity);
        if(destroyAfterSeconds > 0)
            Destroy(go, destroyAfterSeconds);
    }
}
