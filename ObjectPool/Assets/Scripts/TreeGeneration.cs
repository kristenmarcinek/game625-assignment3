using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TreeGeneration : MonoBehaviour
{
    public Transform playerTransform;
    public float minSpawnDistance = 5f;
    public float maxSpawnDistance = 20f;
    public float minPeripheralAngle = 30f;
    public float maxPeripheralAngle = 60f;
    public GameObject treePrefab;

    private ObjectPool<GameObject> treePool;

    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 15; // Updated maxSize to 15

    private List<GameObject> spawnedTrees = new List<GameObject>();

    private void Start()
    {
        // Create the object pool for trees
        treePool = new ObjectPool<GameObject>(() => Instantiate(treePrefab), null, DestroyTree, null, collectionCheck, defaultCapacity, maxSize);
    }
    
    private void Update()
    {
        // Get the player's forward direction
        Vector3 playerDirection = playerTransform.forward;

        // Generate random distance and angle from the player
        float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        float peripheralAngle = Random.Range(minPeripheralAngle, maxPeripheralAngle);

        // Calculate the position to spawn the tree
        Vector3 spawnPosition = playerTransform.position + playerDirection * spawnDistance;
        float randomAngle = Random.Range(-peripheralAngle, peripheralAngle);
        spawnPosition = Quaternion.Euler(0f, randomAngle, 0f) * spawnPosition;

        // Spawn the tree from the object pool
        GameObject tree = treePool.Get();
        tree.transform.position = spawnPosition;
        tree.SetActive(true);

        // Add the spawned tree to the list
        spawnedTrees.Add(tree);

        // Destroy trees that are behind the player and return them to the object pool
        for (int i = 0; i < spawnedTrees.Count; i++)
        {
            GameObject spawnedTree = spawnedTrees[i];
            if (Vector3.Angle(playerDirection, spawnedTree.transform.position - playerTransform.position) > peripheralAngle)
            {
                if (spawnedTree.activeSelf)
                {
                    spawnedTree.SetActive(false);
                    treePool.Release(spawnedTree);
                }
            }
        }

        // Destroy trees that exceed the default capacity and return them to the object pool
        if (spawnedTrees.Count > defaultCapacity)
        {
            GameObject treeToDestroy = spawnedTrees[0];
            if (treeToDestroy.activeSelf)
            {
                treeToDestroy.SetActive(false);
                treePool.Release(treeToDestroy);
            }
            spawnedTrees.RemoveAt(0);
        }
    }

    private void DestroyTree(GameObject tree)
    {
        // Remove the destroyed tree from the list
        spawnedTrees.Remove(tree);
    }
}
