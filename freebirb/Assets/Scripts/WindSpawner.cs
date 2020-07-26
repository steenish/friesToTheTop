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

    void Start() {
        winds = new List<GameObject>();
    }

    void Update() {
        // Spawn new winds if possible.
        if (currentNumWinds < maxNumWinds) {
            SpawnWinds(maxNumWinds - currentNumWinds);
        }

        // Despawn old winds.
        DespawnWinds();
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
