using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowersManager : MonoBehaviour
{
    public static FollowersManager Instance { get; private set; }

    public List<Follower> AllFollowers { get; private set; }

    public float ViewRadius
    {
        get
        {
            return _viewRadius * _viewRadius;
        }
    }
    [SerializeField] float _viewRadius;

    [field: SerializeField, Range(0.5f, 4f)]
    public float SeparationWeight { get; private set; }

    [field: SerializeField, Range(0.5f, 4f)]
    public float AlignmentWeight { get; private set; }

    [field: SerializeField, Range(0.5f, 4f)]
    public float CohesionWeight { get; private set; }

    void Awake()
    {
        Instance = this;

        AllFollowers = new List<Follower>();
    }

    public void RegisterNewFollower(Follower newFollower)
    {
        if (!AllFollowers.Contains(newFollower))
            AllFollowers.Add(newFollower);
    }
}

