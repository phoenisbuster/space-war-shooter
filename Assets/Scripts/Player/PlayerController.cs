using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0;
    public float testSpeed = 0.001f;
    public float norSpeed = 20;
    public float slowSpeed = 10;
    public float buffSpeed = 30;
    public bool Debug = false;

    public GameObject CanVasBound;
    public Vector2 Boudary;
    public float lowerbound = -2.5f;
    public float upperbound = 22f;
    public float leftbound = -23f;
    public float rightbound = 23f;

    public float turnSmoothing = 0.1f;
    float smoothX = 0;
    float smoothY = 0;
    float smoothXvelocity;
	float smoothYvelocity;
    public bool isDevice_Touch = false;
    /*
        Test delay time when user not touch the screen
    */
    int NoTouch = 0;
    public static Action firstTouch;
    public bool allowPlayerControl = true; 
    
    public void SetBoudary()
    {
        Boudary = CanVasBound.GetComponent<RectTransform>().rect.size;
        leftbound = -(Boudary.x * CanVasBound.GetComponent<RectTransform>().localScale.x)/2 + 1;
        rightbound = (Boudary.x * CanVasBound.GetComponent<RectTransform>().localScale.x)/2 - 1;
        lowerbound = 10 - (Boudary.y * CanVasBound.GetComponent<RectTransform>().localScale.y)/2 + 1;
        upperbound = 10 + (Boudary.y * CanVasBound.GetComponent<RectTransform>().localScale.y)/2 - 1;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = norSpeed;
        if(!Debug)
            Cursor.lockState = CursorLockMode.Locked;
        SetBoudary();

        #if UNITY_ANDROID || UNITY_IOS
            isDevice_Touch = true;
        #endif

        #if UNITY_STANDALONE || UNITY_WEBGL || UNITY_EDITOR
            isDevice_Touch = false;
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if(CanVasBound.GetComponent<RectTransform>().rect.size != Boudary)
        {
            SetBoudary();
        }
        if(allowPlayerControl)
        {
            if(Input.GetMouseButtonDown(0) && !isDevice_Touch && Debug)
            {
                NoTouch++;
                if(NoTouch==1)
                {
                    firstTouch?.Invoke();
                }
            }   
            InputProcessing();
        }
        PlayerMovement();
    }

    void InputProcessing()
    {
        float h = 0;
        float v = 0;

        if(isDevice_Touch)
        {
            turnSmoothing = 0.01f;
            if(Input.touchCount > 0)
            {
                Time.timeScale = 1;
                Touch touch = Input.GetTouch(0);
                h = touch.deltaPosition.x;
                v = touch.deltaPosition.y;
                NoTouch++;
                if(NoTouch==1)
                {
                    firstTouch?.Invoke();
                }
            }
            else
            {
                Time.timeScale = 0.05f;
            }
        }
        else
        {
            Time.timeScale = 1;
            h = Input.GetAxisRaw("Mouse X");
		    v = Input.GetAxisRaw("Mouse Y");
            turnSmoothing = 0.01f;
        }

        if (turnSmoothing > 0)
		{
			smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing); //Gradually change a value toward a desired goal over time.
			smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
		}
		else
		{
			smoothX = h;
			smoothY = v;
		}
    }
    void PlayerMovement()
    {
        Vector3 direction = new Vector3();
        if(isDevice_Touch)
        {
            direction = new Vector3(smoothX, smoothY, 0);
        }
        else
        {
            direction = new Vector3(smoothX, smoothY, 0).normalized;
        }
        
        if(transform.position.x < leftbound && direction.x < 0)
        {
            direction.x = 0;                           
        }
        if(transform.position.x > rightbound && direction.x > 0)
        {
            direction.x = 0;                          
        }
        if(transform.position.y < lowerbound + 1 && direction.y < 0)
        {
            direction.y = 0;
        }
        if(transform.position.y > upperbound - 2 && direction.y > 0)
        {
            direction.y = 0;
        }
        if(!GetComponent<HealthManager>().isDead)
        {
            if(isDevice_Touch)
            {
                transform.Translate(direction * testSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
