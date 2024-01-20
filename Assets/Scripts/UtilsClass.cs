using UnityEngine;

public class UtilsClass
{
    public static Vector2 GetRandomDirection() => new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;

    public static TMPro.TextMeshPro CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), float fontSize = 40, Color color = default(Color), TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0)
    {
        GameObject gameObject = new GameObject("World_text", typeof(TMPro.TextMeshPro));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TMPro.TextMeshPro textMesh = gameObject.GetComponent<TMPro.TextMeshPro>();
        //textMesh.anchor = textAnchor;
        textMesh.alignment = TMPro.TextAlignmentOptions.Center;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        // textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPos;
    }
}
