using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int number1, number2, result, score, addScore;
    public Button buttonPrefab;
    public Transform[] positions;
    [SerializeField] private Transform SpawnButtonsPanel;
    [SerializeField] private TextMeshProUGUI number1Text, number2Text, resultText, scoreText,endScore;
    public AudioSource correctAnswer, wrongAnswer;
    [SerializeField] private GameObject gameUI,finishUI;
    private int heartCount = 3;
    [SerializeField] private Image[] hearts;
    private PlayFabLogin playFabLogin;
    private void Start()
    {
        playFabLogin = FindObjectOfType<PlayFabLogin>();
        SpawnNewButtons();
      
        scoreText.text = " Score " + score.ToString();
    }
    private void Update()
    {
        endScore.text = "Final Score " + score.ToString();
        if (heartCount ==0)
        {
            gameUI.SetActive(false);
            finishUI.SetActive(true);
            SpawnButtonsPanel.gameObject.SetActive(false);
            playFabLogin.SendLeaderBoard(score);
        }
       
    }

    #region Buttons
    private void SpawnNewButtons()
    {
        //we gemerate a random number and we calculate the correct answer
        number1 = Random.Range(2, 20);
        number2 = Random.Range(2, 20);
        result = number1 + number2;
        int correctButtonIndex = Random.Range(0, 4); 

       
        //we instatiate the buttons at the specific positioons of the positions array
        for (int i = 0; i < positions.Length; i++)
        {
            Button newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(SpawnButtonsPanel);
            newButton.transform.position = positions[i].position;
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

            if (i == correctButtonIndex)
            {
                buttonText.text = result.ToString();

            }
            else
            {
                //we generate random values for the wrong buttons
                int randomValueNumber = Random.Range(result - Random.Range(2,5), result+Random.Range(6, 11));
               if(randomValueNumber == result) 
                {
                    randomValueNumber =Random.Range(result - Random.Range(2, 5),result +Random.Range(6, 11));
                }
                buttonText.text = randomValueNumber.ToString(); 
            }
            //we add our buttons an event listener 
            newButton.onClick.AddListener(() => Guess(newButton)); 

      
        }
        //we update the ui text to show the numbers and the result
        number1Text.text = number1.ToString() + " + ";
        number2Text.text = number2.ToString() + " = ";
        resultText.text = "? ";
    }

    public void Guess(Button guessButton)
    {
        //we check if the clicked button's text matches the correct result
        if (guessButton.GetComponentInChildren<TextMeshProUGUI>().text == result.ToString())
        {
            
            StartCoroutine(ShowResultAndSpawnNewButtons());
        }
        else
        {
            //if the guess is wrong we play the wrong sound and we lose a heart
            wrongAnswer.Play();
            heartCount--;
            hearts[heartCount].gameObject.SetActive(false);
        }
         
        }

    //we update the score we play the correct answer sound, we display result and spawn the new buttons after 2 seconds
    private IEnumerator ShowResultAndSpawnNewButtons()
    {
        score += addScore;
        scoreText.text = "Score " + score.ToString();
        correctAnswer.Play();
        resultText.text = result.ToString();
        yield return new WaitForSeconds(2f);
        SpawnNewButtons();
    }
 
    #endregion Buttons

    //funnction that restarts the game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}