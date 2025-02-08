using UnityEngine;

public class Linear : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 endPosition;

    [SerializeField] Vector3 movementVector;
    [SerializeField] float speed;
    float movementFactor;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }

    void Update()
    {
        movementFactor += Time.deltaTime * speed;

        if (movementFactor >= 1f)
        {
            transform.position = startPosition;
            movementFactor = 0f;
            return;
        }

        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
