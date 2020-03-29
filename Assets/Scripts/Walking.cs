using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    Vector3 _V3Left = new Vector3(-0.2f, 0, 0);
    Vector3 _V3Right = new Vector3(0.2f, 0, 0);
    Vector3 _V3Up = new Vector3(0, 0.2f, 0);
    Vector3 _V3Down = new Vector3(0, -0.2f, 0);
    
    [SerializeField]
    Rigidbody2D _rigidBody2D;
    [SerializeField]
    float fallspeed;

    Vector3 _lastPosition;

    WalkingDirection _direction;
    WalkingDirection changeDirection;

    float fall;

    System.Func<Vector3, Vector2> goAhead;
    System.Func<Vector3, Vector2> goBack;

    void Start()
    {
        fall = 0;
        _direction = WalkingDirection.idle;
        Debug.Log(_rigidBody2D.transform);
    }   

    void Update()
    {
        if (Time.time - fall >= fallspeed && _direction != WalkingDirection.idle)        
        {
            _rigidBody2D.MovePosition(goAhead(transform.localPosition));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        _direction= WalkingDirection.idle;    
        transform.localPosition = RoundPosition(transform.localPosition);
    }

    public enum WalkingDirection
    {
        left, right, up, down, idle
    }

    public void Move(WalkingDirection direction)
    {
        if (direction == _direction)
            return;
        else
            _direction = direction;
        switch (direction)
        {
            case WalkingDirection.left:
                goAhead = WalkLeft; 
                goBack = WalkRight;
                break;
            case WalkingDirection.right:
                goAhead = WalkRight; 
                goBack = WalkLeft;
                break;
            case WalkingDirection.up:
                goAhead = WalkUp; 
                goBack = WalkDown;
                break;
            case WalkingDirection.down:
                goAhead = WalkDown; 
                goBack = WalkUp;
                break;   
            default:
                return;
        }
    }

    Vector2 WalkLeft(Vector3 position) => (Vector2)(position + _V3Left);
    Vector2 WalkRight(Vector3 position) => (Vector2)(position + _V3Right);
    Vector2 WalkUp(Vector3 position)=> (Vector2)(position + _V3Up);
    Vector2 WalkDown(Vector3 position) => (Vector2)(position + _V3Down);
    Vector3 RoundPosition(Vector3 pos) => new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);
}
