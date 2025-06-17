using UnityEngine;

public class TargetMover : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveDistance = 5f;
    [SerializeField] float moveSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        if (target == null)
            target = transform;

        startPos = target.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * (moveDistance / 2f);
        target.position = startPos + new Vector3(0, offset, 0);
    }
}
