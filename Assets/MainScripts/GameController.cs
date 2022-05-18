using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<ButtonScript> buttonScripts;
    [SerializeField] private int speed = 1;
    private float time = 1;
    public Transform positionObject;
    int dir = 0;
    
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
            Move(dir);
            time = 1;
        }
    }

    void Direction(int dir)
    {
        this.dir = dir;
    }
    
    
    private void Move(int direction)
    {
        var position = positionObject.localPosition;

        switch (direction)
        {
            case 0:
                positionObject.localPosition = new Vector3(position.x, position.y + 100f);
                break;
            case 1:
                positionObject.localPosition = new Vector3(position.x, position.y - 100f); 
                break;
            case 2:
                positionObject.localPosition = new Vector3(position.x - 100f, position.y);
                break;
            case 3:
                positionObject.localPosition = new Vector3(position.x + 100f, position.y);
                break;
        }
        
    }
}
