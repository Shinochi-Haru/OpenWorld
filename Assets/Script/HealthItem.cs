using System.Collections;
using UnityEngine;

public class HealthItem : ItemController
{
    [SerializeField] int healthAmount = 50; // 回復するHP量

    public override void Use()
    {
        // アイテム使用時の処理
        Debug.Log("Health item used: " + itemName);
        // プレイヤーのHPを回復する
        var player = FindObjectOfType<HpController>();
        if (player != null)
        {
            player.RecoverHealth(healthAmount);
        }
    }
}
