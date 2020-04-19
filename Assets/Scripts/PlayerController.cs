using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Walking _walking;                               //ссылка на клас управляющий движением пакмана
    Dictionary <Walking.WalkingDirection,KeyCode> _control; //хранилище кнопок управления
    void Start()
    {
        _control = new Dictionary<Walking.WalkingDirection, KeyCode>{       //андрей, это временный код, написал для теста\ примера, это управление ты должен мне 
            {Walking.WalkingDirection.right, KeyCode.D},                    // предоставить из сцены старта игры.
            {Walking.WalkingDirection.left, KeyCode.A},
            {Walking.WalkingDirection.up, KeyCode.W},
            {Walking.WalkingDirection.down, KeyCode.S},
        };
    }
    void Update()       //обработка нажатия клавишь с клавы
    {
        if (Input.GetKeyDown(_control[Walking.WalkingDirection.right]))  //|| Input.GetKey(_control[Walking.WalkingDirection.right]))
            _walking.DirectionSelection(Walking.WalkingDirection.right);
        else if (Input.GetKeyDown(_control[Walking.WalkingDirection.left])) //|| Input.GetKey(_control[Walking.WalkingDirection.left])) 
            _walking.DirectionSelection(Walking.WalkingDirection.left);           
        else if (Input.GetKeyDown(_control[Walking.WalkingDirection.up])) //|| Input.GetKey(_control[Walking.WalkingDirection.up]))
            _walking.DirectionSelection(Walking.WalkingDirection.up);
        else if (Input.GetKeyDown(_control[Walking.WalkingDirection.down])) //|| Input.GetKey(_control[Walking.WalkingDirection.down]))
            _walking.DirectionSelection(Walking.WalkingDirection.down);
    }
}
