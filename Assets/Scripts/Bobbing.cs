using System;
using Unity.Mathematics;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] float amount = 0.002f;
    [SerializeField] float frq = 10.0f;
    [SerializeField] float smooth = 10.0f;
    private float bob;
    private float bob2;
    private float counter;

    void Update()
    {
        if (!PauseMenu.gamePaused)
        {
            FakeUpdate();
        }
    }

    void FakeUpdate()
    {
        counter += Time.deltaTime;

        bob = Mathf.Sin(counter * frq) * amount;
        bob2 = Mathf.Sin(counter * frq/2) * amount;

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            transform.localPosition += new Vector3(bob2, bob, 0);
        }
    }
}
