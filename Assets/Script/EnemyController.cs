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
    }

    private void UpdateIdleState()
    {
        // �A�C�h����Ԃ̏���
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            // �ǐՏ�Ԃւ̑J�ڏ���
            currentState = EnemyState.Chase;
            idleTimer = 0f;
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
        // �ǐՏ�Ԃ̏���
        // �Ⴆ�΁A�v���C���[��ǂ������鏈�������s
        // �K�v�ɉ����čU����Ԃւ̑J�ڏ�����ݒ�
    }

    private void UpdateAttackState()
    {
        // �U����Ԃ̏���
        // �Ⴆ�΁A�v���C���[�ւ̍U�������s
        // �K�v�ɉ����ĕʂ̃X�e�[�g�ւ̑J�ڏ�����ݒ�
    }
}
