using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackgroundColorScript : MonoBehaviour
{
    Camera cam;  //Camera in which we attach in script

    [Header("Array of Colors")]
    public Color[] ColorsArray; //The colors we want to show in the background

    int CurrentColor; //Index of the current background color

    [Header("Time Parameters")]
    [Range(1.0f,10.0f)]
    public float TimeToChangeColors; //Time to change colors
    [Range(1.0f, 10.0f)]
    public float TransitionSpeed;  //Speed of the transition between two colors

    float CurrentTime; //Time that has passed in the transition from one color to another

    bool CanChange;  //To check if we can change the color not

    bool CanCountDown;  //If the timer can countdown

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        CurrentTime = TimeToChangeColors;
        //Assign the background color to the firt color of ColorsArray
        cam.backgroundColor = ColorsArray[0];

        //Starts the timer
        CanCountDown = true;
    }

    //Function to replace the background color with the one tha the funtion receives
    void ChangeColor(Color color)
    {
        cam.backgroundColor = color;
    }

    // Update is called once per frame
    void Update()
    {
        //Timer

        //Check if it is time to change the color
        if (CurrentTime <= 0.0f)
        {
            CurrentTime = TimeToChangeColors;
            CanChange = true;
        }

        //If it isn't, continue the countdown
        else if (CurrentTime >= 0.0f && CanCountDown)
        {
            CurrentTime -= Time.deltaTime;
            CanChange = false;
        }
        //It is call every frame, but it does not work until CanChange is equal to true
        StartCoroutine(ChangingColor());

        //Coroutine that allow us to change the background color
        IEnumerator ChangingColor()
        {

            if (CanChange == true)
            {
                //Stop the timer
                CanCountDown = false;

                Color colorA = cam.backgroundColor;
                Color colorB = ColorsArray[0];
                
                //Check if the next color is less than the number of elements of ColorsArray
                if (CurrentColor + 1 < ColorsArray.Length)
                {
                    //if true, the following color will be equal to CurrentColor plus 1
                    colorB = ColorsArray[CurrentColor + 1];
                }

                //Change the color
                Color NewColor = Color.Lerp(colorA, colorB, Time.deltaTime * TransitionSpeed);
                //Assign the color to the background
                ChangeColor(NewColor);

                
                if (NewColor == colorB)
                { 
                    CanChange = false;

                    //Pick the next color
                    if (CurrentColor < ColorsArray.Length)
                    {
                        CurrentColor++;
                    }
                    else
                    {
                        CurrentColor = 0;
                    }

                    //Start the timer again
                    CanCountDown = true;
                    yield return null;
                }
            }
        }
    }
}