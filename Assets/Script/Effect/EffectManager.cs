using System.Collections.Generic;
using UnityEngine;

/// <summary>�e��G�t�F�N�g���Ǘ�����N���X</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>�a���G�t�F�N�g�̃��X�g</summary>
    [SerializeField, Header("�a���G�t�F�N�g")] private List<GameObject> _slashEffectList = new List<GameObject>();

    /// <summary>�a���G�t�F�N�g�̑����ƃC���f�b�N�X�̃}�b�v</summary>
    private Dictionary<AttackEffectType, int> _slashEffectIndexMap;

    /// <summary>�K�E�Z�G�t�F�N�g�̃��X�g</summary>
    [SerializeField, Header("�K�E�Z�G�t�F�N�g")] private List<GameObject> _ultEffectList = new List<GameObject>();

    /// <summary>�K�E�Z�G�t�F�N�g�̏��ƃC���f�b�N�X�̃}�b�v</summary>
    private Dictionary<AttackEffectType, int> _ultEffectIndexMap;

    /// <summary>����U���G�t�F�N�g�̃��X�g</summary>
    [SerializeField, Header("����U���̃G�t�F�N�g")] private List<GameObject> _altEffectList = new List<GameObject>();

    private Dictionary<AttackEffectType, int> _altEffectIndexMap;

    /// <summary>�a���G�t�F�N�g�̉�]�ʂ������萔�BX�������̉�]�p�x�i0�x�j�B</summary>
    private const float SLASH_EFFECT_X_ANGLE = 0f;

    /// <summary>�a���G�t�F�N�g�̉�]�ʂ������萔�BY�������̉�]�p�x�i180�x�j�B</summary>
    private const float SLASH_EFFECT_Y_ANGLE = 180f;

    /// <summary>�E�����a���G�t�F�N�g�̉�]�ʂ������萔�BZ�������̉�]�p�x�i180�x�j�B</summary>
    private const float SLASH_EFFECT_Z_ANGLE_RIGHT = 180f;

    /// <summary>�E�΂ߎa���G�t�F�N�g�̉�]�ʂ������萔�BZ�������̉�]�p�x�i-135�x�j�B</summary>
    private const float SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL = -135f;

    /// <summary>�������a���G�t�F�N�g�̉�]�ʂ������萔�BZ�������̉�]�p�x�i0�x�j�B</summary>
    private const float SLASH_EFFECT_Z_ANGLE_LEFT = 0f;

    /// <summary>���΂ߎa���G�t�F�N�g�̉�]�ʂ������萔�BZ�������̉�]�p�x�i-45�x�j�B</summary>
    private const float SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL = -45f;

    /// <summary>����U���G�t�F�N�g���~������܂ł̒x������</summary>
    private const float ALT_ATTACK_EFFECT_STOP_DELAY = 0f;

    protected override void Awake()
    {
        // �V���O���g���̐ݒ�
        base.Awake();

        // �a���G�t�F�N�g�̃}�b�v�̏�����
        _slashEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Ink, 0},
            {AttackEffectType.RedFlame, 1},
            {AttackEffectType.BlueFlame, 2},
            {AttackEffectType.Nebula, 3},
            {AttackEffectType.Blood, 4},
            {AttackEffectType.Water, 5}
        };

        // �K�E�Z�G�t�F�N�g�̃}�b�v�̏�����
        _ultEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Wind , 0},
            {AttackEffectType.Lightning, 1},
            {AttackEffectType.White, 2}
        };

        // ����U���G�t�F�N�g�̃}�b�v�̏�����
        _altEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Ink, 0},
            {AttackEffectType.RedFlame, 1},
            {AttackEffectType.BlueFlame, 2},
            {AttackEffectType.Nebula, 3},
            {AttackEffectType.Blood, 4},
            {AttackEffectType.Water, 5}
        };
    }

    /// <summary>�a���G�t�F�N�g�𐶐��E�\������</summary>
    /// <param name="type">�G�t�F�N�g�̑���</param>
    /// <param name="pos">�G�t�F�N�g�̐����ʒu</param>
    /// <param name="player">�G�t�F�N�g�̈ʒu</param>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������A2������j</param>
    public void PlaySlashEffect(AttackEffectType type, Vector3 pos, Transform player, int handIndex)
    {
        // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��Ɋ��蓖�Ă��Ă��邽�߁j
        if (!_slashEffectIndexMap.TryGetValue((type), out int index)) index = -1;

        // index�̒l������ł���ꍇ
        if (index >= 0 && index < _slashEffectList.Count)
        {
            switch (handIndex)
            {
                // �E��
                case 0:
                    // �E�����̃G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var rightEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    rightEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    rightEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_RIGHT, Space.Self);
                    break;

                // ����
                case 1:
                    // �������̃G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var leftEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    leftEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    leftEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_LEFT, Space.Self);
                    break;

                // ����
                case 2:
                    // �΂߉E�����̃G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalRightEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    diagonalRightEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalRightEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL, Space.Self);

                    // �΂ߍ������̃G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalLeftEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    diagonalLeftEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalLeftEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL, Space.Self);

                    break;

                // ��O�I�ȏ���
                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }

        // �s����index�ł���ꍇ
        else
        {
            Debug.LogError($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>�K�E�Z�G�t�F�N�g�𐶐��E�\������</summary>
    /// <param name="type">�G�t�F�N�g�̑���</param>
    /// <param name="pos">�G�t�F�N�g�̐����ʒu</param>
    /// <param name="player">�v���C���[�̈ʒu</param>
    public void PlayUltEffect(AttackEffectType type, Vector3 pos, Transform player)
    {
        if (!_ultEffectIndexMap.TryGetValue((type), out int index))
        {
            // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��蓖�Ă��Ă��邽�߁j
            index = -1;
        }

        // index�̒l������ł���ꍇ�A�G�t�F�N�g�𐶐����ăv���C���[�̕����ɍ��킹��
        if (index >= 0 && index < _ultEffectList.Count)
        {
            var effect = Instantiate(_ultEffectList[index], pos, Quaternion.identity, transform);
            effect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            effect.transform.Rotate(0, -180, 0, Space.Self);
        }

        // �s����index�ł���ꍇ
        else
        {
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>����U���G�t�F�N�g�𐶐��E�\������</summary>
    /// <param name="type">�G�t�F�N�g�̑���</param>
    /// <param name="pos">�G�t�F�N�g�̐����ʒu</param>
    /// <param name="player">�v���C���[�̈ʒu</param>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������j</param>
    public void PlayAltEffect(AttackEffectType type, Vector3 pos, Transform player, int handIndex)
    {
        // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��Ɋ��蓖�Ă��Ă��邽�߁j
        if (!_altEffectIndexMap.TryGetValue((type), out int index)) index = -1;

        // index�̒l������ł���ꍇ
        if (index >= 0 && index < _altEffectList.Count)
        {
            switch (handIndex)
            {
                // �E��
                case 0:
                    // �΂߉E�����̓���U���G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalRightEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);
                    diagonalRightEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalRightEffect.transform.Rotate(0, -180, SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL, Space.Self);

                    // �G�t�F�N�g�̃p�[�e�B�N�����Đ����A�K�؂ȃ^�C�~���O�Œ�~������
                    var particle = diagonalRightEffect.GetComponent<ParticleSystem>();
                    particle.Play();
                    
                    break;

                // ����
                case 1:
                    // �΂ߍ������̓���U���G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalLeftEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);
                    diagonalLeftEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalLeftEffect.transform.Rotate(0, -180, SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL, Space.Self);
                    break;

                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }
    }
}
