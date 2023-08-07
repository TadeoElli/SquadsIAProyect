using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public List<Node> _neighbors = new List<Node>();

    public int cost = 1;
}
