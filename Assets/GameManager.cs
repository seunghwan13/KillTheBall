using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int GAMETIME = 10;

    // event�� �޼ҵ� ������ ����,�������� �޼��带 ��� �Ǵ� ������ �� ����, Action���� �޾ƿ�, �Ű����� int�� ���� �޼ҵ��
    // �޼ҵ�? 2������ ��ȯ(output), �Ű�����(input)
    // �޼ҵ� ���� Action(��ȯŸ���� void), Function(��ȯŸ���� ����, Func<OutPutType>)
    public static event System.Action<int> onScoreChanged;

    public static event System.Action<int> onLeftGameTimeChanged;

    public static event System.Action onGameOver;

    [SerializeField] //�Ʒ� ��ҿ� �Ӽ��� �߰��ϴµ�, private ��ҵ� unity inspector���� ���� �� �ֵ��� ��
    private int _score;

    [SerializeField]
    private GameObject _targetPrefab; //prefab�� �����̶�� ��, ����ȭ �Ǿ��ִ� ������Ʈ, open�� selector�� ����Ͽ� �����ϰ� override revert�� ���󺹱�, apply�� ���� ���� ����

    [SerializeField]
    private Transform _leftbottom;

    [SerializeField]
    private Transform _rightTop;

    [SerializeField]
    private float _minSpawnTime, _maxSpawnTime;

    public LayerMask targeMask; //2. LayerMask �ڷ��� �̿�: GameManager �κ� targetmask Nothing���� ����, target���� ������ ��

    private int leftGameTime;


    private void Awake() // awa tab, �ڷ�ƾ
    {
        leftGameTime = GAMETIME;
        StartCoroutine(SpawnCoroutine());
        StartCoroutine(TimeInterval());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)  //���콺 Ŭ������ ��
        {
           

            // ������ ������Ʈ�� Ŭ���ߴ��� �Ǵ��ϴ� ��� 2����
            // 1. �������� �����ϱ�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ȭ����ǥ���� Ray�� ����
            //ScreenPointToRay�� �Ⱦ��� ���������� ����� ����� �Ʒ� �ּ� ����
            // Vector3 dir =
            //�̾... (Camere.main.ScreenToWorldPoint(Input.mousePoint) - Camera.main.transform.position).normalized;
            //�̾... ray = new Ray(Camera.main.transform.position, dir);
            RaycastHit hit; // Raycast�� ������� ����

            if(Physics.Raycast(ray, out hit, float.MaxValue, targeMask.value) == true) // out �żҵ� �ȿ��� out���� �־��� ģ������ ���� �־ ������
            {
                // Debug.Log(hit.transform.gameObject.name); ctrl+shift+c �ܼ�â Ȯ��
                _score += 100;
                if(onScoreChanged != null)
                {
                    onScoreChanged(_score);
                }
                //onScoreChanged?.Invoke(_score); �ε� ��밡��
                Destroy(hit.transform.gameObject);
            }
            // 2. Eventsystem �̿��ϱ� -> ui ���鼭 �ѹ� ����
        }

        // Layer�� �����浹 �����ڶ�� �� (�⺻���� default add�� �߰�), 0~31����, 32bit int�ڷ�����
        // LayerMask�� Layer�� ������ ����ŷ (���͸�)�ϴ� ����

        // 1. Bit �����̿�
        // ����Ƽ�� Layer�� int�� �ϳ��� ���  0 0 0 0 1 ... 0 0 0 (32��) & 0 0 0 0 1 ... 0 0 0 (32��) -> true
        // unity layermask bitwise�� �˻��ؼ� ��� ��
        
        // 2. LayerMask �ڷ��� �̿� if(Physics.Raycast(ray, out hit, float.MaxValue, targeMask.value) == true)
        
        // Tag�� �����浹 ������ ���� �ʰ� �����浹�� �Ǿ��� �� �̰��� �������� ������.. ������������ ���� ��   
    }

    private IEnumerator TimeInterval()
    {
        while (gameObject != null)
        {
            yield return new WaitForSeconds(1);
            leftGameTime--;

            if (onLeftGameTimeChanged != null)
            {
                onLeftGameTimeChanged(leftGameTime);
            }


            if (leftGameTime <= 0)
            {
                if (onGameOver != null)
                {
                    onGameOver();
                    this.StopAllCoroutines();
                    break; //yield break �� �ش� �ڷ�ƾ ���Ḧ ��
                }
            }

            
        }
    }


    // �ڷ�ƾ: ����Ƽ �ð� ���� ó��, ����Ƽ�� �̱� ������ ��� ����(�׷���, ���� �� ��Ƽ ������ ���ư�����...)
    // n�� ��� == n�� ���� ������ ����¡ ��
    // �� �ڷ�ƾ�� ����ϸ� �ذ�(���� ������Ʈ���� �����ϸ� �� ��, �ð����� �����ε�... ������Ʈ�� �ȵ�¡)
    private IEnumerator SpawnCoroutine()
    {
        while (gameObject != null)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnTime, _maxSpawnTime));
            // Debug.Log("1��...");
            GameObject copyObject = Instantiate(_targetPrefab);
            // ���� ������ ������ ������ ������ �簢���� ������ �� ����
            copyObject.transform.position = 
                new Vector3(Random.Range(_leftbottom.position.x, _rightTop.position.x),
                Random.Range(_leftbottom.position.y, _rightTop.position.y),
                Random.Range(_leftbottom.position.z, _rightTop.position.z));
            // �����ð� ������ ������ ���⼭ ������ �� �ִ°�?
            // yield retuen new WaitForSeconds(3f);
            // Destroy(copyObject); 
            // �̷������� �����ϸ� 3�ʴ���ϴ� ���� ������ �Ұ�!

            // �Ʒ� �ڷ�ƾ�� ������
            StartCoroutine(DelayDestroy(copyObject));
        }
        // Debug.Log("����1");
        // yield return null; 
        // �� ������ ���
        // yield return new WaitForSeconds(1); // n�� ���
        // 1�� ���
        // yield return new WaitForSecondsRealtime(2); // �����ð� n�� ���
        // �����ð� 2�� ���
        // Debug.Log("����2");
    }
    private IEnumerator DelayDestroy(GameObject target)
    {
        yield return new WaitForSeconds(3f);
        if(target != null)
        {
            Destroy(target);
        }
        // ���� ���� �ʾ����� �����ϱ� ���� if(target != null) ó���� ����
    }

    // �����ð� ������ ����
    // 1. ������ ���ÿ� �ڷ�ƾ �ϳ� �� ����(���� ���� ��)
    // 2. �ش� ������Ʈ�� n�� �� ������� ��ũ��Ʈ�� ����(���� ��𼭵�...���� �߻� ����)
    // Ŭ�� �� ����
}
