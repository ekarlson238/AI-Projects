using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceGame : MonoBehaviour
{
    public string inputValue = "1";
    public Text outputText;
    public InputField inputField;
    public Button button;

    int ThrowNormalDice()
    {
        Debug.Log("Throwing dice...");
        Debug.Log("Finding random between 1 to 6...");
        int diceResult = Random.Range(1, 7);
        Debug.Log("Result: " + diceResult);
        return diceResult;
    }
    int ThrowLoadedDice()
    {
        Debug.Log("Throwing dice...");
        int randomProbability = Random.Range(1, 101);
        int diceResult = 0;
        if (randomProbability < 36)
        {
            diceResult = 6;
        }
        else
        {
            diceResult = Random.Range(1, 5);
        }
        Debug.Log("Result: " + diceResult);
        return diceResult;
    }
    public void ProcessGame()
    {
        inputValue = inputField.text;
        try
        {
            int diceResult = 0;
            int inputInteger = int.Parse(inputValue);
            int totalSix = 0;

            for (var i = 0; i < 10; i++)
            {
                diceResult = ThrowLoadedDice();
                if (diceResult == 6) { totalSix++; }
            }

            if (diceResult == inputInteger)
            {
                outputText.text = "DICE RESULT: " +
               diceResult.ToString() + "\r\nYOU WIN!";
            }
            else
            {
                outputText.text = "DICE RESULT: " +
               diceResult.ToString() + "\r\nYOU LOSE!";
            }

            Debug.Log("Total of six: " + totalSix.ToString());
        }
        catch
        {
            outputText.text = "Input is not a number!";
            Debug.LogError("Input is not a number!");
        }
    }
}
