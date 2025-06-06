using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class FrontBorderController : MonoBehaviour {
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    public float borderZOffset = 35f;
    public float dangerDistance = 10f;
    public float maxDistance = 20f;
    public Color safeColor = Color.white;
    public Color dangerColor = Color.red;

    private Renderer rend;
    private Transform cam;
    private GameObject[] _gameObjects;

    private void Start() {
        _gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        rend = GetComponent<Renderer>();
        cam = Camera.main.transform;
    }

    private void Update() {
        float nearestEnemyZ = float.MinValue;
        foreach (var enemy in _gameObjects) {
            if (!enemy.activeInHierarchy) continue;

            float z = enemy.transform.position.z;
            if (z > nearestEnemyZ) {
                nearestEnemyZ = z;
            }
        }

        float distance = (cam.position.z + borderZOffset) - nearestEnemyZ;

        float t = Mathf.InverseLerp(maxDistance, dangerDistance, distance);
        Color finalColor = Color.Lerp(safeColor, dangerColor, t);

        if (rend.material.HasProperty(EmissionColor))
            rend.material.SetColor(EmissionColor, finalColor);
        else
            rend.material.color = finalColor;
    }
}