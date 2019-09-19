using RamboTeam.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]

public class TargetPoint : MonoBehaviorBase
{
    public static TargetPoint Instance;
    public Sprite defaultModel;
    public Sprite LockModel;
    private bool isInLockState = false;
    private SpriteRenderer rend;
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();

        Instance = this;
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (isInLockState)
        {
            float t = Mathf.PingPong(Time.time * 2F, 1.0F);
            rend.material.color = Color.Lerp(Color.red, Color.black, t);
        }
    }


    public void SetData(Vector3 Position, bool isLock)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (isLock != isInLockState)
        {
            isInLockState = isLock;
            rend.sprite = isInLockState ? LockModel : defaultModel;

            if (isInLockState)
            {
                audioSource.Play();
            }
        }
        else
        {
            rend.material.color = Color.black;
        }

        transform.position = Position;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
