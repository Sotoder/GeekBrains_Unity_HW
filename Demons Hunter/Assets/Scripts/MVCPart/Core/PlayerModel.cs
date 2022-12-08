using System;
using UnityEngine;

public class PlayerModel
{
    public Action OnJump;
    public Action OnDead;

    public readonly Transform PlayerViewTransform;
    public readonly Transform PlayerHeadTransform;
    public readonly Rigidbody PlayerRB;
    public readonly Animator PlayerAnimator;
    public readonly Health Health;

    public float PlayerHeadRotation { get; private set; }
    public float PlayerRotation { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsJumped { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsRun { get; private set; }
    public bool IsWalk { get; private set; }

    public PlayerModel(PlayerConfiguration playerConfig, PlayerView playerView)
    {
        Health = new Health(playerConfig.MaxHP);
        PlayerRB = playerView.Rigidbody;
        PlayerViewTransform = playerView.transform;
        PlayerHeadTransform = playerView.Head.transform;
        PlayerAnimator = playerView.Animator;
    }

    public void SetJumped(bool isJumped)
    {
        IsJumped = isJumped;
        OnJump?.Invoke();
    }

    public void SetIsGraunded(bool isGraunded)
    {
        IsGrounded = isGraunded;
    }

    public void SetIsRun(bool isRun)
    {
        IsRun = isRun;
    }

    public void SetIsWalk(bool isWalk)
    {
        IsWalk = isWalk;
    }

    public void SetPlayerHeadRotation(float value)
    {
        PlayerHeadRotation = value;
    }

    public void SetPlayerRotation(float value)
    {
        PlayerRotation = value;
    }

    public void SetDead(bool isDead)
    {
        IsDead = isDead;
        OnDead?.Invoke();
    }
}
