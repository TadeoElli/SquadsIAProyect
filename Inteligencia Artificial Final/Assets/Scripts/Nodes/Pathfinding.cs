using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    

    float Heuristic(Node currentNode, Node goalNode)
    {
        return Mathf.Abs(currentNode.transform.position.x - goalNode.transform.position.x) +
                    Mathf.Abs(currentNode.transform.position.z - goalNode.transform.position.z);
    }

    public Stack<Node> AStar(Node startingNode, Node goalNode)
    {
        Stack<Node> path = new Stack<Node>();

        if (startingNode == null || goalNode == null) return path;

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                while (current != null)
                {
                    path.Push(current);
                    current = cameFrom[current];
                }

                return path;
            }

            foreach (var next in current._neighbors)
            {
                //if (next.IsBlocked) continue;

                int newCost = costSoFar[current] + next.cost;

                float priority = newCost + Heuristic(next, goalNode);

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (newCost < costSoFar[next])
                {
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }

        return path;
    }
    public List<Node> ThetaStar(Node startingNode, Node goalNode)
    {
        if (startingNode == null || goalNode == null) return new List<Node>();

        List<Node> path = new List<Node>(AStar(startingNode, goalNode));

        int current = 0;

        while (current + 2 < path.Count)
        {
            //Si vemos ese nodo current + 2
            if (InLineOfSight(start: path[current].transform.position, end: path[current + 2].transform.position))
            {
                //Removemos el current + 1
                path.RemoveAt(current + 1);

            }
            else //Sino
            {
                current++;
            }
        }

        return path;
    }

    bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        RaycastHit hit;
        //Origen, direccion, distancia maxima y layer mask
        return !Physics.SphereCast(start,0.5f, dir, out hit, dir.magnitude, LayerMask.GetMask("Objects"));
    }
}
