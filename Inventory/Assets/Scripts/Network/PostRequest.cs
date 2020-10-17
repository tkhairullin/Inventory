using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public class PostRequest : MonoBehaviour
    {
        public void SendPostRequest(string _url, string APIKey, int ID, string InventoryEvent)
        {
            StartCoroutine(UploadData(_url, APIKey, ID, InventoryEvent));
        }

        private IEnumerator UploadData(string _url, string APIKey, int ID, string InventoryEvent)
        {
            // создание и заполнение формы для отправки
            WWWForm form = new WWWForm();
            form.AddField("Object ID", ID);
            form.AddField("Event", InventoryEvent);

            // создание и отправка запроса с ключом авторизации
            UnityWebRequest request = UnityWebRequest.Post(_url, form);
            request.SetRequestHeader("Authorization", APIKey);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(ID+InventoryEvent);
            }
        }
    }
}
