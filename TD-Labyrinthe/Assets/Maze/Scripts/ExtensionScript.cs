using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class ExtensionScript 
{
    // Start is called before the first frame update
   /* void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
   //Getting the path distance between NavMeshAgent position and given position
    public static float GetPathRemainingDistance(this NavMeshAgent navMeshAgent)
    {
        //Debug.Log("path pending is " + navMeshAgent.pathPending + ", path status is " + navMeshAgent.pathStatus + ", path corners length is " + navMeshAgent.path.corners.Length);
        if (navMeshAgent.pathPending ||
            navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
            navMeshAgent.path.corners.Length == 0)
            return -1f;
        float distance = 0.0f;
        for (int i = 0; i < navMeshAgent.path.corners.Length-1; ++i)
        {
            distance += Vector3.Distance(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1]);
        }
        return distance;

    }

}
