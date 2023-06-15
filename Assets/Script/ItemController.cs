using UnityEngine;

public class ItemController : MonoBehaviour
{
    public string itemName; // �A�C�e���̖��O

    private bool isPlayerInRange; // �v���C���[���A�C�e���̋߂��ɂ��邩�ǂ���

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Use();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("");
        }
    }

    public virtual void Use()
    {
        // �A�C�e���̎g�p��������������
        Debug.Log("Item used: " + itemName);
    }
}
