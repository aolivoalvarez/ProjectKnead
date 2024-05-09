/*-----------------------------------------
Creation Date: 4/10/2024 11:05:23 PM
Author: theco
Description: Project Knead
-----------------------------------------*/

public class HeartContainerPickup : Pickup
{
    //temporary
    private new void Start()
    {
        if (GameManager.instance.heartContainerCollected)
            Destroy(gameObject);
    }
    //

    protected override void PlayerCollect()
    {
        PlayerCollectDontDestroy();
        base.PlayerCollect();
    }

    public override void PlayerCollectDontDestroy()
    {
        PlayerController.instance.maxHealth += 4;
        PlayerController.instance.HealthToMax();
        GameManager.instance.heartContainerCollected = true;
        base.PlayerCollectDontDestroy();
    }
}
