using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    FSM<FollowerStates> _FSM;

    Material _myMaterial;
    [SerializeField]Color _originalColor;

    [SerializeField] public float _maxSpeed, life, maxLife;

    private Collider minCollider;
    [SerializeField] public LayerMask nodes, obstacles;

    [Header("Obstacle Avoidance")]
    public int numberOfRays;
    public float angle = 90;

    [SerializeField] float _maxForce;
    [SerializeField] float distance;
    [SerializeField] float _distanceToDie;
    public Vector3 _velocity;

    [Header("SEEK")]
    SeekSteering _mySeekSteering;
    [Header("ARRIVE")]
    [SerializeField] private bool _isArriving;
    [SerializeField] private float _arriveRadius;
    [SerializeField] private Transform arriveTarget;
    ArriveSteering _myArriveSteering;

    [Header("FLOCKING")]
    Separation _mySeparationSteering;
    Alignment _myAlignmentSteering;
    Cohesion _myCohesionSteering;
    
    [Header("FOV")]
    [SerializeField] private float _viewRadius;
    [SerializeField] private float _viewAngle;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Pathfinding")]
    public Node _startingNode, _goalNode;
    //public bool _startSearch = false;
    //public bool _hasReachNode = false;
    List<Node> _pathToFollow;
    Pathfinding _pathfinding;
    private void Start() 
    {
        FollowersManager.Instance.RegisterNewFollower(this);
        _myArriveSteering = new ArriveSteering(transform, _maxSpeed, _maxForce, _arriveRadius);
        _mySeekSteering = new SeekSteering(transform, _maxSpeed, _maxForce);

        _mySeparationSteering = new Separation(transform, _maxSpeed, _maxForce);
        _myAlignmentSteering = new Alignment(transform, _maxSpeed, _maxForce);
        _myCohesionSteering = new Cohesion(transform, _maxSpeed, _maxForce);

        Vector3 random = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor(Color newColor)
    {
        _myMaterial.color = newColor;
    }

    public void RestoreColor()
    {
        _myMaterial.color = _originalColor;
    }
    void AddForce(Vector3 force)
    {
        _velocity += force;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);
    }
    private void OnDrawGizmos()
    {  
        ///FOV Gizmos
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(transform.position, _viewRadius);

        var realAngle = _viewAngle / 2;

        Gizmos.color = Color.magenta;
        Vector3 lineLeft = GetDirFromAngle(-realAngle + transform.eulerAngles.y);
        Gizmos.DrawLine(transform.position, transform.position + lineLeft * _viewRadius);

        Vector3 lineRight = GetDirFromAngle(realAngle + transform.eulerAngles.y);
        Gizmos.DrawLine(transform.position, transform.position + lineRight * _viewRadius);

        ///
        /// Obstacle Avoidance Gizmos
        for (int i = 0; i < numberOfRays; i++)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / ((float)numberOfRays - 1)) * angle * 2 - angle, this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;
            Gizmos.DrawRay(this.transform.position, direction);
        }
        ///
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
}
