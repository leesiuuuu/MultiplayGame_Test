using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
{
    public PhotonView photonView;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;
    public Text PlayerNameText;
    public Collider2D AtkCollider;

    public bool IsGrounded = false;
    public float MoveSpeed;
    public float JumpForce;

    private int ComboCount = 0;
    private void Awake()
    {
        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
            PlayerNameText.text = PhotonNetwork.playerName;
        }
        else
        {
            PlayerNameText.text = photonView.owner.name;
            PlayerNameText.color = Color.cyan;
        }
    }
    private void Update()
    {
        if (photonView.isMine)
        {
            CheckInput();
        }
    }
    private void CheckInput()
    {
        var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += move * MoveSpeed * Time.deltaTime;
        //움직임 코드
        if (Input.GetKeyDown(KeyCode.A))
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }
        //점프 코드
        if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded)
        {
            photonView.RPC("Jump", PhotonTargets.AllBuffered);
        }
        if (Input.GetKeyDown(KeyCode.J) && !IsGrounded)
        {
            photonView.RPC("Attack", PhotonTargets.AllBuffered);
        }
        //애니메이션 코드
        if (IsGrounded && Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("isJumping", true);
        }
        if (Input.GetKey(KeyCode.J) && !IsGrounded)
        {
            if (ComboCount == 0)
            {
                anim.SetBool("Attack", true);
                anim.SetBool("Attack2", false);
            }
            else if(ComboCount == 1)
            {
                anim.SetBool("Attack2", true);
                anim.SetBool("Attack", false);
            }
            else
            {
                anim.SetBool("Attack2", false);
                anim.SetBool("Attack", false);
                ComboCount = 0;
            }
                
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }
    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }
    [PunRPC]
    private void Jump()
    {
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Force);
        IsGrounded = true;
    }
    [PunRPC]
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
            anim.SetBool("isJumping", false);
        }
    }
    [PunRPC]
    private void Attack()
    {
        ComboCount++;
    }
}
