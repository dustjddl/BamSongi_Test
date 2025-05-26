using UnityEngine;
using UnityEngine.TestTools;

public class TargetController : MonoBehaviour
{
    private float MoveSpeed = 2.0f; // 이동 속도
    private float MoveRange = 2.0f; // 이동 범위

    private Vector3 StartPosition;  // 타겟 시작 위치 저장을 위한 벡터
    void Start()
    {
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Mathf.PingPong() : 시간을 기반으로 일정 범위를 반복하는 함수
        // PingPong함수를 통해 과녁이 움직일 거리를 계산하고 offst 변수에 움직일 최종 위치를 저장한다
        float offset = Mathf.PingPong(Time.time * MoveSpeed, MoveRange) - (MoveRange / 2f); ;
        transform.position = StartPosition + new Vector3(offset, 0f, 0f);
    }

}
