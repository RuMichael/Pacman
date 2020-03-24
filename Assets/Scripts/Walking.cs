using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{

    Vector2 _V3Left = new Vector2(-0.2f, 0);
    Vector2 _V3Right = new Vector2(0.2f, 0);
    Vector2 _V3Up = new Vector2(0, 0.2f);
    Vector2 _V3Down = new Vector2(0, -0.2f);

    
    public CharacterController _characterController;
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

    public void WalkingLeft() => _characterController.Move(_V3Left);
    public void WalkingRight() => _characterController.Move(_V3Right);
    public void WalkingUp() => _characterController.Move(_V3Up);
    public void WalkingDown() => _characterController.Move(_V3Down); 

    public enum WalkingDirection
    {
        left, right, up, down, idle
    }
}
