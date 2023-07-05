using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryLow : MonoBehaviour
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
        lowerbound = 10 - (Boudary.y * CanVasBound.GetComponent<RectTransform>().localScale.y)/2;
        upperbound = 10 + (Boudary.y * CanVasBound.GetComponent<RectTransform>().localScale.y)/2;
        transform.position = new Vector3(0, lowerbound, -5);
    }
    // Start is called before the first frame update
    void Awake()
    {
        CanVasBound = GameObject.FindGameObjectWithTag("Boundary");
        SetBoudary();
        for(int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = new Vector3();
            pos.x = leftbound + i * (rightbound - leftbound)/transform.childCount;
            pos.y = transform.position.y - 2;
            pos.z = -5;
            transform.GetChild(i).position = pos;
        }
    }

    // Update is called once per frame
    void Start()
    {
        if(CanVasBound.GetComponent<RectTransform>().rect.size != Boudary)
        {
            SetBoudary();
        } 
    }
}
