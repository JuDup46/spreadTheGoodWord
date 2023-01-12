using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemySpawner : MonoBehaviour
{

    public Ennemy ennemyPrefab;

    public float trajectoryVariance = 15.0f;
    public float spawnRate = 2.0f;

    public float spawnDistance = 15.0f;

    public int spawnAmount = 1;
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        if (GameManager.Instance.gameState != GameState.playing) return;

        for (int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            float variance = Random.Range(-trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Ennemy ennemy = Instantiate(this.ennemyPrefab, spawnPoint, rotation);
            ennemy.size = Random.Range(ennemy.minSize, ennemy.maxSize);

            ennemy.SetTrajectory(rotation * -spawnDirection);
        }
    }
}



