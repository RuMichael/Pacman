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
    float changePos;                                //единица шага пакмана (влияет на скороть передвижения)

    Vector3 _lastPosition;                          //нужэно для сохранения позиции(пока не используется)
    int countExistenceChangeDirection;

    WalkingDirection _direction;                    //нужно для сохранения направления движения
    WalkingDirection changeDirection;               //смена направления движения(пока не используется)

    float fall;                                     //переменная для сохранения времени Time.time

    System.Func<Vector3, Vector2> goAhead;          //идти вперед, делегат для сохранения метода движения
    System.Func<Vector3, Vector2> goSide;           //идти назад(пока не используется)

    internal enum WalkingDirection                                            //перечисление для удобства работы с направлениями
    {
        left, right, up, down, idle
    }
    
    void Start()                                    //вызывается на старте игры
    {        
        countExistenceChangeDirection = 0;
        fall = 0;                  
        _direction = WalkingDirection.idle;         //присваеваем на старте стоять на месте
        changeDirection = WalkingDirection.idle;
        _V3Left = new Vector3(-changePos, 0, 0);
        _V3Right = new Vector3(changePos, 0, 0);
        _V3Up = new Vector3(0, changePos, 0);
        _V3Down = new Vector3(0, -changePos, 0);        
    }  
    bool IntegerCheck(Vector3 pos)          // проверка позиции объекта, при значениях близких к целым (~turnBondary) возвращает истину
    {
        float axisX = (int)(Mathf.Abs(pos.x)*100) % 100;
        float axisY = (int)(Mathf.Abs(pos.y)*100) % 100;

        int turnB11 = (int) (changePos*100) / 3;        //5
        int turnB12 = 100 - (int) (changePos*100) + turnB11;  //90
        if((axisX < turnB11 || axisX >= turnB12) && (axisY < turnB11 || axisY >= turnB12))
            return true;
        return false;
    }
    Vector3 AlignPosition(Vector3 pos, WalkingDirection dir)
    {
        return (dir == WalkingDirection.up || dir == WalkingDirection.down) ?
        new Vector3(Mathf.Round(pos.x), pos.y, pos.z) : new Vector3(pos.x, Mathf.Round(pos.y), pos.z);
    }
    internal void DirectionSelection(WalkingDirection direction)
    {
        if (direction == _direction)                            
            return;
        if(_direction == WalkingDirection.idle)
        {
            _direction = direction;
            goAhead = SwitchDirection(_direction);
        }
        else
        {
            if (changeDirection != direction)
            {
                //checkedChangeDirection = false;
                changeDirection = direction;
                goSide = SwitchDirection(changeDirection);
            }
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
    
    [SerializeField]
    BorderBlock _borderBlock;

    void Update()
    {
        if (Time.time - fall >= fallspeed && _direction != WalkingDirection.idle)        
        {         
            Vector3 updatePosition = transform.localPosition;
            Vector3 tempPosition;
            bool checkGoSide = false;
            if (changeDirection != WalkingDirection.idle && IntegerCheck(updatePosition))
            {                
                if (countExistenceChangeDirection <= 1)
                {
                    tempPosition = goSide(AlignPosition(updatePosition, changeDirection));
                    if (_borderBlock.CheckWalk(tempPosition, VectorDirectiod(changeDirection)))
                    {
                        updatePosition = tempPosition;
                        checkGoSide = true;  
                    }
                    countExistenceChangeDirection++;
                }
                else                                    
                    NullifyTurn();                 
            }                               
            if (!checkGoSide)           
            {        
                tempPosition = goAhead(updatePosition);        
                if (_borderBlock.CheckWalk(tempPosition, VectorDirectiod(_direction)))
                    updatePosition = tempPosition;  
                else
                {
                    _direction = WalkingDirection.idle;
                    NullifyTurn(); 
                    updatePosition = new Vector3(Mathf.Round(updatePosition.x), Mathf.Round(updatePosition.y), updatePosition.z);
                }
            }
            else
            {
                _direction = changeDirection;
                goAhead = goSide;
                NullifyTurn(); 
            }

            transform.localPosition = updatePosition;
            fall = Time.time;
        }
    }
    void NullifyTurn()
    {
        changeDirection = WalkingDirection.idle;  
        countExistenceChangeDirection = 0;  
    }

    Vector3 VectorDirectiod(WalkingDirection dir)
    {
        Vector3 tmp;
        if (dir == WalkingDirection.up)
            tmp = Vector3.up;
        else if (dir == WalkingDirection.down)
            tmp = Vector3.down;
        else if (dir == WalkingDirection.left)
            tmp = Vector3.left;
        else if (dir == WalkingDirection.right)
            tmp = Vector3.right;
        else
            tmp = Vector3.zero;
        return tmp;
    }
}
