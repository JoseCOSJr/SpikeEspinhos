using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class life : MonoBehaviour
{
    [SerializeField]
    [Min(1)]
    private int maxLifes = 1;
    private int currentLife;

    // Start is called before the first frame update
    void Awake()
    {
        currentLife = maxLifes;
    }

   public void AddMaxLife()
    {
        maxLifes += 1;
        currentLife += 1;
    }

    public void AddLife(int add)
    {
        currentLife += add;
        if (currentLife > maxLifes)
        {
            currentLife = maxLifes;
        }
        else if (currentLife < 0)
        {
            currentLife = 0;
        }

        if (currentLife == 0)
        {
            //Adciona evento de morte do personagens
        }
    }
}
