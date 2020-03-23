using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Walking _walking;
    public Dictionary <Walking.WalkingDirection,KeyCode> _control;
    void Start()
    {
        _control = new Dictionary<Walking.WalkingDirection, KeyCode>{       //андрей, это временный код, написал для теста\ примера, это управление ты должен мне 
            {Walking.WalkingDirection.right, KeyCode.D},                    // предоставить из сцены старта игры.
            {Walking.WalkingDirection.left, KeyCode.A},
            {Walking.WalkingDirection.up, KeyCode.W},
            {Walking.WalkingDirection.down, KeyCode.S},
        };
    }


    void Update()
    {
        if (Input.GetKeyDown(_control[Walking.WalkingDirection.right]))
            _walking.WalkingRight();
        else if (Input.GetKeyDown(_control[Walking.WalkingDirection.left]))
            _walking.WalkingLeft();
        else if (Input.GetKeyDown(_control[Walking.WalkingDirection.up]))
            _walking.WalkingUp();
        else if (Input.GetKeyDown(_control[Walking.WalkingDirection.down]))
            _walking.WalkingDown();
    }
}
