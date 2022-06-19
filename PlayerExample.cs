using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerExample : MonoBehaviour
{

    public float walkPerceptionRadius = 1.5f;      // walk perception radius
    public float sprintPerceptionRadius = 2f;  // sprint perception radius
     
    private FirstPersonController fpsc;
    private SphereCollider sphereCollider;  // invoke another collider for walk/sprint radius

    // Start is called before the first frame update
    public void Start()
    {
        fpsc = GetComponent<FirstPersonController>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void Update()
    {
        // if 0(walk) use walk radius else sprint radius
        if (fpsc.GetPlayerStealProfile() == 0)
        {
            sphereCollider.radius = walkPerceptionRadius;
        }else
        {
            sphereCollider.radius = sprintPerceptionRadius;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Zombie"))
        {
            other.GetComponent<AI_zombie>().OnAware();
        }
    }
}
