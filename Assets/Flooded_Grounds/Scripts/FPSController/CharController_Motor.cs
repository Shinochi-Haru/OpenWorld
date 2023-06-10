using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour
{
    public float speed = 10.0f;                     // キャラクターの移動速度
    public float sensitivity = 30.0f;               // マウスの感度
    public float WaterHeight = 15.5f;               // 水面の高さ
    CharacterController character;
    public GameObject cam;                          // カメラオブジェクトへの参照
    float moveFB, moveLR;                            // 前後左右の移動量
    float rotX, rotY;                                // マウスの回転量
    public bool webGLRightClickRotation = true;      // WebGL において右クリックでの回転を有効にするかのフラグ
    float gravity = -9.8f;                           // 重力の値

    void Start()
    {
        character = GetComponent<CharacterController>();     // 自身にアタッチされている CharacterController コンポーネントを取得
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;                // エディタ上では WebGL での右クリック回転を無効化
            sensitivity = sensitivity * 1.5f;               // エディタ上では感度を1.5倍に増やす
        }
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;                                  // キャラクターが水面より下にいる場合は重力を無効化
        }
        else
        {
            gravity = -9.8f;                               // キャラクターが水面より上にいる場合は重力を有効化
        }
    }

    void Update()
    {
        moveFB = Input.GetAxis("Horizontal") * speed;      // 水平方向（左右）の入力に基づいて移動量を計算
        moveLR = Input.GetAxis("Vertical") * speed;        // 垂直方向（前後）の入力に基づいて移動量を計算

        rotX = Input.GetAxis("Mouse X") * sensitivity;     // マウスの X 軸移動量に基づいて横回転量を計算
        rotY = Input.GetAxis("Mouse Y") * sensitivity;     // マウスの Y 軸移動量に基づいて縦回転量を計算

        CheckForWaterHeight();                             // 水面の高さをチェックし、必要に応じて重力を調整

        Vector3 movement = new Vector3(moveFB, gravity, moveLR);   // 移動量をベクトルとして設定

        if (webGLRightClickRotation)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CameraRotation(cam, rotX, rotY);             // WebGL での右クリック回転が有効な場合、マウスの移動量に基づいてカメラを回転
            }
        }
        else if (!webGLRightClickRotation)
        {
            CameraRotation(cam, rotX, rotY);                 // WebGL での右クリック回転が無効な場合、常にマウスの移動量に基づいてカメラを回転
        }

        movement = transform.rotation * movement;            // 移動量をキャラクターのローカル座標系に変換
        character.Move(movement * Time.deltaTime);           // キャラクターを移動させる
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);       // キャラクターを横回転
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);  // カメラを縦回転
    }
}
