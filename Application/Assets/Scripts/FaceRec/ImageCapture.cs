using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.XR.WSA.WebCam;

using UnityEngine.UI;
using TMPro;

public class ImageCapture : MonoBehaviour
{
    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static ImageCapture instance;

    /// <summary>
    /// Keeps track of tapCounts to name the captured images 
    /// </summary>
    private int tapsCount = 0;

    /// <summary>
    /// PhotoCapture object used to capture images on HoloLens 
    /// </summary>
    private PhotoCapture photoCaptureObject = null;

    /// <summary>
    /// HoloLens class to capture user gestures
    /// </summary>
    private GestureRecognizer recognizer;


    /// <summary>
    /// Initialises this class
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Called right after Awake
    /// </summary>
    void Start()
    {
        //comment this out when the voice commands work

        // Initialises user gestures capture 
        /*recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.Tapped += TapHandler;
        recognizer.StartCapturingGestures();*/

        Debug.Log("started image capture");
    }

    /// <summary>
    /// Respond to Tap Input.
    /// </summary>
    private void TapHandler(TappedEventArgs obj)
    {
        TakePicture();
    }

    //this method will be invoked by a voice command, it will replace the TapHandler
    public void TakePicture(){
        //only take a picture if a face has not been recognized yet
        if (FaceRecName.instance.recName == ""){
            tapsCount++;
            Debug.Log(tapsCount);
            FaceRecName.instance.displayText.text = "Image capturing\ninitiated";
            ExecuteImageCaptureAndAnalysis();
        }
    }

    /// <summary>
    /// Begin process of Image Capturing and send To Azure Computer Vision service.
    /// </summary>
    private void ExecuteImageCaptureAndAnalysis()
    {
        //Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending
        //    ((res) => res.width * res.height).First();
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.ElementAt(1);

        Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        //tempText.text = "W: " + cameraResolution.width + " | H: " + cameraResolution.height;

        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;

            CameraParameters c = new CameraParameters();
            c.hologramOpacity = 0.0f;
            c.cameraResolutionWidth = targetTexture.width;
            c.cameraResolutionHeight = targetTexture.height;
            c.pixelFormat = CapturePixelFormat.BGRA32;

            Debug.Log("before start photoasync");

            captureObject.StartPhotoModeAsync(c, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                string filename = string.Format(@"CapturedImages{0}.jpg", tapsCount);
                string filePath = Path.Combine(Application.persistentDataPath, filename);

                //tempText.GetComponent<TextMeshPro>().text = Application.persistentDataPath;

                // Set the image path on the FaceAnalysis class
                FaceAnalysis.Instance.imagePath = filePath;

                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
            });
        });
    }

    /// <summary>
    /// Called right after the photo capture process has concluded
    /// </summary>
    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            FaceRecName.instance.displayText.text = "Picture taken";
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        
    }


    /// <summary>
    /// Register the full execution of the Photo Capture. If successfull, it will begin the Image Analysis process.
    /// </summary>
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;

        // Request image caputer analysis
        StartCoroutine(FaceAnalysis.Instance.DetectFacesFromImage());
    }
}
