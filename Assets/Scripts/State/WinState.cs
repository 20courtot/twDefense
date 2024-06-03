public class WinState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        gameManager.GameStatusLabel.text = "You Won!";
        gameManager.nextWaveBtnLabel.text = "Play Again";
        gameManager.GameStatusImage.gameObject.SetActive(true);
    }

    public void UpdateState(GameManager gameManager)
    {
        // Logic for updating the win state
    }

    public void ExitState(GameManager gameManager)
    {
        // Logic for cleaning up when exiting the win state
    }
}
