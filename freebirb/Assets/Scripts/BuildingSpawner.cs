﻿using System.Collections;
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
    private float initialMaxSpawnDistance = 100.0f;
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
    }

    void Update() {
        // Spawn new buildings if possible.
        if (currentNumBuildings < maxNumBuildings) {
            SpawnBuildings(maxNumBuildings - currentNumBuildings);
        }

        // Despawn old buildings.
        DespawnBuildings();
    }

    private void SpawnInitialBuildings() {
        float spawnAltitude = fogController.playerMaxAltitude - fogController.fogPlayerOffset;

        for (int i = 0; i < initialNumBuildings; ++i) {
            float distance = Random.Range(20.0f, initialMaxSpawnDistance);
            float angle = Random.Range(0.0f, 2.0f * Mathf.PI);
            Vector3 spawnPos = new Vector3(playerTransform.position.x + Mathf.Cos(angle) * distance,
                                           spawnAltitude,
                                           playerTransform.position.z + Mathf.Sin(angle) * distance);
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