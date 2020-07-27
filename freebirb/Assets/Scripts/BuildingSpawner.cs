using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private GameObject[] buildingPrefabs;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private FogController fogController;
    [SerializeField]
    private int initialNumBuildings;
    [SerializeField]
    private int maxNumBuildings;
    [SerializeField]
    private float initialMinSpawnDistance = 30.0f;
    [SerializeField]
    private float initialMaxSpawnDistance = 100.0f;
    [SerializeField]
    private float spawnMaxPerturbance = 10.0f;
    [SerializeField]
    private float spawnRadiusOffset = 100.0f;
    [SerializeField]
    private float despawnAltitudeOffset = 0.0f;
    [SerializeField]
    private float despawnThresholdTotalOffset = 100.0f;
#pragma warning restore

    private float spawnRadius;
    private float despawnThresholdTotal;
    private List<GameObject> buildings;
    private int currentNumBuildings { get { return buildings.Count; } }

    void Start() {
        spawnRadius = RenderSettings.fogEndDistance + spawnRadiusOffset;
        despawnThresholdTotal = spawnRadius + despawnThresholdTotalOffset;

        buildings = new List<GameObject>();

        SpawnInitialBuildings();

        InvokeRepeating("DespawnBuildings", 1.0f, 0.5f);
    }

    void Update() {
        // Spawn new buildings if possible.
        if (currentNumBuildings < maxNumBuildings) {
            SpawnBuildings(maxNumBuildings - currentNumBuildings);
        }
    }

    private void SpawnInitialBuildings() {
        float spawnAltitude = fogController.playerMaxAltitude - fogController.fogPlayerOffset + Random.Range(-spawnMaxPerturbance, spawnMaxPerturbance);

        for (int i = 0; i < initialNumBuildings; ++i) {
            Vector3 spawnPos = HelperFunctions.RandomPointInCircleExcludeInner(new Vector3(playerTransform.position.x, spawnAltitude, playerTransform.position.z), initialMaxSpawnDistance, initialMinSpawnDistance);
            GameObject newBuilding = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)], spawnPos, Quaternion.identity, transform);
            buildings.Add(newBuilding);
        }
    }

    private void SpawnBuildings(int num) {
        float spawnAltitude = fogController.playerMaxAltitude - fogController.fogPlayerOffset;

        for (int i = 0; i < num; ++i) {
            float angle = Random.Range(0.0f, 2.0f * Mathf.PI);
            Vector3 spawnPos = new Vector3(playerTransform.position.x + Mathf.Cos(angle) * spawnRadius,
                                           spawnAltitude,
                                           playerTransform.position.z + Mathf.Sin(angle) * spawnRadius);
            GameObject newBuilding = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Length)], spawnPos, Quaternion.identity, transform);
            buildings.Add(newBuilding);
        }
    }

    // Despawn buildings that are either too far away in total or below the despawn altitude.
    private void DespawnBuildings() {
        for (int i = currentNumBuildings - 1; i >= 0; --i) {
            GameObject building = buildings[i];
            if (building.transform.position.y < fogController.playerMaxAltitude - fogController.fogPlayerOffset - despawnAltitudeOffset ||
                Vector3.Distance(building.transform.position, playerTransform.position) > despawnThresholdTotal) {
                buildings.Remove(building);
                Destroy(building);
            }
        }
    }
}
