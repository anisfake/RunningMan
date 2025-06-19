using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Material material;
    [SerializeField]
    private float parallaxFactor = 0.01f;
    private float offset;
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        ParralaxScroll();
    }
    private void ParralaxScroll()
    {
        float speed = GameManager.instance.GetGameSpeed() * parallaxFactor;
        offset += Time.deltaTime * speed;
        material.SetTextureOffset("_MainTex", Vector2.right * offset);
    }
}
