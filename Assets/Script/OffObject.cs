using UnityEngine;

public class OffObject : MonoBehaviour
{
    public GameObject[] objectsToDisable;

    private void Start()
    {
        // ゲームオブジェクトを非アクティブにする
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}
