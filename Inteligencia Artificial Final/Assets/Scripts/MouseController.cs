using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask floor,nodes;
    [SerializeField] private Leader leader1, leader2;
    Vector3 worldMousePosition;
    float minDistance;
    Collider minCollider;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //minCollider = null;
            minDistance = 10f;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, floor))
                worldMousePosition = raycastHit.point;
                //Debug.Log("Mouse "+ worldMousePosition);
                SearchForNode(worldMousePosition);
        }
            
    }

    void SearchForNode(Vector3 mousePosition)
    {
        Collider[] hitColliders = Physics.OverlapSphere(mousePosition, 5f, nodes);
        foreach (var hitCollider in hitColliders)
        {
            Vector3 pos = hitCollider.GetComponent<Transform>().position;
            //Debug.Log(pos +"" + hitCollider);
            if(Vector3.Distance(worldMousePosition, pos) < minDistance)
            {
                minDistance = Vector3.Distance(worldMousePosition, pos);
                minCollider = hitCollider;
            }
        }
        leader1.SetGoalNode(minCollider.GetComponent<Node>());
        leader2.SetGoalNode(minCollider.GetComponent<Node>());
    }
}
