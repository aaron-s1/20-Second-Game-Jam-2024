using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobFacesPlayer : MonoBehaviour
{
    [SerializeField] Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator FacePlayer()
    {        
        anim.SetTrigger("face player");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.ResetTrigger("face player");
        yield return new WaitForEndOfFrame();
        anim.SetTrigger("open mouth");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.ResetTrigger("open mouth");
    }
}
