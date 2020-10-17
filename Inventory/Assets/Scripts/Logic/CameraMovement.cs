using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    #region User Interface
    [Tooltip("Точка фокуса камеры")] [SerializeField] private Transform target;
    [Tooltip("Смещение от точки фокуса")] [SerializeField] private Vector3 targetOffset;
    [Tooltip("Расстояние до мнимой точки фокуса")] [SerializeField] private float distance = 5.0f;
    [Tooltip("Максимально расстояние до точки фокуса")] [SerializeField] private float maxDistance = 20;
    [Tooltip("Минимальное расстояние до точки фокуса")] [SerializeField] private float minDistance = .6f;
    [Tooltip("Скорость перемещения камеры по оси Y")] [SerializeField] private float xSpeed = 200.0f;
    [Tooltip("Скорость перемещения камеры по оси X")] [SerializeField] private float ySpeed = 200.0f;
    [Tooltip("Минимальный угол наклона камеры")] [SerializeField] private int yMinLimit = 10;
    [Tooltip("Максимальный угол наклона камеры")] [SerializeField] private int yMaxLimit = 80;
    [Tooltip("Коэффициент зума камеры")] [SerializeField] private int zoomRate = 40;
    [Tooltip("Коэффициент поворота камеры")] [SerializeField] private float panSpeed = 0.3f;
    [Tooltip("Коэффициент затухания перемещения и поворота камеры")] [SerializeField] private float zoomDampening = 5.0f;
    #endregion

    #region Fields
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    #endregion

    #region Logic
    void Start() { Init(); }
    void OnEnable() { Init(); }

    public void Init()
    {
        // создание мнимой точки фокуса, если она не определена
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }

        // присвоение переменным начальных значений
        currentDistance = distance;
        desiredDistance = distance;
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }
    
    // выполнение логики камеры после других скриптов
    void LateUpdate()
    {
        // поворот камеры правой кнопкой мыши
        if (Input.GetMouseButton(1))
        {
            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            // ограничение угла поворота по оси Y
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

            // задание углов поворота камеры
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;

            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
            transform.rotation = rotation;
        }
        // перемещение камеры средней кнопкой мыши
        else if (Input.GetMouseButton(2))
        {
            // захват поворота камеры для смещения
            target.rotation = transform.rotation;

            target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
            target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
        }

        // зум на колесо
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        // ограничение дальности
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        // вычисление промежуточных значений
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

        // вычисление позиции
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }

    // ограничение угла наклона
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    #endregion
}