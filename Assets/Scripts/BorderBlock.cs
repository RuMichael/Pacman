using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderBlock : MonoBehaviour
{
    List<Coordinates> listOfCoordinates;
    [SerializeField]
    List<GameObject> linkBorders;
    struct Coordinates
    {
        int x,y;
        internal float X
        {
            get{
                return x;
            }
            set{
               x = (int) value;     
            }
        }
        internal float Y
        {
            get{
                return y;
            }
            set{
               y = (int) value;     
            }
        }
    }
    void Start()
    {
        listOfCoordinates = new List<Coordinates>();
        Coordinates startPos;
        foreach (GameObject gameObj in linkBorders)
        {
            startPos = new Coordinates{ X = gameObj.transform.localPosition.x, Y = gameObj.transform.localPosition.y};
            Filling(gameObj.GetComponentsInChildren<Transform>(), startPos, gameObj.transform);                                   
        }
    }    
    void Filling(Transform[] roughCopy, Coordinates startPos, Transform gameObj)
    {
        foreach (Transform trans in roughCopy)
        {
            int rotation = ((int) gameObj.localEulerAngles.z) % 360;
            Coordinates tmpPos = new Coordinates();
            if (rotation == 0)
                tmpPos = new Coordinates{X = startPos.X + trans.localPosition.x , Y = startPos.Y + trans.localPosition.y};
            else if (rotation == 90 || rotation == -270)                
                tmpPos = new Coordinates{X = startPos.X - trans.localPosition.y , Y = startPos.Y + trans.localPosition.x};
            else if (rotation == -90 || rotation == 270)
                tmpPos = new Coordinates{X = startPos.X + trans.localPosition.y , Y = startPos.Y - trans.localPosition.x};
            else if (rotation == -180 || rotation == 180) 
                tmpPos = new Coordinates{X = startPos.X - trans.localPosition.x , Y = startPos.Y - trans.localPosition.y};
            if (gameObj != trans)
                listOfCoordinates.Add(tmpPos);
        }
    }
    internal bool CheckWalk(Vector3 position, Vector3 direction)           //тут ошибка с расчетом int x y
    {
        Debug.Log(direction.ToString());
        bool rezult = true;
        int x = (int) position.x;
        int y = (int) position.y;

        if (Mathf.Abs(direction.x) > 0)        
            x = (direction.x > 0 && position.x > 0 || direction.x < 0 && position.x < 0) ? (int) (position.x + direction.x) : (int) position.x;     
        else if (Mathf.Abs(direction.y) > 0)        
            y = (direction.y > 0 && position.y > 0 || direction.y < 0 && position.y < 0) ? (int) (position.y + direction.y) : (int) position.y;      
        Coordinates tmpPos = new Coordinates{X = x, Y =  y};
        foreach (var item in listOfCoordinates)
        {
            if (item.X == tmpPos.X && item.Y == tmpPos.Y)
                rezult = false;
        }
        Debug.Log("Проверяется позиция: " + x + " : " + y +". вектор3: " + position.x + " : " + position.y+ ". результат: "+ rezult.ToString());
        return rezult;
    }
}
