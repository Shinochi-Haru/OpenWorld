using UnityEngine;

public class ItemController : MonoBehaviour
{
    public string itemName; // アイテムの名前

    private bool isPlayerInRange; // プレイヤーがアイテムの近くにいるかどうか

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
        // アイテムの使用処理を実装する
        Debug.Log("Item used: " + itemName);
    }
}
