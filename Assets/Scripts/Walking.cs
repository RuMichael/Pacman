using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{

    Vector3 _V3Left = new Vector3(-0.2f, 0, 0);
    Vector3 _V3Right = new Vector3(0.2f, 0, 0);
    Vector3 _V3Up = new Vector3(0, 0.2f, 0);
    Vector3 _V3Down = new Vector3(0, -0.2f, 0);

    Transform _lastPosition;
    WalkingDirection _direction;
    void Start()
    {
        _direction = WalkingDirection.idle;
    }

    void Walk()
    {
        switch (_direction)
        {
            case WalkingDirection.left:
                WalkingLeft();
                break;
            case WalkingDirection.right:
                WalkingRight();
                break;
            case WalkingDirection.up:
                WalkingUp();
                break;
            case WalkingDirection.down:
                WalkingDown();
                break;   
            default:
                return;
        }
    }

    public void WalkingLeft() => transform.localPosition += _V3Left;
    public void WalkingRight() => transform.localPosition += _V3Right;
    public void WalkingUp() => transform.localPosition += _V3Up;
    public void WalkingDown() => transform.localPosition += _V3Down;

    public enum WalkingDirection
    {
        left, right, up, down, idle
    }
}
