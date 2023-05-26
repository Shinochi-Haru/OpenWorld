using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // ステートの列挙型
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    private EnemyState currentState; // 現在のステート

    private void Start()
    {
        // 初期状態を設定
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        // 状態に応じた処理を実行
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
        }
    }

    private void UpdateIdleState()
    {
        // 待機状態の処理
        // 例えば、一定時間経過後に追跡状態に遷移するなどの条件を設定
        //if (/* 追跡状態への遷移条件 */)
        //{
        //    currentState = EnemyState.Chase;
        //}
    }

    private void UpdateChaseState()
    {
        // 追跡状態の処理
        // 例えば、プレイヤーを追いかける処理を実行
        // 必要に応じて攻撃状態への遷移条件を設定
        //if (/* 攻撃状態への遷移条件 */)
        //{
        //    currentState = EnemyState.Attack;
        //}
    }

    private void UpdateAttackState()
    {
        // 攻撃状態の処理
        // 例えば、プレイヤーへの攻撃を実行
        // 必要に応じて別のステートへの遷移条件を設定
    }
}
