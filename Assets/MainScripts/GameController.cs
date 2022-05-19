using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<ButtonScript> buttonScripts;
    [SerializeField] private int speed = 1;
    [SerializeField] private List<Transform> slaveElement;
    [SerializeField] private Transform leader;
    private float time = 1;
    private MoveState MovementState = MoveState.MoveUp;

    private enum MoveState
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight
    }
    
    void Start()
    {
        for (int i = 0;  i < buttonScripts.Count;  i++)
        {
            var id = i;
            buttonScripts[i].click = () => Direction(id);
        }
    }
    
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime * speed;
        }
        else
        {
            Move();
            time = 1;
        }
    }

    void Direction(int dir)
    {
        var i = (MoveState) dir;
        MovementState = i;
    }

    private Vector3 GetDirection()
    {
        var position = leader.localPosition;
        
        switch (MovementState)
        {
            case MoveState.MoveUp:
                    return new Vector3(position.x, position.y + 100f);
                    
            case MoveState.MoveDown:
                    return new Vector3(position.x, position.y - 100f);
                    
            case MoveState.MoveLeft:
                    return new Vector3(position.x - 100f, position.y);
            
            case MoveState.MoveRight:
                    return new Vector3(position.x + 100f, position.y);
        }
        return new Vector3();
    }
    
    private void Move()
    {
        var position = leader.localPosition;
        leader.localPosition = GetDirection();
    }
}


// for (int i = 0;  i < buttonScripts.Count;  i++)
// {
//     var id = i;
//     buttonScripts[i].click = () => Direction(id);
// }