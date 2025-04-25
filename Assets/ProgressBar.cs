using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private RectTransform rect;
    float scaleFac = 1f;
    float initialXScale;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        initialXScale = rect.localScale.x;
    }
    private void Update()
    {
        rect.localScale = new Vector3(initialXScale * scaleFac, rect.localScale.y, rect.localScale.z);
    }

    //Sets the progressbar to a certain size x (value between 1 and 0)
    public void SetProgress(float x)
    {
        scaleFac = x;
    }
}
