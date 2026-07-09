using System.Collections;
using UnityEngine;

public class MoveObjectOutcome : MonoBehaviour, IPuzzleOutcome
{
    [Header("Movement")]
    [SerializeField] private Transform objectToMove;
    [SerializeField] private Transform targetTransform;

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 180f;

    [SerializeField] private bool useLocalSpace = false;

    public void Execute()
    {
        if (objectToMove == null || targetTransform == null)
        {
            Debug.LogWarning($"{name}: Missing object references.");
            return;
        }

        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            if (useLocalSpace)
            {
                objectToMove.localPosition = Vector3.MoveTowards(objectToMove.localPosition, targetTransform.localPosition, moveSpeed * Time.deltaTime);
                objectToMove.localRotation = Quaternion.RotateTowards(objectToMove.localRotation, targetTransform.localRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                objectToMove.position = Vector3.MoveTowards(objectToMove.position, targetTransform.position, moveSpeed * Time.deltaTime);

                objectToMove.rotation = Quaternion.RotateTowards(objectToMove.rotation, targetTransform.rotation, rotationSpeed * Time.deltaTime);
            }

            bool reachedPosition = useLocalSpace ? Vector3.Distance(objectToMove.localPosition, targetTransform.localPosition) < 0.001f
                : Vector3.Distance(objectToMove.position, targetTransform.position) < 0.001f;

            bool reachedRotation = useLocalSpace ? Quaternion.Angle(objectToMove.localRotation, targetTransform.localRotation) < 0.1f
                : Quaternion.Angle(objectToMove.rotation, targetTransform.rotation) < 0.1f;

            if (reachedPosition && reachedRotation)
                yield break;

            yield return null;
        }
    }
}