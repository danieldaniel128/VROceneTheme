using System.Collections.Generic;
using UnityEngine;

public class WalkOnPathState : State
{
    Transform _owner;
    private Queue<Vector3> _waypoints;
    float _walkSpeed;

    Vector3 _targetPosition;
    public WalkOnPathState(Transform owner, Queue<Vector3> waypoints,float walkSpeed) 
    {
        _owner = owner;
        _waypoints = waypoints;
        _walkSpeed = walkSpeed;
    }
    public override void EnterState()
    {
        //Debug.Log("<color=green>entering WalkOnPathState state</color>");
        _targetPosition = _waypoints.Dequeue();
    }

    public override void ExecuteUpdateState()
    {
        FollowPath();
    }

    public override void ExitState()
    {
        Debug.Log("exiting WalkOnPathState state");
    }
    private void FollowPath()
    {
        if (_waypoints.Count > 0)
        {
            // Calculate direction to target (ignore Y-axis)
            Vector3 direction = new Vector3(_targetPosition.x - _owner.position.x,0,_targetPosition.z - _owner.position.z).normalized;

            // Rotate towards the target (Y-axis only)
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                _owner.rotation = Quaternion.Slerp(_owner.rotation, targetRotation, _walkSpeed * Time.deltaTime);
            }

            // Move towards the target
            if (Vector3.Distance(_owner.position, _targetPosition) > 0.1f)
            {
                _owner.position = Vector3.MoveTowards(_owner.position, _targetPosition, _walkSpeed * Time.deltaTime);
            }
            else
            {
                // Reached the waypoint, get the next one
                _targetPosition = _waypoints.Dequeue();
                //Debug.Log($"<color=white>move to ({_targetPosition.x},0,{_targetPosition.z}) worldPos</color>");
            }
        }
        else
        {
            // Reached the end
            OnPathComplete(); // Trigger path complete event
        }
    }

    private void OnPathComplete()
    {
        // Handle what happens when the monster reaches the end (e.g., damage the player's base)
        Debug.Log("<color=red>Monster reached the end!</color>");
        _owner.gameObject.SetActive(false); // Destroy the monster
    }
}
