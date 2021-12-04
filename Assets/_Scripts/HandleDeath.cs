using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleDeath : MonoBehaviour
{

    public void DeathLoadScene(int livesRemaining) {
        if (livesRemaining == 2) {
            StartCoroutine(WaitToLoad("_Scene_1"));
        }
        else if (livesRemaining == 1) {
            StartCoroutine(WaitToLoad("_Scene_2"));
        }
        else {
            StartCoroutine(WaitToLoad("_Game_Lose"));
        }
    }

    private IEnumerator WaitToLoad(string scene) {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(scene);
    }
}
