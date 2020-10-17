using Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUIPanel : MonoBehaviour
    {
        #region User Interface
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _backpack;
        [SerializeField] private RectTransform _inventoryPanel;
        [SerializeField] private Text _weightText;
        #endregion

        #region Logic
        // расположение панели на канвасе, если он активен
        private void Update()
        {
            if (gameObject.activeSelf)
            {
                PositionPanel();
            } else if (!gameObject.activeSelf) {
                return;
            }
        }

        // определение позиции панели в зависимости от положения инвенторя на экране
        private void PositionPanel()
        {
            Vector3 backpackScreenPos = _camera.WorldToScreenPoint(_backpack.position);
            _inventoryPanel.anchoredPosition = backpackScreenPos;
        }
        #endregion
    }
}
