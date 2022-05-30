using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIImageFileDataListObject : MonoBehaviour
{
    //refs
    [SerializeField] RawImage _imageRef;
    [SerializeField] TMP_Text _textRef;

    ImageFileData _data;

    public static UIImageFileDataListObject Create(ImageFileData data,Transform parent)
    {
        GameObject prefab = UIGameAssets.Instance.UI_Pfb_ImageFileDataListObject;
        Transform transf = Instantiate(prefab, parent).transform;
        UIImageFileDataListObject uIImageFileDataListObject = transf.GetComponent<UIImageFileDataListObject>();
        uIImageFileDataListObject.Setup(data);
        return uIImageFileDataListObject;
    }

    private void Setup(ImageFileData data)
    {
        _data = data;
        TimeSpan lifespan = (DateTime.Now - _data.creationTime);
        StringBuilder sb = new StringBuilder("Filename: ", 50);
        sb.Append(_data.fileName + Environment.NewLine);
        sb.Append("Created: " + lifespan.Days + " days, " + lifespan.Hours +" hours, "+ lifespan.Minutes + " minutes, " + lifespan.Seconds + " seconds ago.");
        _textRef.text = sb.ToString();
    }

    //Should be faster than texture.LoadImage();
    public IEnumerator LoadTextureCoroutine()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_data.filePath))
        {
            yield return uwr.SendWebRequest();
            _imageRef.texture = DownloadHandlerTexture.GetContent(uwr);
        }
    }

    private Texture2D CreateTexture2D(byte[] textureBytes)
    {
        var texture = new Texture2D(1, 1);
        texture.LoadImage(textureBytes);
        return texture;
    }
    private Sprite CreateSpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f,0,SpriteMeshType.FullRect);
    }
}
