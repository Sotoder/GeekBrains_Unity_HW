using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //Move Player
    [SerializeField] private float sensetivity;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMult;
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private LayerMask _groundMasck;

    private Animator _animator;
    private bool _isGrounded;
    private Vector3 _direction;
    private float _mouseLookX;
    private Vector3 _normDirection;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        _normDirection = _direction.normalized;

        MoveCheck(_direction);
        PlayerRotate();
        IsGroundedUpate();
        JumpLogic();
        //MovementLogic(_direction * _speed);
    }


    private void MoveCheck(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            _animator.SetBool("IsStay", false);
            _animator.SetFloat("MoveDirection", Mathf.Lerp(_animator.GetFloat("MoveDirection"), _normDirection.z, Time.deltaTime * _speed));

            StrafeMoveCheck(direction);
            SideMoveChek(direction);
        }
        else
        {
            SmoothEndAnimation("MoveDirection", "IsStay", true);

            SmoothEndAnimation("SideStepDirection", "SideStep", false);
        }
    }

    private void StrafeMoveCheck(Vector3 direction)
    {
        if (direction.z != 0 && direction.x != 0)
        {
            _animator.SetFloat("TurnDirection", Mathf.Lerp(_animator.GetFloat("TurnDirection"), _normDirection.x, Time.deltaTime * _speed));
        }
        else if (direction.x == 0)
        {
            SmoothEndAnimation("TurnDirection");
        }
    }

    private void SideMoveChek(Vector3 direction)
    {
        if (direction.z == 0 && direction.x != 0)
        {
            _animator.SetBool("SideStep", true);
            _animator.SetFloat("SideStepDirection", Mathf.Lerp(_animator.GetFloat("SideStepDirection"), _normDirection.x, Time.deltaTime * _speed));
        } else if (direction.z != 0)
        {
            SmoothEndAnimation("SideStepDirection");
            _animator.SetBool("SideStep", false);
        }
    }

    private void PlayerRotate()
    {
        _mouseLookX = Input.GetAxis("Mouse X") * sensetivity * Time.deltaTime;

        if(_mouseLookX != 0 && _animator.GetBool("IsStay"))
        {
            if (_mouseLookX > 0)
            {
                _animator.SetFloat("TurnDirection", Mathf.Lerp(_animator.GetFloat("TurnDirection"), 1, Time.deltaTime * _speed));
            } else
            {
                _animator.SetFloat("TurnDirection", Mathf.Lerp(_animator.GetFloat("TurnDirection"), -1, Time.deltaTime * _speed));
            }
            
        } else if (_animator.GetBool("IsStay"))
        {

            SmoothEndAnimation("TurnDirection");
        }

        if (!_animator.GetBool("IsStay"))
            transform.Rotate(0, _mouseLookX, 0);
    }


    //private void MovementLogic(Vector3 speed)
    //{
    //    _rb.AddForce(transform.forward * speed.z, ForceMode.Impulse);
    //    _rb.AddForce(transform.right * speed.x, ForceMode.Impulse);

    //}

    private void JumpLogic()
    {
        if (Input.GetButton("Jump"))
        {
            if (_isGrounded)
            {
                if (_direction.z < 0)
                {
                    _animator.SetBool("BackFlip", true);
                }
                _animator.SetBool("Jump", true);
                _animator.SetBool("OnGround", false);
            }
        }
    }

    private void IsGroundedUpate()
    {
        _isGrounded = Physics.CheckSphere(_groundDetector.position, 0.2f, _groundMasck);
        if (_isGrounded)
        {
            _animator.SetBool("Jump", false);
            _animator.SetBool("OnGround", true);
            _animator.SetBool("BackFlip", false);
        }
    }

    private void SmoothEndAnimation(string firstParamName)
    {
        if (_animator.GetFloat(firstParamName) > 0.1f || _animator.GetFloat(firstParamName) < -0.1f)
        {
            _animator.SetFloat(firstParamName, Mathf.Lerp(_animator.GetFloat(firstParamName), 0, Time.deltaTime * _speed));
        }
        else
        {
            _animator.SetFloat(firstParamName, 0);
        }
    }

    private void SmoothEndAnimation(string firstParamName, string secondParamName, bool paramFlag)
    {
        if (_animator.GetFloat(firstParamName) > 0.1f || _animator.GetFloat(firstParamName) < -0.1f)
        {
            _animator.SetFloat(firstParamName, Mathf.Lerp(_animator.GetFloat(firstParamName), 0, Time.deltaTime * _speed));
        }
        else
        {
            _animator.SetFloat(firstParamName, 0);
            _animator.SetBool(secondParamName, paramFlag);
        }
    }
}
