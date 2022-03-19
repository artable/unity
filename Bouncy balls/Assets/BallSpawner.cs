using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Transform ball;

    [Range(0,3)] public float spawnFreq;

    private float timeElapsed;

    private Transform spawnerLocation;
    private float[] xRange = new float[2];
    private float[] yRange = new float[2];
    private float[] zRange = new float[2];
    // Start is called before the first frame update

    void Start()
    {
        spawnerLocation = GetComponent<Transform>();

        xRange[0] = spawnerLocation.position.x - (spawnerLocation.localScale.x/2);
        xRange[1] = spawnerLocation.position.x + (spawnerLocation.localScale.x/2);
        yRange[0] = spawnerLocation.position.y - (spawnerLocation.localScale.y/2);
        yRange[1] = spawnerLocation.position.y + (spawnerLocation.localScale.y/2);
        zRange[0] = spawnerLocation.position.z - (spawnerLocation.localScale.z/2);
        zRange[1] = spawnerLocation.position.z + (spawnerLocation.localScale.z/2);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= spawnFreq)
        {
            float x;
            float y;
            float z;
            x = Random.Range(xRange[0],xRange[1]);
            y = Random.Range(yRange[0],yRange[1]);
            z = Random.Range(zRange[0],zRange[1]);
            Instantiate(ball, new Vector3(x,y,z), Quaternion.identity);
            timeElapsed = 0;
        }
    }
}
