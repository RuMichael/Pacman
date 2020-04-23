using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    const int turnBoundary = 10;
    Vector3 _V3Left;     // векторы для передвижения пакмана в соответствии с направлением
    Vector3 _V3Right;
    Vector3 _V3Up;
    Vector3 _V3Down;
    
    [SerializeField]
    Rigidbody2D _rigidBody2D;                      // ссылка необходимая для управления движением
    [SerializeField]
    float fallspeed;                                // скорость пакмана
    [SerializeField]
    float fallspeedcheck;
    [SerializeField]
    float changePos;                                //единица шага пакмана (влияет на скороть передвижения)

    Vector3 _lastPosition;                          //нужэно для сохранения позиции(пока не используется)
    bool checkedChangeDirection;
    int countExistenceChangeDirection;

    WalkingDirection _direction;                    //нужно для сохранения направления движения
    WalkingDirection changeDirection;               //смена направления движения(пока не используется)

    float fall, fallcheck;                                     //переменная для сохранения времени Time.time

    System.Func<Vector3, Vector2> goAhead;          //идти вперед, делегат для сохранения метода движения
    System.Func<Vector3, Vector2> goSide;           //идти назад(пока не используется)

    internal enum WalkingDirection                                            //перечисление для удобства работы с направлениями
    {
        left, right, up, down, idle
    }
    
    void Start()                                    //вызывается на старте игры
    {        
        countExistenceChangeDirection = 0;
        checkedChangeDirection = false;
        fall = 0; fallcheck = 0;                  
        _direction = WalkingDirection.idle;         //присваеваем на старте стоять на месте
        changeDirection = WalkingDirection.idle;
        _V3Left = new Vector3(-changePos, 0, 0);
        _V3Right = new Vector3(changePos, 0, 0);
        _V3Up = new Vector3(0, changePos, 0);
        _V3Down = new Vector3(0, -changePos, 0);        
    }   
    void FixedUpdate()
    {           
        if (Time.time - fall >= fallspeed && _direction != WalkingDirection.idle)        
        {         
            _lastPosition = transform.localPosition;
            if (changeDirection != WalkingDirection.idle && IntegerCheck(_lastPosition))
            {                
                if (countExistenceChangeDirection <= 1)
                {                    
                    Debug.Log("shag vstorony "+countExistenceChangeDirection.ToString());                    
                    checkedChangeDirection = true;                   
                    _rigidBody2D.MovePosition(goSide(AlignPosition(changeDirection)));
                    countExistenceChangeDirection++;                    
                }
                else
                {
                    checkedChangeDirection = false;
                    countExistenceChangeDirection = 0;
                    changeDirection = WalkingDirection.idle;
                }
            }                               
            else             
            {
                _rigidBody2D.MovePosition(goAhead(transform.localPosition));   
                //Debug.Log("Shag");    
            }     
            fall = Time.time;
        }
    }
    void Update() // нужно попробовать обойтись без этого
    {
        if (checkedChangeDirection /* && IntegerCheck(transform.localPosition)*/ && changeDirection == CheckedWalk() && Time.time - fallcheck >= fallspeedcheck)   //должен срабатывать только в случае успешного поворота
        {               
            //Debug.Log("true");                        //при смене позиции добавить выравнивание по осям x y, в зависимости от направления движения
            RefreshDirection();
            fallcheck = Time.time;
        }     
    }    
    void RefreshDirection()
    {
        transform.localPosition = AlignPosition(changeDirection);
        _direction = changeDirection;
        goAhead = goSide;
        changeDirection = WalkingDirection.idle;
        checkedChangeDirection = false;             //
        countExistenceChangeDirection = 0;        
    }    
    Vector3 AlignPosition(WalkingDirection dir)
    {
        Vector3 pos = transform.localPosition;
        return (dir == WalkingDirection.up || dir == WalkingDirection.down) ?
        new Vector3(Mathf.Round(pos.x), pos.y, pos.z) : new Vector3(pos.x, Mathf.Round(pos.y), pos.z);
    }
    void OnTriggerEnter2D(Collider2D other)                                 //событие возникающее при собрикосновении boxcollider и пакмана (стены)
    {
        Debug.Log("Triger " + other.name);
        if ( other.name.IndexOf("Point") >= 0 )
            return;        
        if (checkedChangeDirection)
        {
            transform.localPosition = _lastPosition;
            checkedChangeDirection = false;
            //_rigidBody2D.MovePosition(goAhead(transform.localPosition));
            transform.localPosition = goAhead(_lastPosition);
        }
        else
        {
            changeDirection = WalkingDirection.idle;
            _direction = WalkingDirection.idle;                                  // направление изменяем на значение "стоять на месте"
            transform.localPosition = RoundPosition(_lastPosition);   
        }
    }        
    WalkingDirection CheckedWalk()                      // возвращает фактическое направление движения
    {        
        Vector3 tmp = transform.localPosition - _lastPosition;
        //Debug.Log("CheckWalk tmp= " + tmp.x.ToString() + "; " + tmp.y.ToString());
        if (Mathf.Abs(tmp.x) > Mathf.Abs(tmp.y))
        {
            if (tmp.x > 0)        
                return WalkingDirection.right; 
            else if (tmp.x < 0)        
                return WalkingDirection.left; 
        }
        else 
        {
            if (tmp.y > 0)
                return WalkingDirection.up;
            else if (tmp.y < 0)
                return WalkingDirection.down;
        }
        return WalkingDirection.idle;
    }    
    bool IntegerCheck(Vector3 pos)          // проверка позиции объекта, при значениях близких к целым (~turnBondary) возвращает истину
    {
        float axisX = (int)(Mathf.Abs(pos.x)*100) % 100;
        float axisY = (int)(Mathf.Abs(pos.y)*100) % 100;
        //Debug.Log("tmpINT Position X = " + tmpInt.X + ", Y = " + tmpInt.Y);
        int turnB1 = turnBoundary;             
        int turnB2 = 100 - turnBoundary;    
        if((axisX < turnB1 || axisX > turnB2) && (axisY < turnB1 || axisY > turnB2))
            return true;
        //Debug.Log("IntegerCheck = False");
        return false;
    }
    internal void DirectionSelection(WalkingDirection direction)
    {
        //Debug.Log("DirectionSelection");
        if (direction == _direction)                            
            return;
        if(_direction == WalkingDirection.idle)
        {
            _direction = direction;
            goAhead = SwitchDirection(_direction);
        }
        else
        {
            //Debug.Log("change directoin");
            if (changeDirection != direction)
            {
                checkedChangeDirection = false;
                changeDirection = direction;
                goSide = SwitchDirection(changeDirection);
            }
            //Debug.Log(goSide.Target.ToString());
            countExistenceChangeDirection = 0;            
        }
    }
    System.Func<Vector3, Vector2> SwitchDirection(WalkingDirection direction)
    {
        System.Func<Vector3, Vector2> value;
        switch (direction)                                     
        {                                                       
            case WalkingDirection.left:
                value = WalkLeft;                           
                break;
            case WalkingDirection.right:
                value = WalkRight; 
                break;
            case WalkingDirection.up:
                value = WalkUp; 
                break;
            case WalkingDirection.down:
                value = WalkDown; 
                break;   
            default:
                return null;
        }
        return value;
    }    
    Vector2 WalkLeft(Vector3 position) => (Vector2)(position + _V3Left);            //изменение положения на единицу Vector2 (_V3Left,Right,Up,Down)
    Vector2 WalkRight(Vector3 position) => (Vector2)(position + _V3Right);          // это методы которые мы присваиваем goAhead (идти вперед)
    Vector2 WalkUp(Vector3 position)=> (Vector2)(position + _V3Up);
    Vector2 WalkDown(Vector3 position) => (Vector2)(position + _V3Down);
    Vector3 RoundPosition(Vector3 pos) => new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);     //метод принимает значение позиции и возращает его с округленными до целых
    
}
