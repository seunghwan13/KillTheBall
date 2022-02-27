using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public void OnClickGameStartButton()
    {
        // Debug.Log("게임스타트");
        SceneManager.LoadScene("Scenes/MainGameScene");
        //SceneManager.LoadScene(1); 이 방법은 순서가 바뀌면 꼬일 수 있음
    }
}
