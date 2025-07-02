using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Cài đặt camera")]
    [SerializeField] private float heightOffset = 5f; // Khoảng cách Y giữa camera và player
    [SerializeField] private float smoothSpeed = 0.125f; // Tốc độ mượt mà khi di chuyển
    [SerializeField]
    private List<Transform> players = new List<Transform>(); // Danh sách player
    private Transform currentTarget; // Player đang được follow
    private Vector3 initialPosition; // Vị trí ban đầu (giữ nguyên X và Z)

    void Start()
    {
        // Lưu trữ vị trí ban đầu của camera
        initialPosition = transform.position;

        // Nếu có player được thêm vào, chọn player đầu tiên làm mục tiêu
        if (players.Count > 0)
        {
            currentTarget = players[0];
        }
    }

    void LateUpdate()
    {
        if (currentTarget != null)
        {
            // Giữ nguyên X và Z, chỉ cập nhật Y dựa trên player
            Vector3 desiredPosition = new Vector3(
                initialPosition.x,
                currentTarget.position.y + heightOffset,
                initialPosition.z
            );

            // Di chuyển mượt mà đến vị trí mới
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
    }

    // Thêm player vào danh sách
    public void AddPlayer(Transform player)
    {
        if (player != null && !players.Contains(player))
        {
            players.Add(player);
            if (currentTarget == null) currentTarget = player; // Chọn player đầu tiên nếu chưa có mục tiêu
        }
    }

    // Chọn player để follow
    public void SetTarget(Transform target)
    {
        if (players.Contains(target))
        {
            currentTarget = target;
        }
    }

    // Trả về danh sách player (dùng cho UI)
    public List<Transform> GetPlayers()
    {
        return players;
    }
}