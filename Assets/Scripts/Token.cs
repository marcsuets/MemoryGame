using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public int id;
    public GameObject gm;
    private Animator animator;
    private AudioClip turnCard;
    
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController");
        animator = GetComponent<Animator>();
        turnCard = gm.GetComponent<GameManager>().turnCard; // Sound
    }
    
    private void OnMouseDown()
    {
        gm.GetComponent<GameManager>().SetSelectedCard(id);
    }

    public void StartAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hidden"))
        {
            animator.SetTrigger("Reveal");
            gm.GetComponent<AudioSource>().PlayOneShot(turnCard);
        }
    }
}