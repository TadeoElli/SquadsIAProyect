using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavLeader : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask floor;
    public UnityEngine.AI.NavMeshAgent agent;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, floor))
                agent.SetDestination(raycastHit.point);
        }
    }
}
