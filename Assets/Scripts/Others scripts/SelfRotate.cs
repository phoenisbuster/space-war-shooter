using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelfRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetLink(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
