using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSpawner : SingletonPattern<PageSpawner>
{
    public List<GameObject> spawnPositions = new List<GameObject>();

    [HideInInspector] public List<GameObject> spawnedPapers = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        int maxAmount = UIManager.Instance.maxScore;

        if (maxAmount >= spawnPositions.Count)
        {
            spawnedPapers = spawnPositions;

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
    }
}
