using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // �X�e�[�g�̗񋓌^
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    private EnemyState currentState; // ���݂̃X�e�[�g
    private float idleTimer; // �A�C�h����Ԃ̌o�ߎ���
    private float idleDuration = 100f; // �A�C�h����Ԃ̎�������
    private Vector3 idleDestination; // �A�C�h�����̖ړI�n
    [SerializeField] Transform player; // �v���C���[��Transform�R���|�[�l���g
    [SerializeField] float chaseRadius = 20f; // �ǐՔ��a
    float distanceToPlayer = 0f;

    private void Start()
    {
        // ������Ԃ�ݒ�
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        // ��Ԃɉ��������������s
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
        // �v���C���[�ƓG�̋������v�Z
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    private void UpdateIdleState()
    {
        // �A�C�h����Ԃ̏���
        idleTimer += Time.deltaTime;

        if (distanceToPlayer <= chaseRadius)// �v���C���[���ǐՔ��a�ɋ߂Â����ꍇ
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            // �����_���ȕ����ɕ������
            if (Random.Range(0f, 1f) < 0.01f || Vector3.Distance(transform.position, idleDestination) < 1f)
            {
                // 10���[�g���ȏ�ړ�����ړI�n��ݒ�
                Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
                idleDestination = transform.position + randomDirection * Random.Range(20f, 40f);
            }
            // �ړI�n�Ɍ������Ĉړ�
            transform.position = Vector3.MoveTowards(transform.position, idleDestination, 2f * Time.deltaTime);
        }
    }

    private void UpdateChaseState()
    {
        // �v���C���[�̕���������
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // �v���C���[�Ɍ������Ĉړ�����
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 5f);

        if (distanceToPlayer >= chaseRadius)// �v���C���[���ǐՔ��a�͈̔͊O�̏ꍇ
        {
            currentState = EnemyState.Idle;
        }
    }

    private void UpdateAttackState()
    {
        
    }
}
