using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : MonoBehaviour
{

    public AudioSource AudioSource;  // invoke audio source
    public AudioClip[] patrolClips;    // idle sound
    public AudioClip[] chaseClips;   // chase sound
    public AudioClip[] AttackClips;     // attack sound
    public AudioClip DeadClip;       // dead sound

    // Invoke
    public AI_zombie AI;

    // bool
    public bool PlaySound = true;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponentInParent<AudioSource>();
        AI = GetComponent<AI_zombie>();
    }

    void Update()
    {
        if (PlaySound == true)
        {
            StartCoroutine(PlayZombieSound());
            return;
        }
    }

    IEnumerator PlayZombieSound()
    {
        // patrol sound
        if (AI.isAware == false && AI.isDead == false)
        {
            PlaySound = false;

            int randomIdleSound = Random.Range(0, patrolClips.Length);
            AudioSource.PlayOneShot(patrolClips[randomIdleSound]);

            yield return new WaitForSeconds(7f);

            if (AI.isDead == true)
            {
               Dead();      
            }

            PlaySound = true;
        }

        // chase sound
        else if (AI.isAware == true && AI.isAttack == false && AI.isDead == false)
        {
            PlaySound = false;

            int randomChaseSound = Random.Range(0, chaseClips.Length);
            AudioSource.PlayOneShot(chaseClips[randomChaseSound]);

            if (randomChaseSound == 0)
            {
                yield return new WaitForSeconds(4.1f);
            }

            else if (randomChaseSound == 1)
            {
                yield return new WaitForSeconds(4.35f);
            }

            else if (randomChaseSound == 2)
            {
                yield return new WaitForSeconds(3.75f);
            }

            if (AI.isDead == true)
            {
                Dead();
            }

            PlaySound = true;
        }

        // attack sound
        else if (AI.isAttack == true && AI.isDead == false)
        {
            PlaySound = false;

            int randomAttackSound = Random.Range(0, AttackClips.Length);
            AudioSource.PlayOneShot(AttackClips[randomAttackSound]);

            if (randomAttackSound == 0)
            {
                yield return new WaitForSeconds(1.32f);
            }

            else if (randomAttackSound == 1)
            {
                yield return new WaitForSeconds(1.315f);
            }

            else if (randomAttackSound == 2)
            {
                yield return new WaitForSeconds(1.12f);
            }

            else if (randomAttackSound == 3)
            {
                yield return new WaitForSeconds(1.11f);
            }

            if (AI.isDead == true)
            {
                Dead();
            }

            PlaySound = true;
        }
    }

    // dead sound 
    void Dead()
    {
        if (AI.isDead == true)
        {
            AudioSource.PlayOneShot(DeadClip);
            return;
        }
    }
}
