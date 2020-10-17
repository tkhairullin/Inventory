// данная реализация основана на рэйкасте от позиции курсора к поверхности объектов, относящихся к слою PlaceableAre,
// что позволяет привязать Drag&Drop не к координатам на экране, а к виртуальному пространству;
// при перемещении объект поднимается на некоторую высоту над поверхностью;
// под перетаскиваемым объектом предусмотрена тень для ориентирования.

using UnityEngine;

namespace Inventory
{
    public class DragNDrop : MonoBehaviour
    {
        #region User Interface
        [Tooltip("Главная камера сцены")]
        [SerializeField] private Camera _camera; // кэш камеры

        [Tooltip("Спрайт тени объекта")]
        [SerializeField] private GameObject _dragShadow; // тень перемещаемого объекта, падающая вертикально вниз;

        [Tooltip("Высота подъема объекта при Drag&Drop'е")]
        [SerializeField] private float howerHeight = 0.5f; // высота, на которую поднимается объект

        [Tooltip("Слой объектов, над которыми можно перемещать объект")]
        [SerializeField] private LayerMask HitLayers; // маска слоя, на котором будут располагаться объекты, над которыми возможно перемещение
        #endregion

        #region DragObject
        private GameObject _dragObject; // перемещаемый объект
        public GameObject DragObject
        {
            get
            {
                return _dragObject;
            }
            set
            {
                _dragObject = value;
            }
        }
        #endregion

        #region Logic
        private void Start()
        {
            _dragShadow.SetActive(false);
        }

        // Вызов метода, если объект задан и не поднят
        private void Update()
        {
            if (_dragObject != null && !_dragObject.GetComponent<ItemToPickup>().IsPicked) 
            {
                MoveToMousePosition();
            }
            else if (_dragObject == null | (_dragObject != null && _dragObject.GetComponent<ItemToPickup>().IsPicked))
            {
                _dragShadow.SetActive(false);
            }
        }

        private void MoveToMousePosition()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, HitLayers)) // перемещение объекта в точку падения луча на поверхность, относящуюся к слою
            {
                // падение тени
                _dragShadow.SetActive(true);
                _dragShadow.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.01f, hitInfo.point.z);

                //перемещение объекта в точку над тенью
                _dragObject.transform.position = new Vector3(_dragShadow.transform.position.x, _dragShadow.transform.position.y + howerHeight, _dragShadow.transform.position.z);
            }
        }
        #endregion
    }
}
