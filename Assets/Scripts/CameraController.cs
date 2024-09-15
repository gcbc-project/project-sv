using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;    // 시네머신 가상 카메라 참조
    [SerializeField] float dragSpeed = 1f;                      // 드래그 속도
    [SerializeField] float zoomSpeed = 2f;                      // 줌 인/아웃 속도
    [SerializeField] float minZoom = 5f;                        // 최소 줌 값
    [SerializeField] float maxZoom = 15f;                       // 최대 줌 값

    private Vector2? previousMousePos;                          // 이전 프레임의 마우스 위치 (월드 좌표)
    private Transform cameraFollowTarget;                       // 카메라가 따라갈 빈 게임 오브젝트
    private CinemachineConfiner2D confiner;                     // Confiner2D 참조
    private Bounds confinerBounds;                              // Confiner의 경계
    private float cameraHalfWidth;                              // 카메라 뷰포트의 반너비
    private float cameraHalfHeight;                             // 카메라 뷰포트의 반높이

    void Start()
    {
        // 팔로우 대상 생성 및 설정
        GameObject followObject = new GameObject("CameraFollowTarget");
        cameraFollowTarget = followObject.transform;
        virtualCamera.Follow = cameraFollowTarget;

        // Confiner2D 컴포넌트 가져오기
        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();

        // 초기 카메라 뷰포트 크기 계산
        UpdateCameraSize();

        // Confiner의 경계 가져오기
        if (confiner != null && confiner.m_BoundingShape2D != null)
        {
            confinerBounds = confiner.m_BoundingShape2D.bounds;
        }
    }

    void Update()
    {
        bool isDragging = Mouse.current.rightButton.isPressed;

        if (isDragging)
        {
            // 현재 마우스 위치 (월드 좌표)
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if (previousMousePos == null)
            {
                previousMousePos = currentMousePos;
            }

            // 마우스 이동량 계산
            Vector2 delta = (Vector2)previousMousePos - currentMousePos;

            // 팔로우 대상 이동
            cameraFollowTarget.position += (Vector3)(delta * dragSpeed);

            // 팔로우 대상의 위치를 경계 내로 제한
            ClampFollowTargetPosition();

            // 이전 마우스 위치 업데이트
            previousMousePos = currentMousePos;
        }
        else
        {
            // 드래그가 끝나면 이전 마우스 위치 초기화
            previousMousePos = null;
        }

        // 줌 인/아웃 처리
        float scrollValue = Mouse.current.scroll.ReadValue().y;
        if (scrollValue != 0)
        {
            var lens = virtualCamera.m_Lens;
            lens.OrthographicSize -= scrollValue * zoomSpeed * Time.deltaTime;
            lens.OrthographicSize = Mathf.Clamp(lens.OrthographicSize, minZoom, maxZoom);
            virtualCamera.m_Lens = lens;

            // 줌 레벨 변경 시 Confiner 캐시 무효화
            if (confiner != null)
            {
                confiner.InvalidateCache();
            }

            // 줌 레벨 변경에 따른 카메라 뷰포트 크기 업데이트
            UpdateCameraSize();

            // 팔로우 대상의 위치를 경계 내로 제한
            ClampFollowTargetPosition();
        }
    }

    void UpdateCameraSize()
    {
        float orthographicSize = virtualCamera.m_Lens.OrthographicSize;
        float aspectRatio = Screen.width / (float)Screen.height;

        cameraHalfHeight = orthographicSize;
        cameraHalfWidth = orthographicSize * aspectRatio;
    }

    void ClampFollowTargetPosition()
    {
        if (confinerBounds != null)
        {
            float minX = confinerBounds.min.x + cameraHalfWidth;
            float maxX = confinerBounds.max.x - cameraHalfWidth;
            float minY = confinerBounds.min.y + cameraHalfHeight;
            float maxY = confinerBounds.max.y - cameraHalfHeight;

            Vector3 position = cameraFollowTarget.position;

            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);

            cameraFollowTarget.position = position;
        }
    }
}
