// создание объекта из конфигуратора и его передача в DragNDrop и Inventory
using UnityEngine;

namespace Inventory
{
    public class ItemToPickup : MonoBehaviour
    {
        #region User Interface
        [Tooltip("Выберете нужный Scriptable Object")]
        [SerializeField] private PickableItem _item; // SO из которого будет создаваться объект
        #endregion

        #region Private fields with Read-only properties
        private int _id; public int ID => _id;
        private string _name; public string Name => _name;
        private float _weight; public float Weight => _weight;
        private PickableItem.ObjectType _type; public PickableItem.ObjectType Type => _type;
        private bool _isPicked = false; // состояние предмета, нужно для остановки DragNDrop'a при снапе к инвентарю
        public bool IsPicked
        {
            get
            {
                return _isPicked;
            }
            set
            {
                _isPicked = value;
            }
        }
        #endregion

        #region Action Managers
        private DragNDrop _dragNDrop;
        private InventoryClass _inventory;
        #endregion

        #region Create Object
        private void OnEnable()
        {
            _dragNDrop = FindObjectOfType<DragNDrop>();
            _inventory = FindObjectOfType<InventoryClass>();

            // заполнение полей объекта из scriptable object'а
            // физические свойства можно было указать, исходя из свойств объекта, указанных в ТЗ

            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<CapsuleCollider>();
            gameObject.AddComponent<Rigidbody>();

            gameObject.GetComponent<MeshFilter>().mesh = _item.Model.GetComponent<MeshFilter>().sharedMesh;
            gameObject.GetComponent<MeshRenderer>().material = _item.Model.GetComponent<MeshRenderer>().sharedMaterial;

            gameObject.GetComponent<CapsuleCollider>().center = _item.Model.GetComponent<CapsuleCollider>().center;
            gameObject.GetComponent<CapsuleCollider>().radius = _item.Model.GetComponent<CapsuleCollider>().radius;
            gameObject.GetComponent<CapsuleCollider>().height = _item.Model.GetComponent<CapsuleCollider>().height;
            gameObject.GetComponent<CapsuleCollider>().direction = _item.Model.GetComponent<CapsuleCollider>().direction;
            gameObject.GetComponent<CapsuleCollider>().material = _item.Model.GetComponent<CapsuleCollider>().sharedMaterial;

            // gameObject.GetComponent<Rigidbody>().mass = _item.Model.GetComponent<Rigidbody>().mass; // на случай определения массы в модели
            gameObject.GetComponent<Rigidbody>().mass = _item.Weight;
            gameObject.GetComponent<Rigidbody>().drag = _item.Model.GetComponent<Rigidbody>().drag;
            gameObject.GetComponent<Rigidbody>().angularDrag = _item.Model.GetComponent<Rigidbody>().angularDrag;
            gameObject.GetComponent<Rigidbody>().useGravity = _item.Model.GetComponent<Rigidbody>().useGravity;
            gameObject.GetComponent<Rigidbody>().isKinematic = _item.Model.GetComponent<Rigidbody>().isKinematic;
            gameObject.GetComponent<Rigidbody>().interpolation = _item.Model.GetComponent<Rigidbody>().interpolation;
            gameObject.GetComponent<Rigidbody>().collisionDetectionMode = _item.Model.GetComponent<Rigidbody>().collisionDetectionMode;

            _id = _item.ID;
            _name = _item.Name;
            _weight = _item.Weight;
            _type = _item.Type;
        }
        #endregion

        #region Logic
        // назначение объекта предметом для Drag^Drop'а и объектом для поднятия/сброса при нажатии на нем
        // условия нужны для того, чтобы была возможность вынимать объекты не только через UI панель, но и Drag&Drop'ом модели
        private void OnMouseDown()
        {
            if (!_isPicked)
            {
                _inventory._ItemToPickup = gameObject;
            } else if (_isPicked)
            {
                _inventory._ItemToDrop = gameObject;
            }
            _dragNDrop.DragObject = gameObject;
        }

        // обнуление объектов для перетаскивания и поднятия/сброса при отжатии мыши
        private void OnMouseUp()
        {
            if (_isPicked)
            {
                _inventory._ItemToDrop = null;
            }
            else if (!_isPicked)
            {
                _inventory._ItemToPickup = null;
            }
            _dragNDrop.DragObject = null;
        }
        #endregion
    }
}
