using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryLeft : MonoBehaviour
{
    public GameObject CanVasBound;
    public Vector2 Boudary;
    public float lowerbound = -2.5f;
    public float upperbound = 22f;
    public float leftbound = -23f;
    public float rightbound = 23f;
    // Start is called before the first frame update
    public void SetBoudary()
    {
        Boudary = CanVasBound.GetComponent<RectTransform>().rect.size;
        leftbound = -(Boudary.x * CanVasBound.GetComponent<RectTransform>().localScale.x)/2 - 1;
        rightbound = (Boudary.x * CanVasBound.GetComponent<RectTransform>().localScale.x)/2 + 1;
        lowerbound = 10 - (Boudary.y * CanVasBound.GetComponent<RectTransform>().localScale.y)/2 + 1;
        upperbound = 10 + (Boudary.y * CanVasBound.GetComponent<RectTransform>().localScale.y)/2 - 1;
        transform.position = new Vector3(leftbound, 0, -5);
    }

    void Awake()
    {
        CanVasBound = GameObject.FindGameObjectWithTag("Boundary");
        SetBoudary();
        for(int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = new Vector3();
            pos.x = transform.position.x - 2;
            pos.y = upperbound - i * (upperbound - lowerbound)/transform.childCount;
            pos.z = -5;
            transform.GetChild(i).position = pos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(CanVasBound.GetComponent<RectTransform>().rect.size != Boudary)
        {
            SetBoudary();
        } 
    }
}
