using UnityEngine;

public class StaminaItem : ItemController
{
    [SerializeField] int staminaRecoveryAmount = 50; // スタミナの回復量

    public override void Use()
    {
        // スタミナを回復する処理を実装する
        Debug.Log("StaminaItem used: " + itemName);
        StaminaController staminaController = FindObjectOfType<StaminaController>();
        if (staminaController != null)
        {
            staminaController.RecoverStamina(staminaRecoveryAmount);
        }
    }
}
