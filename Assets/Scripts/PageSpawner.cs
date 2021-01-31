using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSpawner : SingletonPattern<PageSpawner>
{

    [SerializeField] private int minSpawnRange = 3;

    [Header("Max score range MUST be larger than min score range.")]
    [SerializeField] private int maxSpawnRange = 10;

    [Header("List of papers in the world that can be spawned.")]
    public List<GameObject> spawnPositions = new List<GameObject>();

    [HideInInspector] public List<GameObject> spawnedPapers = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        foreach (Transform paper in transform)
        {
            if (!spawnPositions.Contains(paper.gameObject))
            {
                spawnPositions.Add(paper.gameObject);
            }
        }

        foreach (GameObject paper in spawnPositions)
        {
            paper.SetActive(false);
        }

        UIManager.Instance.maxScore = Random.Range(minSpawnRange, maxSpawnRange);

        int maxAmount = UIManager.Instance.maxScore;

        if (maxAmount >= spawnPositions.Count)
        {
            spawnedPapers = spawnPositions;

            UIManager.Instance.maxScore = spawnPositions.Count;

            foreach (GameObject paper in spawnedPapers)
            {
                paper.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < maxAmount; i++)
            {
                while (true)
                {
                    int index = Random.Range(0, maxAmount);

                    if (spawnedPapers.Count == 0 || !spawnedPapers.Contains(spawnPositions[index]))
                    {
                        spawnedPapers.Add(spawnPositions[index]);
                        break;
                    }
                }
            }

            foreach (GameObject paper in spawnedPapers)
            {
                paper.SetActive(true);
            }

        }

        UIManager.Instance.Score = 0;
    }
}
