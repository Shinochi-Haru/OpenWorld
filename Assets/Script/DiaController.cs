using UnityEngine.UI;

public class DiaController : ItemController
{
    // �J�E���g��\������UI�̃e�L�X�g�I�u�W�F�N�g
    public Text countText;

    // �A�C�e���̃J�E���g
    private int itemCount = 0;

    // �I�[�o�[���C�h����Use���\�b�h
    public override void Use()
    {
        // �e�N���X��Use���\�b�h���Ăяo��
        base.Use();

        // �J�E���g�𑝂₷
        itemCount++;

        // �J�E���g���X�V���ĕ\������
        countText.text = itemCount.ToString();
    }
}
