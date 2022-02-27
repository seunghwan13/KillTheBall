using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverUIPanel;

    [SerializeField]
    private TextMeshProUGUI _scoreLabel;

    [SerializeField]
    private TextMeshProUGUI _timeLabel;


    // Start is called before the first frame update
    void Awake()
    {
        _gameOverUIPanel.SetActive(false);
        _scoreLabel.text = "0";
        _timeLabel.text = $"{GameManager.GAMETIME / 60}:{GameManager.GAMETIME % 60}";
        GameManager.onScoreChanged += OnScoreChanged;
        GameManager.onLeftGameTimeChanged += OnLeftGameTimeChanged;
        GameManager.onGameOver += onGameOver;
    }

    private void onGameOver()
    {
        _gameOverUIPanel.SetActive(true);
    }

    private void OnLeftGameTimeChanged(int leftTime)
    {
        _timeLabel.text = $"{leftTime / 60:00}:{leftTime % 60:00}"; // 60:00:00으로하면 세자리까지(사용자 정의 텍스트표시형식)
    }

    private void OnDestroy()
    {
        GameManager.onScoreChanged -= OnScoreChanged;
        GameManager.onLeftGameTimeChanged -= OnLeftGameTimeChanged;
    }

    private void OnScoreChanged(int score)
    {
        _scoreLabel.text = $"{score}";
    }

    public void OnClickPlayAgainButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

// Texture는 모든 이미지의 최상위 부모클래스
// sprite 는 Texture를 상속받은 자식 클래스임 (Texture를 가지고 꾸밀 수 있는 옵션 추가)
// 이미지가 Default일 때 Image 즉 sprite에는 들어갈 수 없음
