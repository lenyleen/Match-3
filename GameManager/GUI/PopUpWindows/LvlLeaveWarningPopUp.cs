public sealed class LvlLeaveWarningPopUp : LvlWindowPopUp
{
    public void LeaveLvl()
    {
        GameSystemsManager.instance.healthSystem.RemoveHearth();
        gameScene.ToLevelMenu();
    }
}
