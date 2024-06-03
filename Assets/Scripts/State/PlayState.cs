public class PlayState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        gameManager.GameStatusLabel.text = "Let's GOOO";
        gameManager.nextWaveBtnLabel.text = "PLAY";
        gameManager.GameStatusImage.gameObject.SetActive(true);
    }

    public void UpdateState(GameManager gameManager)
    {
        // Logic for updating the play state, such as spawning enemies
    }

    public void ExitState(GameManager gameManager)
    {
        // Logic for cleaning up when exiting the play state
    }
}
