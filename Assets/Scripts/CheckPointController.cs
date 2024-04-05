using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public GameObject lastPoint;
    public int lap = 0;
    public int checkPoint = -1;
    int pointCount = 0;
    public int nextPoint;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        pointCount = checkpoints.Length;
        
        for(int i = 0; i < pointCount; i++)
        {
            if(checkpoints[i].name == "0")
            {
                lastPoint = checkpoints[i];
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Checkpoint")
        {
            int thisPoint = int.Parse(other.gameObject.name);

            if(thisPoint == nextPoint)
            {
                lastPoint = other.gameObject; 
                checkPoint = thisPoint;
                if(checkPoint == 0)
                {
                    lap++;
                    print("Lap: " + lap);
                }

                nextPoint++;
                nextPoint = nextPoint % pointCount;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
