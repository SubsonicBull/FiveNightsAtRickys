using UnityEngine;

public class Walking : MonoBehaviour
{
    private Player player;

    [Header("Bobbing Settings")]
    [SerializeField] private float bobFrequency = 1.5f;
    [SerializeField] private float bobAmplitude = 0.05f;
    [SerializeField] private float speedToBobFactor = 0.2f;

    [Header("Step Sound Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] stepClips;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;
    [SerializeField] private float baseStepInterval = 0.5f; // Zeit bei normaler Laufgeschwindigkeit

    private Vector3 initialPosition;
    private float bobTimer = 0f;
    private float stepTimer = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        initialPosition = transform.localPosition;
    }

    private void Update()
    {


        Vector2 velocity = player.GetXZVelocity();

        //Dont do bobbing while player is locked
        if (player.GetIsLocked())
        {
            velocity = Vector2.zero;
        }

        float speed = velocity.magnitude;

        if (speed > 0.1f)
        {
            // Head bobbing
            bobTimer += Time.deltaTime * bobFrequency * speed * speedToBobFactor;
            float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
            transform.localPosition = initialPosition + new Vector3(0f, bobOffset, 0f);

            // Schritte
            stepTimer += Time.deltaTime;
            float adjustedInterval = baseStepInterval / Mathf.Max(speed, 0.1f);
            if (stepTimer >= adjustedInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            // Zurück zur Ausgangsposition
            bobTimer = 0f;
            stepTimer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * 5f);
        }
    }

    private void PlayFootstep()
    {
        if (audioSource != null && stepClips.Length > 0)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.PlayOneShot(stepClips[Random.Range(0, stepClips.Length)]);
        }
    }
}
