using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float t_S = 0f;
    private bool lerpingSize = false;
    float initSize;
    float finalSize;
    float rate;
    float shakeDuration;
    Vector3 origPos;
    Vector3 startPos;
    Vector3 leftOffset;
    Vector3 rightOffset;
    bool doneShaking = true;
    public GameObject player = null;
    private string[] cont = { "WeaponMask" };
    public List<GameObject> maskObjects;
    private GameObject canvas;
    private float baseScale;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform.gameObject;
        baseScale = this.GetComponent<Camera>().orthographicSize;
    }

    // Use this for initialization
    public void Start ()
    {
        origPos = this.transform.position;
        maskObjects = new List<GameObject>();

        foreach (string mask in cont)
        {
            try
            {
                maskObjects.Add(GameObject.Find(mask));
            }
            catch
            {
                print("Could not find mask: " + mask);
            }
        }
        //print(this.GetComponent<Camera>().aspect);
        //if(this.GetComponent<Camera>().aspect > 1.9)
        //{
        //    Screen.SetResolution(1920, 1080, true);
        //}
        
    }

    public void OnPreCull()
    {
        foreach (GameObject mask in maskObjects)
        {
            if (mask != null)
                mask.GetComponent<SpriteMaskAnimator>().UpdateMasks();
        }
    }

    public List<GameObject> GetMaskObjects()
    {
        return maskObjects;
    }

    public float getSizeScaleOffset()
    {
        return this.GetComponent<Camera>().orthographicSize / baseScale;
    }

    public void ShakeCamera(int intensity = 1, bool leftToRight = true, float duration = 1.0f)
    {
        shakeDuration = duration;
        doneShaking = false;
        startPos = this.transform.position;

        if (leftToRight)
        {
            leftOffset = startPos - new Vector3(2 * intensity, 0, 0);
            rightOffset = startPos + new Vector3(2 * intensity, 0, 0);
            StartCoroutine(ShakeLeft(intensity, duration));
        }
    }

    IEnumerator ShakeLeft(int intensity, float duration)
    {
        this.GetComponent<LerpScript>().LerpToPos(rightOffset, leftOffset, intensity);
        yield return new WaitForSeconds(0.05f);
        if (!doneShaking)
            StartCoroutine(ShakeRight(intensity, duration));
        else
            this.GetComponent<LerpScript>().LerpToPos(leftOffset, startPos, intensity);
    }

    IEnumerator ShakeRight(int intensity, float duration)
    {
        this.GetComponent<LerpScript>().LerpToPos(leftOffset, rightOffset, intensity);
        yield return new WaitForSeconds(0.05f);
        if (!doneShaking)
            StartCoroutine(ShakeLeft(intensity, duration));
        else
            this.GetComponent<LerpScript>().LerpToPos(rightOffset, startPos, intensity);
    }

    // Update is called once per frame
    void Update ()
    {
        if (lerpingSize)
        {
            t_S += Time.deltaTime * rate;

            this.GetComponent<Camera>().orthographicSize = initSize + (t_S * (finalSize - initSize));
            
            for(int i = 0; i < canvas.transform.childCount; ++i)
            {
                Vector3 og = canvas.transform.GetChild(i).transform.position;
                canvas.transform.GetChild(i).transform.position =  new Vector3(og.x, og.y, 0);
            }

            if (t_S > 1)
            {
                t_S = 0;
                lerpingSize = false;

                for (int i = 0; i < canvas.transform.childCount; ++i)
                {
                    Vector3 og = canvas.transform.GetChild(i).transform.position;
                    canvas.transform.GetChild(i).transform.position = new Vector3(og.x, og.y, 0);
                }
            }
        }

        if(!doneShaking)
        {
            shakeDuration -= Time.deltaTime;

            if (shakeDuration < 0.0f)
                doneShaking = true;
        }
	}

    public void LerpCameraSize(float startSize, float endSize, float speed = 1f)
    {
        t_S = 0f;
        initSize = startSize;
        finalSize = endSize;
        rate = speed;
        lerpingSize = true;
    }
}
