using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    public static RadialProgress instance;

    bool startLoad = false;
    float currentValue = 0;
    public float speed;

    // Start is called before the first frame update
    void Start(){
        if (instance == null){
            instance = this;
        } else if (instance != this){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update(){
        if (startLoad){
            if (currentValue < 100){
                currentValue += speed * Time.deltaTime;
            }

            gameObject.GetComponent<Image>().fillAmount = currentValue / 100;

            if (currentValue >= 100){
                End();
            }
        }
    }

    public void Begin(float s){
        speed = s;
        startLoad = true;
    }

    public void End(){
        startLoad = false;
        currentValue = 0;
        gameObject.GetComponent<Image>().fillAmount = 0.0f;
    }
}
