using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleDeath : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject player;

    public void DeathLoadScene() {
        StartCoroutine(WaitToLoad("_Game_Lose"));
    }

    public void StartNextLife() {
        StartCoroutine(WaitToStartNextLife());
    }

    private IEnumerator WaitToLoad(string scene) {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(scene);
    }

    private IEnumerator WaitToStartNextLife() {
        player.SetActive(false);
        yield return new WaitForSeconds(2);
        player.SetActive(true);
    }
}
