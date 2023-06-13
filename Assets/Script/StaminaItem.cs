using UnityEngine;

public class StaminaItem : ItemController
{
    [SerializeField] int staminaRecoveryAmount = 50; // �X�^�~�i�̉񕜗�

    public override void Use()
    {
        // �X�^�~�i���񕜂��鏈������������
        Debug.Log("StaminaItem used: " + itemName);
        StaminaController staminaController = FindObjectOfType<StaminaController>();
        if (staminaController != null)
        {
            staminaController.RecoverStamina(staminaRecoveryAmount);
        }
    }
}
