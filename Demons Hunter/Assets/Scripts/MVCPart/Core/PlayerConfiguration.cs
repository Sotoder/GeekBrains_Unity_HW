using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerConfiguration), menuName = "UnitConfigs/" + nameof(PlayerConfiguration), order = 0) ]
public class PlayerConfiguration: ScriptableObject
{
    [SerializeField] private GameObject _playerPref;
    [SerializeField] private int _maxHP;
    [SerializeField] private float _mouseSensetivity;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runSpeedMult;

    public GameObject PlayerPref => _playerPref;
    public int MaxHP => _maxHP;
    public float MouseSensetivity => _mouseSensetivity;
    public float MoveSpeed => _moveSpeed;
    public float RunSpeedMult => _runSpeedMult;
}