public class NextState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        gameManager.GameStatusLabel.text = "Next Wave: " + (gameManager.WaveNumber + 1);
        gameManager.nextWaveBtnLabel.text = "Start Wave";
        gameManager.GameStatusImage.gameObject.SetActive(true);
    }

    public void UpdateState(GameManager gameManager)
    {
        // Logic for updating the next state
    }

    public void ExitState(GameManager gameManager)
    {
        // Logic for cleaning up when exiting the next state
    }
}
