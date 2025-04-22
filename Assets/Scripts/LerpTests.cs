using UnityEngine;

public class LerpTests : MonoBehaviour
{
    public GameObject g;
    public Vector3 start =  new Vector3(10,10,10);
    float x = 0;

    private void Update()
    {
        x += Time.deltaTime;
        g.transform.position = Vector3.Lerp(start, start + new Vector3(0, 2, 2), 0.5f*Mathf.Sin(x * 2) + 0.5f);
    }
}
