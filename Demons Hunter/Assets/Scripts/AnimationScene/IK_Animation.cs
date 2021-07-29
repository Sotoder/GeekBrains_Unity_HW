using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class IK_Animation : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    [SerializeField] private Transform _catchObject;
    [SerializeField] private Transform _catchPoint;
    [SerializeField] private Transform _wallPoint;
    [SerializeField] private LayerMask _rayLayer;

    private Transform _lookObject;
    private Animator _animator;
    private float _speed = 0.5f;
    private float _weight;
    private Dictionary<Transform, float> _lookObjectContainer = new Dictionary<Transform, float>();
    private bool _isInVision;
    private bool _isWallNear;
    private bool _isTargeted;
    private GameObject _catchPointForWall;


    private const string LookObjectTag = "InteractiveLook";
    private const string CatchObjectTag = "InteractiveCatch";
    private const string WallsTag = "Walls";


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //CheckIK(); //�� �������� ���������� ������ � ������ ����� �� ����, ���� ��������� ��������� � �������� �� ��������,
        //� ��� ������� ������ ���� ����� ����������� � OnAnimatorIK. ����� �� ����� ������ ���������, ������� ����� �� ������������.
        if (_lookObjectContainer.Count > 0)
        {
            _lookObject = GetObject(_lookObjectContainer);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!_isActive) return;
        
        if(_lookObject != null)
        {
            _animator.SetLookAtWeight(_weight);
            _animator.SetLookAtPosition(_lookObject.position);
        }

        if(IsObjectNear(_catchObject) && _isInVision)
        {
            //float angle = Vector3.Angle(_catchPoint.position, transform.forward); // ����� ������������ ����, �� ����� ��� �������� �� ���� ����� ��������
            //Debug.Log(angle);                                                     // � ������� ������ �����. ����� �������� �� ���� �������� ������ ������ �
            // ������������ ������ ��.

            SetIKAnimator(_catchPoint);
        }

        if (_isWallNear)
        {
            if (Physics.Raycast(_wallPoint.position, Vector3.forward, out var hit, 0.8f, _rayLayer))
            {
                if (!_isTargeted)
                {
                    _catchPointForWall = new GameObject();
                    _catchPointForWall.transform.position = _wallPoint.position;
                    _catchPointForWall.transform.Translate(new Vector3 (0,0,hit.distance));
                    _catchPointForWall.transform.rotation = _wallPoint.rotation;

                    _isTargeted = true;
                }

                SetIKAnimator(_catchPointForWall.transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckIK();

        switch (other.tag)
        {
            case LookObjectTag: 
                _lookObjectContainer.Add(other.transform, Vector3.Distance(other.transform.position, transform.position));
                break;

            case CatchObjectTag: 
                _isInVision = true;
                break;

            case WallsTag:
                _isWallNear = true;
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CheckIK();

        if (other.CompareTag(LookObjectTag))
        {
            UpdateDistanceValue(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        switch (other.tag)
        {
            case LookObjectTag:
                _lookObjectContainer.Remove(other.transform);
                if (_lookObjectContainer.Count < 1) _lookObject = null;
                break;

            case CatchObjectTag:
                _isInVision = false;
                break;

            case WallsTag:
                _isWallNear = false;
                break;
        }

        CheckIK();
    }

    private void SetIKAnimator(Transform _catchPoint)
    {
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _weight);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _weight);
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, _catchPoint.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, _catchPoint.rotation);
    }

    private void UpdateDistanceValue(Collider other)
    {
        if (_lookObjectContainer.TryGetValue(other.transform, out float value))
        {
            _lookObjectContainer[other.transform] = Vector3.Distance(other.transform.position, transform.position);
        }
    }

    private Transform GetObject(Dictionary<Transform, float> _objectContainer) => _objectContainer.OrderBy(x => x.Value)
                                                                                                .ToDictionary(x => x.Key, x => x.Value)
                                                                                                .First().Key;

    private bool IsObjectNear(Transform catchObject)
    {
        float distance = Vector3.Distance(catchObject.transform.position, transform.position);

        if (distance > 0.4f && distance < 1.5f) 
            return true;
        else 
            return false;
    }

    private void CheckIK()
    {
        if (_animator.GetBool("IsStay"))
        {
            _isActive = true;
            _weight = Mathf.Lerp(_weight, 3, Time.deltaTime * _speed);
        }
        else
        {
            _isActive = false;
            _weight = 0;

            if (_isTargeted) //�������� ������� ������������ ��� ������� �����
            {
                Destroy(_catchPointForWall);
                _isTargeted = false;
            }
        }
    }
}
