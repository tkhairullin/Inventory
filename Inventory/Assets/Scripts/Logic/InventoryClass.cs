using UnityEngine;
using UnityEngine.Events;

namespace Inventory
{
    public class InventoryClass : MonoBehaviour
    {
        #region User Interface
        [Header("Settings")]

        [Tooltip("Канвас, содержащий GUI инвентаря")]
        [SerializeField] private GameObject _inventCanvas;

        [Tooltip("Позиции на объекте, к которым будут крепиться поднятые предметы")]
        [SerializeField] private Transform[] ItemSlot;

        [Space]
        [Header("Network")]
        [SerializeField] private string _postURL;
        [SerializeField] private string _postAPIKey;
        [SerializeField] private Network.PostRequest PostSender;

        [Space]
        [Header("Events")]
        public UnityEvent OnItemPickup;
        public UnityEvent OnItemDrop;
        #endregion

        // сохранение состояний слотов, для реального проекта следует использовать PlayerPrefs
        private GameObject[] _slotObjects; public GameObject[] SlotObjects => _slotObjects;

        #region Items to pick/drop
        private GameObject _itemToPickup; // поднимаемый объект
        public GameObject _ItemToPickup
        {
            get
            {
                return _itemToPickup;
            }
            set
            {
                _itemToPickup = value;
            }
        }

        private GameObject _itemToDrop; // сбрасываемый объект
        public GameObject _ItemToDrop
        {
            get
            {
                return _itemToDrop;
            }
            set
            {
                _itemToDrop = value;
            }
        }
        #endregion

        #region Logic
        // инициализация
        private void OnEnable()
        {
            _inventCanvas.SetActive(false);
            _itemToPickup = null;
            _slotObjects = new GameObject[] { null, null, null };
        }

        // активация UI принажатии на инвентарь
        private void OnMouseDown()
        {
            _inventCanvas.SetActive(true);
        }

        // даективация при отжатии кнопки мыши и сборс предмета, если он определен
        private void OnMouseUp()
        {
            _inventCanvas.SetActive(false);
            if (_itemToDrop != null)
            {
                DropItem();
            }
            else if (_itemToDrop == null)
            {
                return;
            }
        }

        // сбор предмета, если он определен и UI не активен. Нужно для вытаскивания предмета Drag&Drop'ом
        private void Update()
        {
            if (_itemToDrop != null && !_inventCanvas.activeSelf)
            {
                DropItem();
            }
            else if (_itemToDrop == null)
            {
                return;
            }
        }

        // поднятие предмета при наведении мыши на инвентарь, если предмет определен
        private void OnMouseOver()
        {
            if (_itemToPickup != null)
            {
                PickupItem();
            }
            else if (_itemToPickup == null)
            {
                return;
            }
        }


        private void PickupItem()
        {
            _itemToPickup.GetComponent<Rigidbody>().isKinematic = true; // Объект не должен падать
            PickableItem.ObjectType objectType = _itemToPickup.GetComponent<ItemToPickup>().Type; // определение типа подбираемого объекта
            switch (objectType) // размещение объекта в инвентаре исходя из типа - RedType с индексом 0, GreenType с 1 и BlueType с 2
            {
                case PickableItem.ObjectType.RedType:
                    _itemToPickup.transform.position = ItemSlot[0].position;
                    _slotObjects[0] = _itemToPickup;
                    break;
                case PickableItem.ObjectType.GreenType:
                    _itemToPickup.transform.position = ItemSlot[1].position;
                    _slotObjects[1] = _itemToPickup;
                    break;
                case PickableItem.ObjectType.BlueType:
                    _itemToPickup.transform.position = ItemSlot[2].position;
                    _slotObjects[2] = _itemToPickup;
                    break;
            }
            _itemToPickup.GetComponent<ItemToPickup>().IsPicked = true; // смена статуса объекта на "Подобран"
            
            OnItemPickup.Invoke();
            PostSender.SendPostRequest(_postURL, _postAPIKey, _itemToPickup.GetComponent<ItemToPickup>().ID, "Item has been picked up!");

            _itemToPickup = null;
        }

        private void DropItem()
        {
            _itemToDrop.GetComponent<Rigidbody>().isKinematic = false; // возврат физических свойств
            PickableItem.ObjectType objectType = _itemToDrop.GetComponent<ItemToPickup>().Type; // определение типа сбрасываемого объекта
            switch (objectType) // освобождение слота в инвентаре исходя из типа сбрасываемого объекта
            {
                case PickableItem.ObjectType.RedType:
                    _slotObjects[0] = null;
                    break;
                case PickableItem.ObjectType.GreenType:
                    _slotObjects[1] = null;
                    break;
                case PickableItem.ObjectType.BlueType:
                    _slotObjects[2] = null;
                    break;
            }
            _itemToDrop.GetComponent<ItemToPickup>().IsPicked = false; // смена статуса объекта на "не подобран"
            
            OnItemDrop.Invoke();
            PostSender.SendPostRequest(_postURL, _postAPIKey, _itemToDrop.GetComponent<ItemToPickup>().ID, "Item has been dropped!");

            _itemToDrop = null;
        }
        #endregion
    }
}
