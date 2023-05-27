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
    private float idleTimer; // アイドル状態の経過時間
    private float idleDuration = 100f; // アイドル状態の持続時間
    private Vector3 idleDestination; // アイドル時の目的地

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
        // アイドル状態の処理
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            // 追跡状態への遷移条件
            currentState = EnemyState.Chase;
            idleTimer = 0f;
        }
        else
        {
            // ランダムな方向に歩き回る
            if (Random.Range(0f, 1f) < 0.01f || Vector3.Distance(transform.position, idleDestination) < 1f)
            {
                // 10メートル以上移動する目的地を設定
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                idleDestination = transform.position + randomDirection * Random.Range(20f, 40f);
            }
            // 目的地に向かって移動
            transform.position = Vector3.MoveTowards(transform.position, idleDestination, 2f * Time.deltaTime);
        }
    }

    private void UpdateChaseState()
    {
        // 追跡状態の処理
        // 例えば、プレイヤーを追いかける処理を実行
        // 必要に応じて攻撃状態への遷移条件を設定
    }

    private void UpdateAttackState()
    {
        // 攻撃状態の処理
        // 例えば、プレイヤーへの攻撃を実行
        // 必要に応じて別のステートへの遷移条件を設定
    }
}
