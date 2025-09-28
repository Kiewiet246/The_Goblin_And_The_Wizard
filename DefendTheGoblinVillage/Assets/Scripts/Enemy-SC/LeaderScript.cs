using System.Collections.Generic;
using UnityEngine;

public class LeaderScript : MonoBehaviour
{
    [Header("Target Movement")]
    [SerializeField] private List<TileInfo> waypoints;
    [SerializeField] private Transform leaderTarget;
    [SerializeField] private float distanceToTarget; //How far the Target is
    [SerializeField] private float closeEnough; //How far the leader needs to be to switch target
    
    
    [Header("Followers")]
    [SerializeField] private List<EnemyContoller> followers;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        CheckDistToTarget();
    }

  
    private void CheckDistToTarget()
    {
        if (waypoints.Count > 0 && leaderTarget != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, leaderTarget.position);

            if (distanceToTarget <= closeEnough)
            {
                waypoints.RemoveAt(0);
                if (waypoints.Count > 0)
                {
                    leaderTarget = waypoints[0].transform;
                    AssignTargetForFollowers();           
                }
            }
        }
    }

    public void SetWaypoints(List<TileInfo> newWaypoints)
    {
        if (waypoints != null)
        {
            for (int i = 0; i < newWaypoints.Count; i++)
            {
                waypoints.Add(newWaypoints[i]);
            }
            
            leaderTarget = waypoints[0].transform;
            AssignTargetForFollowers();
        }
        
    }

    private void AssignTargetForFollowers()
    {
        if (followers != null)
        {
            for (int i = 0; i < followers.Count; i++)
            {
                followers[i].SetTarget(leaderTarget);
            }
        }
        else
        {
            Debug.Log("Null Follows");
        }
    }
    
}
