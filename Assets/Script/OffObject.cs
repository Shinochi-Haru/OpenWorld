using UnityEngine;

public class OffObject : MonoBehaviour
{
    public GameObject[] objectsToDisable;

    private void Start()
    {
        // �Q�[���I�u�W�F�N�g���A�N�e�B�u�ɂ���
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}
