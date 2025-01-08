using UnityEngine;
using UnityEngine.UIElements;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followDampen = 6f;

    private void Update()
    {
        if (target == null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 0f, transform.position.z), Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, target.position.y / followDampen, transform.position.z);
        }
    }
}