using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private float time = 1;

    private int speed = 1;

    public Transform aaa;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    private void Move()
    {
        var r = aaa.localPosition;
        aaa.localPosition = new Vector3(0, r.y - 100f);
    }
}
