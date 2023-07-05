using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipManager : MonoBehaviour
{
    public List<GameObject> SpaceShipType;
    public GameObject CurSpaceShipType;
    public int index = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        SetSpaceShip(0);
    }

    public void SetSpaceShip(int i)
    {
        if(i != index)
        {
            index = i;
            CurSpaceShipType = Instantiate(SpaceShipType[i], transform.position, transform.rotation);
            if(transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }       
            CurSpaceShipType.transform.SetParent(transform);
            GetComponent<PlayerStats>().isLv10 = false;
            GetComponent<PlayerStats>().CurLv10Time = 0;
            GetComponent<PlayerStats>().SetLevel(11, CurSpaceShipType);
        }
        else
        {
            GetComponent<PlayerStats>().SetLevel();
        }
        //transform.GetChild(0).localPosition = Vector3.zero;
        //transform.GetChild(0).localEulerAngles = Vector3.zero;
    }

    public void SetSpaceShipDebug()
    {
        Debug.Log("Length Space Ship: " + SpaceShipType.Count);
        Debug.Log("Index is: " + index);
        int i = index;
        i = i < SpaceShipType.Count-1? i+1 : 0;
        SetSpaceShip(i); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetSpaceShipDebug();
        }
    }
}
