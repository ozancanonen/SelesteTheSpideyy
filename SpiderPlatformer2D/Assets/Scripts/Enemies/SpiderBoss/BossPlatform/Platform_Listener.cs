using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Listener : MonoBehaviour
{
    [SerializeField] GameObject stone;
    [SerializeField] float stoneNumber;
    [SerializeField] Transform[] spawnPosition;

    private void Awake()
    {
        Boss.CreateAllStones += DropStones;
    }
    public void DropStones()
    {
        StartCoroutine(SpawnWithDelay());
    }
    public void OnDisable()
    {
        Boss.CreateAllStones -= DropStones;
    }
    IEnumerator SpawnWithDelay()
    {
        for (int i=0;i< stoneNumber; i++)
        {
            Instantiate(stone, spawnPosition[Random.Range(0, spawnPosition.Length - 1)].position,
                Quaternion.Euler(0, 0,10));
            yield return new WaitForSeconds(0.5f);
        }
    }
}
