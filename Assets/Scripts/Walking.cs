using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    Vector3 _V3Left = new Vector3(-0.2f, 0, 0);
    Vector3 _V3Right = new Vector3(0.2f, 0, 0);
    Vector3 _V3Up = new Vector3(0, 0.2f, 0);
    Vector3 _V3Down = new Vector3(0, -0.2f, 0);

    public Rigidbody2D _rigidBody2D;
    Vector3 _lastPosition;
    WalkingDirection _direction;
    WalkingDirection changeDirection;

    float fall, fallspeed;

    System.Func<Vector3, Vector2> goAhead;
    System.Func<Vector3, Vector2> goBack;

    void Start()
    {
        fall = 0;
        fallspeed = 0.9f;
        _direction = WalkingDirection.idle;
    }   

    void Update()
    {
        if (Time.time - fall >= fallspeed && _direction != WalkingDirection.idle)        
        {
            _rigidBody2D.MovePosition(goAhead(transform.localPosition));
        }
    }

    
    /*public void Walk(WalkingDirection direction)
    {
        _direction = direction;
        switch (direction)
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

    void WalkingLeft() => _rigidBody2D.MovePosition((Vector2) (transform.localPosition + _V3Left));
    void WalkingRight() => _rigidBody2D.MovePosition((Vector2) (transform.localPosition + _V3Right));
    void WalkingUp()=> _rigidBody2D.MovePosition((Vector2) (transform.localPosition + _V3Up));
    void WalkingDown() => _rigidBody2D.MovePosition((Vector2) (transform.localPosition + _V3Down));*/
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        _direction= WalkingDirection.idle;
        _rigidBody2D.MovePosition(goBack(transform.localPosition));
        /* if (_direction == WalkingDirection.right)
            _rigidBody2D.MovePosition((Vector2) (transform.localPosition - _V3Right));
        else if (_direction == WalkingDirection.left)
            _rigidBody2D.MovePosition((Vector2) (transform.localPosition - _V3Left));
        else if (_direction == WalkingDirection.up)
            _rigidBody2D.MovePosition((Vector2) (transform.localPosition - _V3Up));
        else if (_direction == WalkingDirection.down)
            _rigidBody2D.MovePosition((Vector2) (transform.localPosition - _V3Down));
        _direction = WalkingDirection.idle;*/
    }

    public enum WalkingDirection
    {
        left, right, up, down, idle
    }

    public void Move(WalkingDirection direction)
    {
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

}
