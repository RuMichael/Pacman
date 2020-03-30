using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    Vector3 _V3Left;     // векторы для передвижения пакмана в соответствии с направлением
    Vector3 _V3Right;
    Vector3 _V3Up;
    Vector3 _V3Down;
    
    [SerializeField]
    Rigidbody2D _rigidBody2D;                      // ссылка необходимая для управления движением
    [SerializeField]
    float fallspeed;                                // скорость пакмана
    [SerializeField]
    float changePos;                                //единица шага пакмана (влияет на скороть передвижения)

    Vector3 _lastPosition;                          //нужэно для сохранения позиции(пока не используется)

    WalkingDirection _direction;                    //нужно для сохранения направления движения
    WalkingDirection changeDirection;               //смена направления движения(пока не используется)

    float fall;                                     //переменная для сохранения времени Time.time

    System.Func<Vector3, Vector2> goAhead;          //идти вперед, делегат для сохранения метода движения
    System.Func<Vector3, Vector2> goBack;           //идти назад(пока не используется)

    void Start()                                    //вызывается на старте игры
    {
        fall = 0;                   
        _direction = WalkingDirection.idle;         //присваеваем на старте стоять на месте
        _V3Left = new Vector3(-changePos, 0, 0);
        _V3Right = new Vector3(changePos, 0, 0);
        _V3Up = new Vector3(0, changePos, 0);
        _V3Down = new Vector3(0, -changePos, 0);        
    }   

    void Update()
    {
        if (Time.time - fall >= fallspeed && _direction != WalkingDirection.idle)       //если есть направление движения и промежуток времени fallspeed прошел, 
        {                                                                               //то перемещаем пакмана вперед
            _rigidBody2D.MovePosition(goAhead(transform.localPosition));                //мув позишн принимает место на которое нужно переместить пакмана,goAhaed принимает текущее положение пакмана и возвращает его следующее положение
            fall = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D other)                                 //событие возникающее при собрикосновении boxcollider и пакмана (стены)
    {
        Debug.Log("OnTriggerEnter2D");                                      // сообщение в консоль(для облегчения тестирования)
        _direction= WalkingDirection.idle;                                  // направление изменяем на значение "стоять на месте"
        transform.localPosition = RoundPosition(transform.localPosition);   // округляем значение позиции пакмана до целых чисел(возвращаем 
    }                                                                       //в точку из которой пакман сможет дальше ходить)

    public enum WalkingDirection                                            //перечисление для удобства работы с направлениями
    {
        left, right, up, down, idle
    }

    public void Move(WalkingDirection direction)                            //метод где мы устанавливаем направление движения пакмана
    {
        if (direction == _direction)                            //проверяем изменилось ли направление движения, если нет то заканчиваем работу метода
            return;
        else
            _direction = direction;
        switch (_direction)                                     
        {                                                       // в соответствии с направлением движения присваиваем делегату метод направления
            case WalkingDirection.left:
                goAhead = WalkLeft; 
                goBack = WalkRight;                             //goBAck не используется
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

    Vector2 WalkLeft(Vector3 position) => (Vector2)(position + _V3Left);            //изменение положения на единицу Vector2 (_V3Left,Right,Up,Down)
    Vector2 WalkRight(Vector3 position) => (Vector2)(position + _V3Right);          // это методы которые мы присваиваем goAhead (идти вперед)
    Vector2 WalkUp(Vector3 position)=> (Vector2)(position + _V3Up);
    Vector2 WalkDown(Vector3 position) => (Vector2)(position + _V3Down);
    Vector3 RoundPosition(Vector3 pos) => new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);     //метод принимает значение позиции и возращает его с округленными до целых
}
