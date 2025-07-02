using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance { get; private set; }

    [Header("Background Settings")]
    private GameObject currentBackground;
    private Camera mainCamera;
    private Vector3 cameraOffset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Không tìm thấy Main Camera!");
            return;
        }

        // Tạo background từ GameData nếu có
        CreateBackgroundFromGameData();
    }

    private void Update()
    {
        // Cập nhật vị trí background theo camera
        if (currentBackground != null && mainCamera != null)
        {
            Vector3 targetPosition = mainCamera.transform.position + cameraOffset;
            targetPosition.z = currentBackground.transform.position.z; // Giữ nguyên z
            currentBackground.transform.position = targetPosition;
        }
    }

    public void CreateBackgroundFromGameData()
    {
        if (GameData.Instance == null || GameData.Instance.selectedBackgroundPrefab == null)
        {
            Debug.LogWarning("Không có background data để tạo!");
            return;
        }

        // Destroy background cũ nếu có
        if (currentBackground != null)
        {
            Debug.Log("xóa ");
            Destroy(currentBackground);
        }

        // Tạo background mới
        currentBackground = Instantiate(GameData.Instance.selectedBackgroundPrefab);

        if (mainCamera != null)
        {
            // Đặt background ở vị trí camera với z âm hơn để ở phía sau
            Vector3 backgroundPos = mainCamera.transform.position;
            backgroundPos.z = 10f; // Đặt background ở phía sau camera
            currentBackground.transform.position = backgroundPos;

            // Tính toán offset để background luôn cố định với camera
            cameraOffset = currentBackground.transform.position - mainCamera.transform.position;

            // Scale background để phù hợp với camera view (tùy chọn)
            ScaleBackgroundToFitCamera();
        }

        Debug.Log($"Đã tạo background: {GameData.Instance.selectedBackgroundPrefab.name}");
    }

    private void ScaleBackgroundToFitCamera()
    {
        if (currentBackground == null || mainCamera == null) return;

        // Lấy renderer của background
        Renderer bgRenderer = currentBackground.GetComponent<Renderer>();
        if (bgRenderer == null)
        {
            bgRenderer = currentBackground.GetComponentInChildren<Renderer>();
        }

        if (bgRenderer != null)
        {
            // Tính toán kích thước camera view
            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            // Tính toán scale để background cover toàn bộ camera view
            Vector3 bgSize = bgRenderer.bounds.size;
            float scaleX = cameraWidth / bgSize.x;
            float scaleY = cameraHeight / bgSize.y;

            // Sử dụng scale lớn hơn để đảm bảo background cover hết
            float finalScale = Mathf.Max(scaleX, scaleY) * 1.1f; // Thêm 10% để chắc chắn

            currentBackground.transform.localScale = Vector3.one * finalScale;
        }
    }

    // Method để thay đổi background trong runtime (nếu cần)
    public void ChangeBackground(GameObject newBackgroundPrefab)
    {
        if (newBackgroundPrefab == null) return;

        // Destroy background cũ
        if (currentBackground != null)
        {
            Destroy(currentBackground);
        }

        // Tạo background mới
        currentBackground = Instantiate(newBackgroundPrefab);

        if (mainCamera != null)
        {
            Vector3 backgroundPos = mainCamera.transform.position;
            backgroundPos.z = 10f;
            currentBackground.transform.position = backgroundPos;
            cameraOffset = currentBackground.transform.position - mainCamera.transform.position;
            ScaleBackgroundToFitCamera();
        }
    }

    private void OnDestroy()
    {
        if (currentBackground != null)
        {
            Destroy(currentBackground);
        }
    }
}