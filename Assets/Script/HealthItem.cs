using System.Collections;
using UnityEngine;

public class HealthItem : ItemController
{
    [SerializeField] int healthAmount = 50; // �񕜂���HP��

    public override void Use()
    {
        // �A�C�e���g�p���̏���
        Debug.Log("Health item used: " + itemName);
        // �v���C���[��HP���񕜂���
        var player = FindObjectOfType<HpController>();
        if (player != null)
        {
            player.RecoverHealth(healthAmount);
        }
    }
}
