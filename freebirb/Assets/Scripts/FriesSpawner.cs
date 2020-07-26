using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriesSpawner : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private GameObject friesPrefab;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private int maxNumFries;
    [SerializeField]
    private float minSpawnDistance;
    [SerializeField]
    private float maxSpawnDistance;
    [SerializeField]
    private float despawnThresholdDistance = 100.0f;
#pragma warning restore

    private int currentNumFries { get => fries.Count; }
    private List<GameObject> fries;
    private float height;

    void Start() {
        fries = new List<GameObject>();
        height = playerTransform.position.y;
    }

    void Update() {
        // Spawn new fries if possible.
        if (currentNumFries < maxNumFries) {
            SpawnFries(maxNumFries - currentNumFries);
        }

        // Despawn old fries.
        DespawnFries();

        // Lower max number of fries as the player gets higher
        if (playerTransform.position.y > height + 100f) {
            maxNumFries = Mathf.Clamp(maxNumFries - 1, 5, 50);
            height = playerTransform.position.y;
        }
    }

    private void SpawnFries(int num) {
        for (int i = 0; i < num; ++i) {
            Vector3 spawnPos = HelperFunctions.RandomPointInSphereExcludeInner(playerTransform.position, maxSpawnDistance, minSpawnDistance);
            GameObject newFry = Instantiate(friesPrefab, spawnPos, Quaternion.identity, transform);
            fries.Add(newFry);
        }
    }

    // Despawn fries that are too far away.
    private void DespawnFries() {
        for (int i = currentNumFries - 1; i >= 0; --i) {
            GameObject fry = fries[i];
            if (fry != null && Vector3.Distance(fry.transform.position, playerTransform.position) > despawnThresholdDistance) {
                RemoveFry(fry);
            }
        }
    }

    public void RemoveFry(GameObject fry) {
        fries.Remove(fry);
        Destroy(fry);
    }
}
