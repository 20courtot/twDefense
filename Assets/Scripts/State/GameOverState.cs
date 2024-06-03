public class GameOverState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        gameManager.GameStatusLabel.text = "Game Over";
        gameManager.nextWaveBtnLabel.text = "Play Again";
        gameManager.GameStatusImage.gameObject.SetActive(true);
        gameManager.AudioSource.PlayOneShot(SoundManager.Instance.Gameover);
    }

    public void UpdateState(GameManager gameManager)
    {
        // Logic for updating the game over state
    }

    public void ExitState(GameManager gameManager)
    {
        // Logic for cleaning up when exiting the game over state
    }
}
