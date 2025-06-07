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

    private void Start() {
        rend = GetComponent<Renderer>();
        cam = Camera.main.transform;
    }

    private void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y, cam.position.z + borderZOffset);

        float nearestEnemyZ = float.MinValue;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            if (!enemy.activeInHierarchy) continue;
            float z = enemy.transform.position.z;
            if (z > nearestEnemyZ) {
                nearestEnemyZ = z;
            }
        }

        float borderZ = cam.position.z + borderZOffset;
        float distance = nearestEnemyZ - borderZ;

        float t = Mathf.InverseLerp(dangerDistance, maxDistance, distance);
        Color finalColor = Color.Lerp(dangerColor, safeColor, t);

        if (rend.material.HasProperty("_EmissionColor"))
            rend.material.SetColor("_EmissionColor", finalColor);
        else
            rend.material.color = finalColor;
    }

}