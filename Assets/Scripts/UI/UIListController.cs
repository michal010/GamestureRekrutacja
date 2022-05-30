using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;


public class UIListController : MonoBehaviour
{
    [SerializeField] TMP_InputField _pathInputField;
    private List<UIImageFileDataListObject> _listObjects;
    ImageLoader imgLoader;

    [SerializeField] RectTransform _container;
    [SerializeField] Button _refreshButton;

    private void Awake()
    {
        imgLoader = new ImageLoader();
        _listObjects = new List<UIImageFileDataListObject>();
        _refreshButton.onClick.AddListener(Refresh);
    }

    public void Refresh()
    {
        RefreshAsync();
    }
    private async void RefreshAsync()
    {
        _refreshButton.interactable = false;
        ClearListObjects();
        await LoadObjectsAsync(_pathInputField.text);
       
    }

    async Task LoadObjectsAsync(string path)
    {
        List<ImageFileData> data = await imgLoader.GetFilesAsync(path);
        CreateListObjects(data);
    }

    void CreateListObjects(List<ImageFileData> data)
    {
        foreach (var d in data)
        {
            _listObjects.Add(CreateUIImageFileDataListObject(d));
        }
            StartCoroutine(LoadImagesOneByOne());
    }

    void ClearListObjects()
    {
        foreach (var el in _listObjects)
        {
            Destroy(el.gameObject);
        }
        _listObjects.Clear();
    }

    IEnumerator LoadImagesOneByOne()
    {
        foreach (var el in _listObjects)
        {
            yield return StartCoroutine(el.LoadTextureCoroutine());
        }
        _refreshButton.interactable = true;
    }

    UIImageFileDataListObject CreateUIImageFileDataListObject(ImageFileData data)
    {
        return UIImageFileDataListObject.Create(data, _container);
    }

}
