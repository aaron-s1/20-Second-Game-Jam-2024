using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BobFacesPlayer : MonoBehaviour
{
    [SerializeField] GameObject jaw;
    [SerializeField] GameObject self; // functions as the Head.
    [SerializeField] Animator anim;

    Vector3 targetPos;
    Vector3 targetRotation;

    void Awake()
    {
        anim = GetComponent<Animator>();
        // StartCoroutine(FacePlayer());
    }

    void Start()
    {
        RotateHead();
    }

    void RotateHead()
    {
        // anim.enabled = false;
        targetRotation = new Vector3 (self.transform.rotation.x, self.transform.rotation.y + 70, self.transform.rotation.z);
        StartCoroutine(RotateObj(self, targetRotation, 1f, false));
    }

    void OpenMouth()
    {
        targetPos = new Vector3 (jaw.transform.localPosition.x, jaw.transform.localPosition.y - 0.3f, jaw.transform.localPosition.z + 0.01f);
        StartCoroutine(MoveObj(jaw, targetPos, 1f, true));
    }

    public IEnumerator FacePlayer()
    {        
        anim.SetTrigger("face player");

        yield return null;
        yield break;
    }

    IEnumerator RotateObj(GameObject obj, Vector3 destination, float duration, bool rotateLocal = true)
    {
        anim.enabled = false;
        Vector3 startRotation = rotateLocal ? obj.transform.localEulerAngles : obj.transform.eulerAngles;
        
        if (rotateLocal)
            startRotation = obj.transform.localEulerAngles;
        else
            startRotation = obj.transform.eulerAngles;

        // Debug.Log($"starting at {startRotation}, ending at {destination}");
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            if (rotateLocal)
                obj.transform.localEulerAngles = Vector3.Lerp(startRotation, destination, elapsedTime / duration);
            else
                obj.transform.eulerAngles = Vector3.Lerp(startRotation, destination, elapsedTime / duration);                
            
            yield return null;
        }

        if (rotateLocal)
            obj.transform.localEulerAngles = destination;
        else
            obj.transform.eulerAngles = destination;
        // anim.enabled = true;

        OpenMouth();
    }

    IEnumerator MoveObj(GameObject obj, Vector3 destination, float duration, bool moveLocal = false)
    {
        anim.enabled = false;
        Vector3 startPosition;

        if (moveLocal)
            startPosition = obj.transform.localPosition;
        else startPosition = obj.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            if (moveLocal)
                obj.transform.localPosition = Vector3.Lerp(startPosition, destination, elapsedTime / duration);
            else
                obj.transform.position = Vector3.Lerp(startPosition, destination, elapsedTime / duration);

            yield return null;
        }

        if (moveLocal)
            obj.transform.localPosition = destination;
        else
            obj.transform.position = destination;
        // anim.enabled = true;
    }    
}
