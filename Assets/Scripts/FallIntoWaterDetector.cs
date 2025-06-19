//using UnityEngine;
//using UnityEngine.Tilemaps;

//public class FallIntoWaterDetector : MonoBehaviour
//{
//    public TileBase dangerTile;  // Drag your Map_91.asset (water tile) here

//    void Update()
//    {
//        // Get position slightly below the player (feet)
//        Vector3 checkPos = transform.position + Vector3.down * 0.1f;

//        // Find all Tilemaps currently active in the scene
//        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();

//        foreach (Tilemap tilemap in tilemaps)
//        {
//            Vector3Int cellPos = tilemap.WorldToCell(checkPos);
//            TileBase tile = tilemap.GetTile(cellPos);

//            if (tile == dangerTile)
//            {
//                Debug.Log("Player fell into water → Game Over");
//                GameManager.instance.GameOver();
//                return;
//            }
//        }
//    }
//}
