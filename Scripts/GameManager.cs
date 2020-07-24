using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public bool Playing { get; private set; }
    public bool GameWon { get; private set; }
    public bool GameLost { get; private set; }

    [SerializeField] PlayerController playerController = default;
    [SerializeField] EntityFactory entityFactory = default;
    [SerializeField] Vector2 buffManStart = default;
    [SerializeField] Vector2 jumpManStart = default;

    int playersLeftToWin = 2;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }

        Playing = true;
        GameWon = false;
    }

    private void Start()
    {
        var buffMan = entityFactory.Spawn(0);
        var jumpMan = entityFactory.Spawn(1);
        buffMan.transform.position = buffManStart;
        jumpMan.transform.position = jumpManStart;
        playerController.AddPlayer(buffMan.GetComponent<Player>(), 0);
        playerController.AddPlayer(jumpMan.GetComponent<Player>(), 1);

        Playing = true;
    }

    public void ReachedGoal() {
        playersLeftToWin--;
        if (playersLeftToWin <= 0) {
            Win();
        }
    }

    public void LeftGoal()
    {
        playersLeftToWin++;
    }

    private void Win()
    {
        Debug.Log("You win!");
        GameWon = true;
        Playing = false;
        AudioManager.Instance.Victory();
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(15);
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == SceneManager.sceneCountInBuildSettings - 1) SceneManager.LoadScene(0);
        else SceneManager.LoadScene(sceneIndex + 1);
    }

    IEnumerator LoadMenu()
    {
        while (!Input.anyKeyDown) {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }


    public void Lose()
    {
        Debug.Log("You Lose!");
        Playing = false;
        AudioManager.Instance.Defeat();
        StartCoroutine(LoadMenu());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(buffManStart, 0.5f);
        Gizmos.DrawSphere(jumpManStart, 0.5f);
    }
}
