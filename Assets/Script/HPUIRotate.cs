using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIRotate : MonoBehaviour
{
    [SerializeField] public GameObject cam;
    void LateUpdate()
    {
        //�@�J�����Ɠ��������ɐݒ�
        transform.rotation = cam.transform.rotation;
    }
}
