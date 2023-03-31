using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretLevelEnter : MonoBehaviour
{
    private IEnumerator EndLoading()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Secret Level");
    }

    void Update()
    {
        if (SecretCode.code == "4231")
            StartCoroutine(EndLoading());
    }
}
