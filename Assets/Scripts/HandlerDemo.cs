using UnityEngine;
using UnityEngine.UI;

// Class that handles the operation of the demo
public class HandlerDemo : MonoBehaviour
{
    // --- VARIABLES ---
    // PUBLIC VARIABLES

    public GameObject panelGame;
    public GameObject panelStart;

    public Button startButton;
    public Button exitButton;
    public Dropdown animationDropdown;

    public GameObject attacker;
    public GameObject defender;

    [HideInInspector] public bool startAnimation;

    // --- METHODS ---
    // PRIVATE METHODS

    // Start is called before the first frame update
    private void Start()
    {
        startAnimation = false;

        // A method is added to be called if the button is pressed
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);

        // A method is added to be called if the drop-down value is changed
        animationDropdown.onValueChanged.AddListener(delegate {
            PlayAnimation(animationDropdown);
        });

    }

    // Activates the animations drop-down panel
    private void StartGame()
    {
        panelGame.SetActive(true);
        panelStart.SetActive(false);
    }

    // Play the animation selected from the drop-down menu.
    private void PlayAnimation(Dropdown change)
    {
        panelGame.SetActive(false);

        startAnimation = true;

        attacker.GetComponent<MoveAttacker>().enabled = true;
        attacker.GetComponent<MoveAttacker>().numAnim = change.value;

        defender.GetComponent<SwordDefender>().enabled = true;
    }


    // Quits the aplication
    private void ExitGame()
    {
        Application.Quit();
    }


    // PUBLIC METHODS

    // When the animation is over, characters are disabled by disabling their scripts 
    public void Restart()
    {
        panelGame.SetActive(true);
        attacker.GetComponent<MoveAttacker>().enabled = false;
        defender.GetComponent<SwordDefender>().enabled = false;
    }

}
