using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerators : MonoBehaviour
{
    [SerializeField] private GameObject obstacles;
    [SerializeField] private int cant;
    private Vector3 randomPosition;
    void Start()
    {
        for (int i = 0; i < cant; i++)
        {
            Instantiate(obstacles);
            randomPosition = new Vector3 (Random.Range(-30f, 20f), 1.5f, Random.Range(-20f, 30f));
            obstacles.transform.position = randomPosition;
        }
    }

}
