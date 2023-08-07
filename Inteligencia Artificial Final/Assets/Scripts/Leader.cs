using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    // Start is called before the first frame update
    FSM<LeaderStates> _FSM;

    Material _myMaterial;
    [SerializeField]Color _originalColor;

    [SerializeField] public float _maxSpeed, life, maxLife;
    private float timer;
    Collider minCollider;
    [SerializeField] private LayerMask nodes;
    [Header("FOV")]
    [SerializeField] public float _viewRadius;
    [SerializeField] public float _viewAngle;
    [SerializeField] public LayerMask obstacleLayer;

    public Node _startingNode, _goalNode;
    public bool _startSearch = false;
    public bool _hasReachNode = false;

    List<Node> _pathToFollow;
    Pathfinding _pathfinding;


    void Awake()
    {
        _FSM = new FSM<LeaderStates>();

        _myMaterial = GetComponent<Renderer>().material;

        _pathToFollow = new List<Node>();
        _pathfinding = new Pathfinding();
        maxLife = 100;
        life = maxLife;
        _myMaterial.color = _originalColor;

        IState idle = new LeaderIdleState(_FSM, this);
        _FSM.AddState(LeaderStates.Idle, new LeaderIdleState(_FSM, this));

        _FSM.AddState(LeaderStates.Search, new LeaderSearchState(_FSM, this, _pathToFollow, _pathfinding));

        _FSM.ChangeState(LeaderStates.Idle);
    }


    void Update()
    {
        _FSM.Update();
        _FSM.FixedUpdate();
    }

    public void SetGoalNode(Node node)
    {
        SetStartingNode();
        _goalNode = node;
        _FSM.ChangeState(LeaderStates.Search);
    }
    void SetStartingNode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5f, nodes);
        float minDistance = 10f;
        
        foreach (var hitCollider in hitColliders)
        {
            Vector3 pos = hitCollider.GetComponent<Transform>().position;
            //Debug.Log(pos +"" + hitCollider);
            if(Vector3.Distance(this.transform.position, pos) < minDistance)
            {
                minDistance = Vector3.Distance(this.transform.position, pos);
                minCollider = hitCollider;
            }
        }
        _startingNode = minCollider.GetComponent<Node>();
    }
    
    public void ChangeColor(Color newColor)
    {
        _myMaterial.color = newColor;
    }

    public void RestoreColor()
    {
        _myMaterial.color = _originalColor;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        var realAngle = _viewAngle / 2;

        Gizmos.color = Color.magenta;
        Vector3 lineLeft = GetDirFromAngle(-realAngle + transform.eulerAngles.y);
        Gizmos.DrawLine(transform.position, transform.position + lineLeft * _viewRadius);

        Vector3 lineRight = GetDirFromAngle(realAngle + transform.eulerAngles.y);
        Gizmos.DrawLine(transform.position, transform.position + lineRight * _viewRadius);
    }

    Vector3 GetDirFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool InFieldOfView(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;

        //Que este dentro de la distancia maxima de vision
        if (dir.sqrMagnitude > _viewRadius * _viewRadius) return false;

        //Que no haya obstaculos
        //if (InLineOfSight(dir)) return false;

        //Que este dentro del angulo
        return Vector3.Angle(transform.forward, dir) <= _viewAngle/2;
        

    }

    /*public bool InLineOfSight(Vector3 direction)
    {
        Debug.DrawLine(transform.position, _player.transform.position, Color.red);
        return Physics.Raycast(transform.position, direction, _viewRadius, obstacleLayer);
    }
    */
}

