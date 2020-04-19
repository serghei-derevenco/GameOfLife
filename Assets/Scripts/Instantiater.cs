using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Instantiater : MonoBehaviour
{	
	public static float generationInterval = 1F;
    
	int[ , ] monkeyArray;
	int[ , ] chickenArray;
	int[ , ] crocodileArray;
	int[ , ] lavaArray;
	int[ , ] bananaArray;
	int[ , ] fertileArray;

	public int gridHeight;
	private int gridWidth;

	private float cellSize;
	private float lavaSize;
	private float bananaSize;
	private float fertileSize;

	public Fertile fertile;
	public Banana banana;
	public Lava lava;
	public Chicken chicken;
	public Monkey monkey;
	public Crocodile crocodile;


    private void Start()
    {
    	gridWidth = Mathf.RoundToInt(gridHeight * Camera.main.aspect);

    	cellSize = (Camera.main.orthographicSize * 2) / gridHeight;
    	lavaSize = (Camera.main.orthographicSize * 2) / gridHeight;
    	bananaSize = (Camera.main.orthographicSize * 2) / gridHeight;
    	fertileSize = (Camera.main.orthographicSize * 2) / gridHeight;

    	monkeyArray = new int[gridHeight, gridWidth];
    	chickenArray = new int[gridHeight, gridWidth];
    	crocodileArray = new int[gridHeight, gridWidth];
    	lavaArray = new int[gridHeight, gridWidth];
    	bananaArray = new int[gridHeight, gridWidth];
    	fertileArray = new int[gridHeight, gridWidth];
    	
    	GenerateFertile();
    	GenerateBanana();
    	GenerateLava();
        InvokeRepeating("NewGenerationUpdate", generationInterval, generationInterval);
    }

    private void NewGenerationUpdate()
    {
    	ApplyRules();
    	GenerateCells(monkey, ref monkeyArray, "Monkey");
    	GenerateCells(chicken, ref chickenArray, "Chicken");
    	GenerateCells(crocodile, ref crocodileArray, "Crocodile");
    }
    
	private bool CheckOverlapping(int[,] arr, int row, int col) {
	    int start, start1, final, final1;

	    if (row - 20 < 0)
	        start = 0;
	    else
	        start = row - 20;
	    if (row + 20 >= gridHeight)
	        final = gridHeight - 1;
	    else
	        final = row + 20;


	    if (col - 20 < 0)
	        start1 = 0;
	    else
	        start1 = col - 20;
	    if (col + 20 >= gridWidth)
	        final1 = gridWidth - 1;
	    else
	        final1 = col + 20;


	    for(int i = start; i <= final; i++)
	        for (int j = start1; j <= final1; j++)
	            if(arr[i,j] == 1)
	                return false;


	    return true;
	}
    
    private void GenerateFertile()
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag("Fertile"))
    	{
    		Destroy(cell);
    	}
    	for (int i = 0; i < 3; i++)
    	{
    		fertileArray[Random.Range(20, 70), Random.Range(30, 120)] = 1;
    	}

    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			if (fertileArray[i, j] == 0) continue;
    			Vector3 fertilePosition = new Vector3(
    				j * fertileSize + fertileSize/2,
    				(fertileSize * gridHeight) - (i * fertileSize + fertileSize/2),
    				0
    			);

    			Fertile clone3 = Instantiate(fertile, fertilePosition, Quaternion.identity) as Fertile;
    		}
    	}
    }

    private void GenerateBanana()
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag("Banana"))
    	{
    		Destroy(cell);
    	}

    	int count = 0;
    	while (count != 3)
    	{
    		int row = Random.Range(20, 70);
    		int col = Random.Range(30, 120);

    		if (!CheckOverlapping(fertileArray, row, col) || !CheckOverlapping(lavaArray, row, col))
    		{
    			bananaArray[row, col] = 1;
    			count++;
    		}
    	}

    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			if (bananaArray[i, j] == 0) continue;
    			Vector3 bananaPosition = new Vector3(
    				j * bananaSize + bananaSize/2,
    				(bananaSize * gridHeight) - (i * bananaSize + bananaSize/2),
    				0
    			);

    			Banana clone2 = Instantiate(banana, bananaPosition, Quaternion.identity) as Banana;
    		}
    	}
    }

    private void GenerateLava()
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag("Lava"))
    	{
    		Destroy(cell);
    	}

    	int count = 0;
    	while (count != 5)
    	{
    		int row = Random.Range(20, 70);
    		int col = Random.Range(30, 120);

    		if (!CheckOverlapping(bananaArray, row, col) || !CheckOverlapping(fertileArray, row, col))
    		{
    			lavaArray[row, col] = 1;
    			count++;
    		}
    	}

    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			if (lavaArray[i, j] == 0) continue;
    			Vector3 lavaPosition = new Vector3(
    				j * lavaSize + lavaSize/2,
    				(lavaSize * gridHeight) - (i * lavaSize + lavaSize/2),
    				0
    			);

    			Lava clone1 = Instantiate(lava, lavaPosition, Quaternion.identity) as Lava;
    		}
    	}
    }

    private void GenerateCells<T>(T animal, ref int[ , ] arr, string tag) where T: Unit
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag(tag))
    	{
    		Destroy(cell);
    	}

    	for (int i = 0; i < 5; i++)
    	{
    		arr[Random.Range(20, 70), Random.Range(30, 120)] = 1;
    	}

    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			if (arr[i, j] == 0) continue;
    			Vector3 cellPosition = new Vector3(
    				j * cellSize + cellSize/2,
    				(cellSize * gridHeight) - (i * cellSize + cellSize/2),
    				0
    			);

    			T clone = Instantiate(animal, cellPosition, Quaternion.identity) as T;    			
    		}
    	}
    }


    private void ApplyRules()
    {
    	EatChicken();
    	PeckMonkey();
    	KillCrocodile();
    	monkeyArray = Breed(monkeyArray);
    	chickenArray = Breed(chickenArray);
    	crocodileArray = Breed(crocodileArray);
    }

    private int[,] Breed(int[, ] arr)
    {
    	int[ , ]nextGenGrid = new int[gridHeight, gridWidth];
    	for (int i = 0; i < gridHeight; i++)
    	{
    		for (int j = 0; j < gridWidth; j++ )
    		{
    			int livingNeighbours = CountLivingNeighbours(i, j, arr);
    			if (livingNeighbours == 3)	// Reproduction, exactly 3 neighbours
    			{
    				nextGenGrid[i, j] = 1;
    			}
    			else if (livingNeighbours == 2 && arr[i, j] == 1)	// exactly 2 neigh, the live cell survives
    			{
    				nextGenGrid[i, j] = 1;
    			}
    			else if (livingNeighbours == 0)
    			{
    				nextGenGrid[i, j] = 0;
    			}

    		}
    	}
       	
    	return arr;	// GOING TO THE NEXT GEN!!! 
    }


    private void EatChicken() 
    {
        for(int i = 1; i < gridHeight - 1; i++)
        {
            for(int j = 1; j < gridWidth - 1; j++ ) 
            {
                if (crocodileArray[i, j] == 0) continue;

                if (chickenArray[i - 1, j] == 1) 
                {
                    chickenArray[i - 1, j] = 0;
                    crocodileArray[i, j] = 0;
                    crocodileArray[i - 1, j] = 1;
                    continue;
                }

                if (chickenArray[i - 1, j - 1] == 1) 
                {
                    chickenArray[i - 1, j - 1] = 0;
                    crocodileArray[i - 1, j - 1] = 1;
                    crocodileArray[i, j] = 0;
                    continue;
                }

                if (chickenArray[i - 1, j + 1] == 1) 
                {
                    chickenArray[i - 1, j + 1] = 0;
                    crocodileArray[i - 1, j + 1] = 1;
                    crocodileArray[i, j] = 0;
                    continue;
                }

                if (chickenArray[i, j - 1] == 1) 
                {
                    chickenArray[i, j - 1] = 0;
                    crocodileArray[i, j - 1] = 1;
                    crocodileArray[i, j] = 0;
                    continue;
                }

                if (chickenArray[i, j + 1] == 1) 
                {
                    chickenArray[i, j + 1] = 0;
                    crocodileArray[i, j + 1] = 1;
                    crocodileArray[i, j] = 0;
                    continue;
                }

                if (chickenArray[i + 1, j - 1] == 1) 
                {
                    chickenArray[i + 1, j - 1] = 0;
                    crocodileArray[i + 1, j - 1] = 1;
                    crocodileArray[i, j] = 0;
                    continue;
                }

                if (chickenArray[i + 1, j] == 1) 
                {
                    chickenArray[i + 1, j] = 0;
                    crocodileArray[i + 1, j] = 1;
                    crocodileArray[i, j] = 0;
                    continue;
                }

                if (chickenArray[i + 1, j + 1] == 1) 
                {
                    chickenArray[i + 1, j + 1] = 0;
                    crocodileArray[i + 1, j + 1] = 1;
                    crocodileArray[i, j] = 0;
                    continue;
                }
            }
        }
    }

    private void PeckMonkey()
    {
    	for(int i = 1; i < gridHeight - 1; i++)
        {
            for(int j = 1; j < gridWidth - 1; j++ ) 
            {
            	if (monkeyArray[i, j] == 1)
            	{
	            	if (chickenArray[i - 1, j] == 1 && chickenArray[i + 1, j] == 1 && chickenArray[i, j - 1] == 1 && chickenArray[i, j + 1] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i - 1, j - 1] == 1 && chickenArray[i - 1, j + 1] == 1 && chickenArray[i + 1, j - 1] == 1 && chickenArray[i + 1, j + 1] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i + 1, j - 1] == 1 && chickenArray[i, j - 1] == 1 && chickenArray[i - 1, j - 1] == 1 && chickenArray[i - 1, j] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i - 1, j] == 1 && chickenArray[i - 1, j + 1] == 1 && chickenArray[i, j + 1] == 1 && chickenArray[i + 1, j + 1] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i + 1, j - 1] == 1 && chickenArray[i, j - 1] == 1 && chickenArray[i - 1, j - 1] == 1 && chickenArray[i + 1, j] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i + 1, j] == 1 && chickenArray[i - 1, j + 1] == 1 && chickenArray[i, j + 1] == 1 && chickenArray[i + 1, j + 1] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i - 1, j - 1] == 1 && chickenArray[i - 1, j] == 1 && chickenArray[i - 1, j + 1] == 1 && chickenArray[i, j + 1] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i - 1, j - 1] == 1 && chickenArray[i - 1, j] == 1 && chickenArray[i - 1, j + 1] == 1 && chickenArray[i, j - 1] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i, j - 1] == 1 && chickenArray[i + 1, j - 1] == 1 && chickenArray[i + 1, j] == 1 && chickenArray[i + 1, j + 1] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (chickenArray[i, j + 1] == 1 && chickenArray[i + 1, j - 1] == 1 && chickenArray[i + 1, j] == 1 && chickenArray[i + 1, j + 1] == 1)
	            	{
	            		monkeyArray[i, j] = 0;
	            		continue;	
	            	}
	            }	
            }	
        }    
    }

    private void KillCrocodile()
    {
    	for(int i = 1; i < gridHeight - 1; i++)
    	{
    		for(int j = 1; j < gridWidth - 1; j++)
    		{
    			if (crocodileArray[i, j] == 1)
    			{
    				if (monkeyArray[i - 1, j] == 1 && monkeyArray[i + 1, j] == 1 && monkeyArray[i, j - 1] == 1 && monkeyArray[i, j + 1] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i - 1, j - 1] == 1 && monkeyArray[i - 1, j + 1] == 1 && monkeyArray[i + 1, j - 1] == 1 && monkeyArray[i + 1, j + 1] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i + 1, j - 1] == 1 && monkeyArray[i, j - 1] == 1 && monkeyArray[i - 1, j - 1] == 1 && monkeyArray[i - 1, j] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i - 1, j] == 1 && monkeyArray[i - 1, j + 1] == 1 && monkeyArray[i, j + 1] == 1 && monkeyArray[i + 1, j + 1] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i + 1, j - 1] == 1 && monkeyArray[i, j - 1] == 1 && monkeyArray[i - 1, j - 1] == 1 && monkeyArray[i + 1, j] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i + 1, j] == 1 && monkeyArray[i - 1, j + 1] == 1 && monkeyArray[i, j + 1] == 1 && monkeyArray[i + 1, j + 1] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i - 1, j - 1] == 1 && monkeyArray[i - 1, j] == 1 && monkeyArray[i - 1, j + 1] == 1 && monkeyArray[i, j + 1] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i - 1, j - 1] == 1 && monkeyArray[i - 1, j] == 1 && monkeyArray[i - 1, j + 1] == 1 && monkeyArray[i, j - 1] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i, j - 1] == 1 && monkeyArray[i + 1, j - 1] == 1 && monkeyArray[i + 1, j] == 1 && monkeyArray[i + 1, j + 1] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}

	            	if (monkeyArray[i, j + 1] == 1 && monkeyArray[i + 1, j - 1] == 1 && monkeyArray[i + 1, j] == 1 && monkeyArray[i + 1, j + 1] == 1)
	            	{
	            		crocodileArray[i, j] = 0;
	            		continue;	
	            	}
    			}
    		}
    	}
    }

    private int CountLivingNeighbours(int i, int j, int[,] arr)
    {
    	int result = 0;
    	for (int iNeigh = i - 1; iNeigh < i + 2; iNeigh++)
    	{
    		for (int jNeigh = j - 1; jNeigh < j + 2; jNeigh++)
    		{
    			if (iNeigh == i && jNeigh == j) continue;
    			try 
    			{
    				result += arr[iNeigh, jNeigh];
    			}
    			catch{}
    		}
    	}

    	return result;
    }

}