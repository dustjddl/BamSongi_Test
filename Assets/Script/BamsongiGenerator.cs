using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BamsongiGenerator : MonoBehaviour
{
    // 프리팹 설계도 전달을 위한 public GameObject 변수 선언
    public GameObject gBamsongiPrefab = null;

    // 파워차징을 위한 게이지 변수
    public Image iChargeGauge = null;
    public Image iChargeGaugeBg = null;

    private float ChargeSpeed = 1000f;  // 게이지 차징속도
    private float MaxCharge = 2000f;    // 최대 차징게이지 값

    private float CurrentCharge = 0f; // 현재 차징된 게이지의 값
    //private bool Click = false; // 마우스 클릭 허용유무 
    private bool Charging = false; // 파워게이지 증가감소 제어변수

    // Instantiate된 밤송이 오브젝트 저장 변수
    GameObject insBamsongiPrefab = null;

    // 밤송이 월드 좌표
    Vector3 vBamsongiworldDir = Vector3.zero;

    void Start()
    {
        // 파워게이지 초기세팅
        //Click = true;
        Charging = true;
        iChargeGauge.gameObject.SetActive(false);
        iChargeGaugeBg.gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        // 줌 상태를 확인하기 위해 씬 안에서 CameraController 컴포넌트를 가진 객체를 찾음
        CameraController zoomController = FindFirstObjectByType<CameraController>();
        // 줌 동작 중일 경우, 밤송이 발사를 막음
        if (zoomController != null && zoomController.IsZooming)
            return; // 줌 동작 중엔 발사 불가
        {
            // 게임화면을 마우스로 클릭했을때 작동하는 함수
            if (Input.GetMouseButtonDown(0))
            {
                CurrentCharge = 0f;     // 현재 차징된 값 초기화
                iChargeGauge.gameObject.SetActive(true);
                iChargeGaugeBg.gameObject.SetActive(true);
            }

            // 게임화면을 마우스로 누르고 있을때 파워게이지 작동
            if (Input.GetMouseButton(0))
            {
                /* Charging 변수가 true일때 CurrentCharge 값이 증가하다가  MaxCharge값에 도달하면
                   Charging 변수 flase로 변환하고 CurrentCharge 값이 감소하다 0이 되면 다시 Charging 값을 
                   true로 변환하고 이 과정을 반복
                */
                if (Charging == true)
                {
                    CurrentCharge += ChargeSpeed * Time.deltaTime;
                    if (CurrentCharge >= MaxCharge)
                    {
                        CurrentCharge = MaxCharge;
                        Charging = false;
                    }
                }
                else
                {
                    CurrentCharge -= ChargeSpeed * Time.deltaTime;
                    if (CurrentCharge <= 0f)
                    {
                        CurrentCharge = 0f;
                        Charging = true;
                    }
                }
                // CurrentCharge 값에따라 fillAmount값 변화
                iChargeGauge.fillAmount = CurrentCharge / MaxCharge;
            }

            // 마우스 버튼을 뗐을때 작동하는 함수
            if (Input.GetMouseButtonUp(0))
            {
                UiManager.Instance.UpdateGameCount();               // 밤송이가 발사되면 게임카운트 1 감소하는 함수
                iChargeGauge.gameObject.SetActive(false);           // 파워게이지 비활성화
                iChargeGaugeBg.gameObject.SetActive(false);         // 파워게이지 배경 비활성화
                // 게임을 실행하는 도중에 밤송이 오브젝트를 생성
                insBamsongiPrefab = Instantiate(gBamsongiPrefab);

                /*
                 * Ray 클래스
                 *  Ray(레이)는 이름 그대로 광선이며, 광원의 좌표(Origin)와 광선의 방향(direction)을 멤버 변수로 갖음
                 *  Ray는 콜라이더가 적용된 오브젝트와 충돌을 감지하는 특징이 있음
                 *  ScreenPointToRay 메서드의 반환값으로 얻을 수 있는 Ray는 Origin이 Main Camera의 좌표고.
                 *  direction 방향으로 밤송이를 날리기 때문에 direction 벡터가 가진 normalized 변수를 사용해 길이가 1인 벡터를 만든 후
                 *  힘을 2000 곱한다. 일단 길이를 1 벡터로 해서 direction 벡터 크기에 관계없이 밤송이에 일정한 힘을 가할 수 있음
                 */

                Ray ScreenPointToRayBamsongi = Camera.main.ScreenPointToRay(Input.mousePosition);

                vBamsongiworldDir = ScreenPointToRayBamsongi.direction;

                insBamsongiPrefab.GetComponent<BamsongiController>().f_TargetShoot(vBamsongiworldDir.normalized * CurrentCharge);

                
            }
        }
        
    }
}
