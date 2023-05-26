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
        // �ҋ@��Ԃ̏���
        // �Ⴆ�΁A��莞�Ԍo�ߌ�ɒǐՏ�ԂɑJ�ڂ���Ȃǂ̏�����ݒ�
        //if (/* �ǐՏ�Ԃւ̑J�ڏ��� */)
        //{
        //    currentState = EnemyState.Chase;
        //}
    }

    private void UpdateChaseState()
    {
        // �ǐՏ�Ԃ̏���
        // �Ⴆ�΁A�v���C���[��ǂ������鏈�������s
        // �K�v�ɉ����čU����Ԃւ̑J�ڏ�����ݒ�
        //if (/* �U����Ԃւ̑J�ڏ��� */)
        //{
        //    currentState = EnemyState.Attack;
        //}
    }

    private void UpdateAttackState()
    {
        // �U����Ԃ̏���
        // �Ⴆ�΁A�v���C���[�ւ̍U�������s
        // �K�v�ɉ����ĕʂ̃X�e�[�g�ւ̑J�ڏ�����ݒ�
    }
}
