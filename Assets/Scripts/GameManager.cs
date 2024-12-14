using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ball;
    Rigidbody rb;
    public Text scoreUI;
    [SerializeField]
    float force = 500f; // Adjusted default force value
    int score = 0;
    int turnCounter = 0; // Max 2 turns per level
    bool isShooting = false; // Ball is in motion
    bool isMovingSideToSide = true; // Control side-to-side movement
    GameObject[] pins;
    Vector3[] positions;

    void Start()
    {
        rb = ball.GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 50;

        // Find all pins and save their initial positions
        pins = GameObject.FindGameObjectsWithTag("Pin");
        positions = new Vector3[pins.Length];
        for (int i = 0; i < pins.Length; i++)
        {
            positions[i] = pins[i].transform.position;
        }

        UpdateScoreUI();
    }

    void Update()
    {
        // Allow side-to-side movement before throwing
        if (isMovingSideToSide && !isShooting)
        {
            MoveBallSideToSide();
        }

        // Start the throw when Enter is pressed
        if (Input.GetKeyDown(KeyCode.Return) && !isShooting)
        {
            ThrowBall();
        }

        // Check if the ball has stopped moving
        if (isShooting && ball.transform.position.y < 20)
        {
            isShooting = false; // Ball has stopped
            CountPinsDown();

            // Analyze the pin state after every turn
            HandleTurnResults();
        }
    }

    void HandleTurnResults()
    {
        if (CheckAllPinsKnockedDown())
        {
            // WIN CONDITION: Transition to the next level
            Debug.Log("All pins knocked down! Loading next level...");
            StartCoroutine(LoadNextLevelWithDelay(2f));
        }
        else
        {
            // TURN MANAGEMENT
            turnCounter++;

            if (turnCounter < 2)
            {
                // If there is a second turn available
                Debug.Log($"Turn {turnCounter} ended. Preparing for next turn...");
                StartCoroutine(PrepareForNextTurn());
            }
            else
            {
                // After the second turn, restart the level if pins are left
                Debug.Log("Player failed to knock down all pins after two turns. Restarting level...");
                StartCoroutine(RestartLevelWithDelay(2f));
            }
        }
    }

    void MoveBallSideToSide()
    {
        float movementSpeed = 1.5f; // Adjust speed as needed
        float bounds = 0.5f; // Define movement bounds

        // Oscillate the ball between the bounds
        Vector3 position = ball.transform.position;
        position.x = Mathf.PingPong(Time.time * movementSpeed, bounds * 2) - bounds;
        ball.transform.position = position;
    }

    void ThrowBall()
    {
        // Stop side-to-side movement
        isMovingSideToSide = false;

        // Reset velocity and rotation before applying force
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity; // Reset rotation to ensure straight movement

        // Apply force forward to make the ball go straight
        rb.AddForce(Vector3.forward * force, ForceMode.Impulse);

        // Mark the ball as in motion
        isShooting = true;
    }

    void CountPinsDown()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            // Check if a pin is knocked down
            if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && pins[i].activeSelf)
            {
                score++;
                pins[i].SetActive(false); // Mark the pin as knocked down
            }
        }

        UpdateScoreUI();
    }

    bool CheckAllPinsKnockedDown()
    {
        foreach (GameObject pin in pins)
        {
            if (pin.activeSelf)
            {
                return false; // At least one pin is still active
            }
        }
        return true; // All pins are inactive
    }

    void UpdateScoreUI()
    {
        // Update the score display
        scoreUI.text = score.ToString();
    }

    IEnumerator PrepareForNextTurn()
    {
        yield return new WaitForSeconds(3); // Give time for pins to settle
        ResetBallForNextTurn(); // Reset only the ball for the next turn
    }

    IEnumerator RestartLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RestartCurrentLevel(); // Restart the current level
    }

    IEnumerator LoadNextLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Add a delay before transitioning to the next level
        LoadNextLevel();
    }

    void ResetBallForNextTurn()
    {
        // Reset the ball's position and physics for the next turn
        ball.transform.position = new Vector3(0, 0.0927f, -8.33f);
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;

        // Enable side-to-side movement for the next turn
        isMovingSideToSide = true;
    }

    void RestartCurrentLevel()
    {
        Debug.Log("Restarting current level...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current level
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading next level...");
            SceneManager.LoadScene(nextSceneIndex); // Load the next level
        }
        else
        {
            Debug.Log("All levels completed! Restarting game...");
            SceneManager.LoadScene(0); // Restart at the first level
        }
    }
}
