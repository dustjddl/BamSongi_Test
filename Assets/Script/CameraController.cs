using System.Collections;   // 코루틴 기능을 사용하기 위한 네임스페이스
using UnityEngine;
using Unity.Cinemachine;    // Cinemachine 기능을 사용하기 위한 네임스페이스

public class CameraController : MonoBehaviour
{
    public CinemachineCamera CinemachineCam;    //Cinemachine 카메라 참조함으로 줌 효과를 줄 대상을 지정함
    public float zoomInFOV = 10.0f;   // 줌인할 때의 FOV설정값
    public float zoomOutFOV = 60.0f;    // 기본 시야각(줌아웃)으로 되돌릴 때의 FOV 설정값
    public float zoomDuration = 4.0f;   // 줌인, 줌아웃 시 소요시간
    public float holdDuration = 3.0f;   // 줌인 상태 유지시간

    private bool isZooming = false;
    // 현재 줌 애니메이션이 진행 중인지 체크하는 플래그
    // 줌인, 줌아웃 시 추가로 발사되지 않게하기 위해 사용
    public bool IsZooming => isZooming;
    // 외부에서 isZooming 상태를 읽을 수 있게 하는 읽기 전용 프로퍼티
    // 외부에서는 isZooming의 값만 확인할 수 있고 isZooming가 false되는 것을 하지 못하게 하기 위해 사용
    private static CameraController _instance = null;
    public static CameraController Instance
    {
        get
        {
            if (_instance == null) Debug.Log("CameraController is null.");
            return _instance;
        }
    }
    public void StartZoomEffect()
    {
        if (!isZooming) //줌 상태일 때 반복 실행되지 않도록 방지
            StartCoroutine(ZoomRoutine());  // 코루틴으로 줌 효과 실행
    }

    private IEnumerator ZoomRoutine()
    {
        isZooming = true;   // 줌 상태 시작

        // 줌인
        yield return StartCoroutine(ChangeFOV(zoomOutFOV, zoomInFOV, zoomDuration));
        // 줌인 상태 유지
        yield return new WaitForSeconds(holdDuration);

        // 줌아웃
        yield return StartCoroutine(ChangeFOV(zoomInFOV, zoomOutFOV, zoomDuration));
        // 줌 상태 종료
        isZooming = false;
    }

    private IEnumerator ChangeFOV(float from, float to, float duration)  // FOV를 부드럽게 변화시키는 코루틴
    {
        // 경과 시간 초기화
        float time = 0f;
        // 전체 지속 시간 동안 반복
        while (time < duration)
        {
            float t = time / duration;
            // Lerp로 FOV 값을 선형 보간하여 부드럽게(자연스럽게) 변화
            // 예: from=60.0f, to=30.0f, t=1.0f라면 FOV가 1초 동안 60 → 30으로 자연스럽게 줄어듬
            CinemachineCam.Lens.FieldOfView = Mathf.Lerp(from, to, t);
            // 경과 시간 누적
            time += Time.deltaTime;
            yield return null;
        }
        // 최종적으로 FOV 값을 정확히 설정
        CinemachineCam.Lens.FieldOfView = to;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Debug.Log("CameraController has another instance.");
            Destroy(gameObject);
        }
    }
}

