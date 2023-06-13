using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIRotate : MonoBehaviour
{
    [SerializeField] public GameObject cam;
    void LateUpdate()
    {
        //　カメラと同じ向きに設定
        transform.rotation = cam.transform.rotation;
    }
}
