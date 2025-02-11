using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private Queue<Vector3> _waypoints; // The path in world positions
    private float _speed;
    StateMachine _monsterStateMachine;
    public void Init(List<Vector2Int> path, float speed, System.Func<Vector2Int, Vector3> gridToWorldPosition)
    {
        _speed = speed;
        _waypoints = new Queue<Vector3>();
        _monsterStateMachine = new StateMachine();
        // Convert path from grid positions to world positions
        foreach (Vector2Int point in path)
        {
            _waypoints.Enqueue(gridToWorldPosition(point));
        }
        IState walkOnPathState = new WalkOnPathState(transform, _waypoints,speed);
        _monsterStateMachine.ChangeState(walkOnPathState);

        // Start moving along the path
        //StartCoroutine(FollowPath());
    }
    private void Update()
    {
        _monsterStateMachine.Update();
    }

}
