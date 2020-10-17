using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemUIPanel : MonoBehaviour
    {
        #region User Interface
        [SerializeField] private PickableItem.ObjectType _type;
        [SerializeField] private GameObject ItemImage;
        #endregion

        #region Fields
        private Inventory.InventoryClass _inventory; // инвертарь
        private GameObject _slotObject; // ссылка на объект, размещаемый в слоте
        private int slotObjectIndex; // индекс переменной состояния слота
        #endregion

        #region Logic
        private void Start()
        {
            _inventory = FindObjectOfType<Inventory.InventoryClass>(); // поиск инвентаря
            switch (_type) // выбор объекта для отображения в панели исходя из ее типа - RedType с индексом 0, GreenType с 1 и BlueType с 2
            {
                case PickableItem.ObjectType.RedType:
                    slotObjectIndex = 0;
                    ItemImage.GetComponent<RawImage>().color = Color.red;
                    break;
                case PickableItem.ObjectType.GreenType:
                    slotObjectIndex = 1;
                    ItemImage.GetComponent<RawImage>().color = Color.green;
                    break;
                case PickableItem.ObjectType.BlueType:
                    slotObjectIndex = 2;
                    ItemImage.GetComponent<RawImage>().color = Color.blue;
                    break;
            }
        }

        // активация иконки объекта, если он в инвентаре
        private void Update()
        {
            _slotObject = _inventory.SlotObjects[slotObjectIndex];
            if (_slotObject != null)
            {
                ItemImage.SetActive(true);
            } else if (_slotObject == null)
            {
                ItemImage.SetActive(false);
            }
        }

        // назначение предмета в слоте объектом для выкидывания при наведении мыши
        private void OnMouseEnter()
        {
            _inventory._ItemToDrop = _slotObject;
        }

        // отмена назначения предмета в слоте объектом для выкидывания
        private void OnMouseExit()
        {
            _inventory._ItemToDrop = null;
        }
        #endregion
    }
}
