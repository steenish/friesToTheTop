using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpawner : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private GameObject windPrefab;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private int maxNumWinds;
    [SerializeField]
    private float minSpawnDistance;
    [SerializeField]
    private float maxSpawnDistance;
    [SerializeField]
    private float despawnThresholdDistance = 100.0f;
#pragma warning restore

    private int currentNumWinds { get => winds.Count; }
    private List<GameObject> winds;
    private float height;

    void Start() {
        winds = new List<GameObject>();
        height = playerTransform.position.y;

        InvokeRepeating("DespawnWinds", 1.0f, 0.5f);
    }

    void Update() {
        // Spawn new winds if possible.
        if (currentNumWinds < maxNumWinds) {
            SpawnWinds(maxNumWinds - currentNumWinds);
        }

        // Lower max number of winds as the player gets higher
        if (playerTransform.position.y > height + 100f) {
            maxNumWinds = Mathf.Clamp(maxNumWinds - 1, 1, 20);
            height = playerTransform.position.y;
        }
    }

    private void SpawnWinds(int num) {
        for (int i = 0; i < num; ++i) {
            Vector3 spawnPos = HelperFunctions.RandomPointInSphereExcludeInner(playerTransform.position, maxSpawnDistance, minSpawnDistance);
            GameObject newWind = Instantiate(windPrefab, spawnPos, Quaternion.identity, transform);
            winds.Add(newWind);
        }
    }

    // Despawn winds that are too far away.
    private void DespawnWinds() {
        for (int i = currentNumWinds - 1; i >= 0; --i) {
            GameObject wind = winds[i];
            if (wind != null && Vector3.Distance(wind.transform.position, playerTransform.position) > despawnThresholdDistance) {
                winds.Remove(wind);
                Destroy(wind);
            }
        }
    }
}
