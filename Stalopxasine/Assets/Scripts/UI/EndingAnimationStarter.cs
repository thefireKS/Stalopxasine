using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingAnimationStarter : MonoBehaviour
{
    public float AnimTime;
    public string AnimName;
    public Animator anim;
    public int EndingNumber;
    public void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Update()
    {
        if (Globals.StartEnding)
            StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        anim.Play(AnimName);
        yield return new WaitForSeconds(AnimTime);
        SceneManager.LoadScene("Character Selection");
        Globals.StartEnding = false;
    }
}
