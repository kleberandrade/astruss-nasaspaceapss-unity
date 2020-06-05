using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class LocalCameraManager : MonoBehaviour
{
    private WebCamTexture _webCamTexture = null;
    private WebCamDevice[] _webCamDevices;
    private Renderer _renderer;
 
    IEnumerator Start ()
    {
        if(!Permission.HasUserAuthorizedPermission(Permission.Camera))
            Permission.RequestUserPermission(Permission.Camera);
        
        while(!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return null;
        }

        _renderer = GetComponent<Renderer>();
        _webCamDevices = WebCamTexture.devices;
        _webCamDevices.ToList().ForEach(webCamDevice =>
        {
            if (webCamDevice.isFrontFacing)
            {
                _webCamTexture = new WebCamTexture(webCamDevice.name);
            }
        });
        _renderer.material.mainTexture = _webCamTexture;
        _webCamTexture.Play();
    }
}
