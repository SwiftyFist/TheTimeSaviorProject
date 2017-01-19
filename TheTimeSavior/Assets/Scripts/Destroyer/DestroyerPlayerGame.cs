using UnityEngine;
using System.Collections;

public class DestroyerPlayerGame : MonoBehaviour
{
    public static float velocityModificatorByGame;
    public float velocityLimits = 10;
    public float velocityVariationDestroyed = 2;
    public float velocityVariationEnemy1 = 1;
    public float velocityVariationEnemy2 = 1;
    public float timeToResetBonus = 2;
    
    void Awake ()
    {
        velocityModificatorByGame = 0;
    }

    public void VelocityModificatorByGame (int who)
    {
        if (who == 2)
            velocityModificatorByGame = velocityModificatorByGame - velocityVariationDestroyed;
        else if (who == 1)
            velocityModificatorByGame = velocityModificatorByGame + velocityVariationEnemy1;
        else
            velocityModificatorByGame = velocityModificatorByGame + velocityVariationEnemy2;

        StartCoroutine(ModificatorByGameReset(who));
    }

    IEnumerator ModificatorByGameReset (int who)
    {
        yield return new WaitForSeconds(timeToResetBonus);
        if (who == 2)
            velocityModificatorByGame = velocityModificatorByGame + velocityVariationDestroyed;
        else if (who == 1)
            velocityModificatorByGame = velocityModificatorByGame - velocityVariationEnemy1;
        else
            velocityModificatorByGame = velocityModificatorByGame - velocityVariationEnemy2;
    }
}
