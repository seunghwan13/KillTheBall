using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public void OnClickGameStartButton()
    {
        // Debug.Log("���ӽ�ŸƮ");
        SceneManager.LoadScene("Scenes/MainGameScene");
        //SceneManager.LoadScene(1); �� ����� ������ �ٲ�� ���� �� ����
    }
}
