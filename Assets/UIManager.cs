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
        _timeLabel.text = $"{leftTime / 60:00}:{leftTime % 60:00}"; // 60:00:00�����ϸ� ���ڸ�����(����� ���� �ؽ�Ʈǥ������)
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

// Texture�� ��� �̹����� �ֻ��� �θ�Ŭ����
// sprite �� Texture�� ��ӹ��� �ڽ� Ŭ������ (Texture�� ������ �ٹ� �� �ִ� �ɼ� �߰�)
// �̹����� Default�� �� Image �� sprite���� �� �� ����
