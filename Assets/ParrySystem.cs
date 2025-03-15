using UnityEngine;

public class ParrySystem : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayParryAnimation()
    {
        animator.SetTrigger("Parry");
    }
}