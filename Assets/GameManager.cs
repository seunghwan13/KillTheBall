using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int GAMETIME = 10;

    // event는 메소드 변수에 연결,여러개의 메서드를 등록 또는 해제할 수 있음, Action으로 받아옴, 매개변수 int를 가진 메소드들
    // 메소드? 2가지임 반환(output), 매개변수(input)
    // 메소드 변수 Action(반환타입이 void), Function(반환타입이 있음, Func<OutPutType>)
    public static event System.Action<int> onScoreChanged;

    public static event System.Action<int> onLeftGameTimeChanged;

    public static event System.Action onGameOver;

    [SerializeField] //아래 요소에 속성을 추가하는데, private 요소도 unity inspector에서 보일 수 있도록 함
    private int _score;

    [SerializeField]
    private GameObject _targetPrefab; //prefab은 원본이라는 뜻, 파일화 되어있는 오브젝트, open과 selector을 사용하여 수정하고 override revert는 원상복귀, apply는 현재 상태 적용

    [SerializeField]
    private Transform _leftbottom;

    [SerializeField]
    private Transform _rightTop;

    [SerializeField]
    private float _minSpawnTime, _maxSpawnTime;

    public LayerMask targeMask; //2. LayerMask 자료형 이용: GameManager 부분 targetmask Nothing으로 변경, target으로 변경할 것

    private int leftGameTime;


    private void Awake() // awa tab, 코루틴
    {
        leftGameTime = GAMETIME;
        StartCoroutine(SpawnCoroutine());
        StartCoroutine(TimeInterval());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)  //마우스 클릭했을 때
        {
           

            // 유저가 오브젝트를 클릭했는지 판단하는 방법 2가지
            // 1. 수동으로 구현하기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //화면좌표에서 Ray로 변경
            //ScreenPointToRay를 안쓰고 세부적으로 만드는 방법은 아래 주석 참고
            // Vector3 dir =
            //이어서... (Camere.main.ScreenToWorldPoint(Input.mousePoint) - Camera.main.transform.position).normalized;
            //이어서... ray = new Ray(Camera.main.transform.position, dir);
            RaycastHit hit; // Raycast의 결과값을 저장

            if(Physics.Raycast(ray, out hit, float.MaxValue, targeMask.value) == true) // out 매소드 안에서 out으로 넣어준 친구에게 값을 넣어서 내보냄
            {
                // Debug.Log(hit.transform.gameObject.name); ctrl+shift+c 콘솔창 확인
                _score += 100;
                if(onScoreChanged != null)
                {
                    onScoreChanged(_score);
                }
                //onScoreChanged?.Invoke(_score); 로도 사용가능
                Destroy(hit.transform.gameObject);
            }
            // 2. Eventsystem 이용하기 -> ui 들어가면서 한번 적용
        }

        // Layer은 물리충돌 구분자라고 함 (기본값은 default add로 추가), 0~31까지, 32bit int자료형임
        // LayerMask는 Layer을 가지고 마스킹 (필터링)하는 과정

        // 1. Bit 연산이용
        // 유니티는 Layer을 int형 하나로 취급  0 0 0 0 1 ... 0 0 0 (32개) & 0 0 0 0 1 ... 0 0 0 (32개) -> true
        // unity layermask bitwise를 검색해서 배울 것
        
        // 2. LayerMask 자료형 이용 if(Physics.Raycast(ray, out hit, float.MaxValue, targeMask.value) == true)
        
        // Tag는 물리충돌 구분은 하지 않고 물리충돌이 되었을 때 이것이 나무인지 돌인지.. 하위개념으로 들어가게 함   
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
                    break; //yield break 는 해당 코루틴 종료를 뜻
                }
            }

            
        }
    }


    // 코루틴: 유니티 시간 관련 처리, 유니티는 싱글 쓰레드 기반 엔진(그래픽, 물리 등 멀티 쓰레드 돌아가지만...)
    // n초 대기 == n초 동안 게임이 프리징 됨
    // 즉 코루틴을 사용하면 해결(절대 업데이트에서 실행하면 안 됨, 시간관리 때문인데... 업데이트는 안되징)
    private IEnumerator SpawnCoroutine()
    {
        while (gameObject != null)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnTime, _maxSpawnTime));
            // Debug.Log("1초...");
            GameObject copyObject = Instantiate(_targetPrefab);
            // 왼쪽 밑점과 오른쪽 윗점만 있으면 사각형을 정의할 수 있음
            copyObject.transform.position = 
                new Vector3(Random.Range(_leftbottom.position.x, _rightTop.position.x),
                Random.Range(_leftbottom.position.y, _rightTop.position.y),
                Random.Range(_leftbottom.position.z, _rightTop.position.z));
            // 일정시간 지나면 삭제를 여기서 구현할 수 있는가?
            // yield retuen new WaitForSeconds(3f);
            // Destroy(copyObject); 
            // 이런식으로 구현하면 3초대기하는 동안 생성이 불가!

            // 아래 코루틴을 실행함
            StartCoroutine(DelayDestroy(copyObject));
        }
        // Debug.Log("실행1");
        // yield return null; 
        // 한 프레임 대기
        // yield return new WaitForSeconds(1); // n초 대기
        // 1초 대기
        // yield return new WaitForSecondsRealtime(2); // 실제시간 n초 대기
        // 실제시간 2초 대기
        // Debug.Log("실행2");
    }
    private IEnumerator DelayDestroy(GameObject target)
    {
        yield return new WaitForSeconds(3f);
        if(target != null)
        {
            Destroy(target);
        }
        // 삭제 되지 않았음을 가정하기 위해 if(target != null) 처리를 진행
    }

    // 일정시간 지나면 삭제
    // 1. 생성과 동시에 코루틴 하나 더 실행(나름 좋은 듯)
    // 2. 해당 오브젝트에 n초 뒤 삭제라는 스크립트를 생성(언제 어디서든...버그 발생 많음)
    // 클릭 시 삭제
}
