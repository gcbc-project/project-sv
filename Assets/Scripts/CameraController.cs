using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;  // 시네머신 카메라 참조
    [SerializeField] float dragSpeed = 2f;
    [SerializeField] float zoomSpeed = 2f;          // 줌 인/아웃 속도
    [SerializeField] float minZoom = 5f;            // 최소 줌 값
    [SerializeField] float maxZoom = 15f;           // 최대 줌 값

    private Vector2 dragOrigin;                     // 드래그 시작 시점에서의 마우스 위치
    private Vector3 cameraOrigin;                   // 드래그 시작 시점에서의 카메라 위치
    private Vector3 cameraTargetPosition;           // LateUpdate에서 적용할 카메라 위치

    private Transform cameraFollowTarget;           // 카메라가 따라갈 빈 오브젝트

    void Start()
    {
        // 카메라가 따라갈 빈 오브젝트를 생성하여 시네머신 카메라에 할당
        GameObject followObject = new GameObject("CameraFollowTarget");
        cameraFollowTarget = followObject.transform;
        virtualCamera.Follow = cameraFollowTarget;
        cameraTargetPosition = cameraFollowTarget.position; // 초기값 설정
    }

    void Update()
    {
        // 마우스 우클릭 중일 때 드래그 시작
        if (Mouse.current.rightButton.isPressed)
        {
            // 드래그 시작 시점에서 마우스 위치와 카메라 위치 기록
            if (dragOrigin == Vector2.zero)
            {
                dragOrigin = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                cameraOrigin = cameraFollowTarget.position;
            }

            // 현재 마우스 위치
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // 마우스 드래그 시작 위치와 현재 위치의 차이를 계산
            Vector3 difference = dragOrigin - currentMousePos;

            // 드래그 시작 시점의 카메라 위치에서 차이값을 적용하여 목표 위치 설정
            cameraTargetPosition = cameraOrigin + (Vector3)difference * dragSpeed;
        }
        else
        {
            // 드래그가 끝나면 시작 좌표 초기화
            dragOrigin = Vector2.zero;
        }

        // 스크롤 입력에 따라 줌 인/아웃 처리
        float scrollValue = Mouse.current.scroll.ReadValue().y;
        if (scrollValue != 0)
        {
            var lens = virtualCamera.m_Lens;
            lens.OrthographicSize -= scrollValue * zoomSpeed * Time.deltaTime;
            lens.OrthographicSize = Mathf.Clamp(lens.OrthographicSize, minZoom, maxZoom);
            virtualCamera.m_Lens = lens;
        }
    }

    void LateUpdate()
    {
        // LateUpdate에서 카메라의 위치 이동 적용
        cameraFollowTarget.position = cameraTargetPosition;
    }
}
