using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    private Button button;
    public Action click { get; set; }
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => click.Invoke());
    }
}
