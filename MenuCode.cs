using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management functions

public class MenuCode : MonoBehaviour // Class definition (MenuCode is a class that inherits from MonoBehaviour)
{
    // Method (called when the "Play Game" button is clicked)
    public void PlayGame()
    {
        SceneManager.LoadScene(2); // Method (Loads the scene with build index 2)
    }

    // Method (called when the "Exit Game" button is clicked)
    public void ExitGame()
    {
        Application.Quit(); // Method (Quits the application)
    }

    // Method (called when the "Instructions" button is clicked)
    public void instructions()
    {
        SceneManager.LoadScene(1); // Method (Loads the scene with build index 1, typically for instructions)
    }

    // Method (called when the "Info" button is clicked)
    public void infoGame()
    {
        SceneManager.LoadScene(0); // Method (Loads the scene with build index 0, typically for game information or the main menu)
    }
}
