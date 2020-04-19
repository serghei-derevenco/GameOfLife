using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Instantiater : MonoBehaviour
{	
	// public GameObject cellTemplate;
	// public GameObject templateCrocodile;
	// public GameObject templateMonkey;
	// public GameObject templateChicken;
	float generationInterval = 1F;
    
	int[ , ] monkeyArray;
	int[ , ] chickenArray;
	int[ , ] crocodileArray;
	int[ , ] lavaArray;

	public int gridHeight;
	private int gridWidth;

	private float cellSize;
	private float lavaSize;

	public Lava lava;
	public Chicken chicken;
	public Monkey monkey;
	public Crocodile crocodile;


    private void Start()
    {
    	gridWidth = Mathf.RoundToInt(gridHeight * Camera.main.aspect);

    	// cellsArray = new int[gridHeight, gridWidth];
    	cellSize = (Camera.main.orthographicSize * 2) / gridHeight;
    	lavaSize = (Camera.main.orthographicSize * 2) / gridHeight;
    	monkeyArray = new int[gridHeight, gridWidth];
    	chickenArray = new int[gridHeight, gridWidth];
    	crocodileArray = new int[gridHeight, gridWidth];
    	lavaArray = new int[gridHeight, gridWidth];
    	


    	// print("Dimensions: " + gridWidth + "X" + gridHeight);
    	// print("Cell size: " + cellSize);

    	// for (int i = 0; i < 1500; i++)
    	// {
    	// 	cellsArray[Random.Range(20, 70), Random.Range(30, 120)] = 1;
    	// }

    	// for (int i = 0; i < 5; i++)
    	// {
    	// 	cellsArray[Random.Range(20, 70), Random.Range(30, 120)] = 1;
    	// }
    	GenerateLava();
        InvokeRepeating("NewGenerationUpdate", generationInterval, generationInterval);
    }

    // private void Awake()
    // {
    // 	chicken = Resources.Load<Chicken>("Chicken");
    // 	monkey = Resources.Load<Monkey>("Monkey");
    // 	crocodile = Resources.Load<Crocodile>("Crocodile");
    // }
    
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //     	Vector3 clickPositionInScreenCoords = Input.mousePosition;
    //     	clickPositionInScreenCoords.z = -Camera.main.transform.position.z;
    //     	Vector3 clickPositionInWorldCoords = Camera.main.ScreenToWorldPoint(clickPositionInScreenCoords);
    //     	clickPositionInWorldCoords.z = 0;

        	 
    //     	Chicken newChicken = Instantiate (chicken, clickPositionInWorldCoords, Quaternion.identity) as Chicken;
    //     }

    //     if (Input.GetMouseButtonDown(1))
    //     {
    //     	Vector3 clickPositionInScreenCoords = Input.mousePosition;
    //     	clickPositionInScreenCoords.z = -Camera.main.transform.position.z;
    //     	Vector3 clickPositionInWorldCoords = Camera.main.ScreenToWorldPoint(clickPositionInScreenCoords);
    //     	clickPositionInWorldCoords.z = 0;

    //     	Monkey newMonkey = Instantiate (monkey, clickPositionInWorldCoords, Quaternion.identity) as Monkey;
    //     }

    //     if (Input.GetMouseButtonDown(2))
    //     {
    //     	Vector3 clickPositionInScreenCoords = Input.mousePosition;
    //     	clickPositionInScreenCoords.z = -Camera.main.transform.position.z;
    //     	Vector3 clickPositionInWorldCoords = Camera.main.ScreenToWorldPoint(clickPositionInScreenCoords);
    //     	clickPositionInWorldCoords.z = 0;

    //     	Crocodile newCrocodile = Instantiate (crocodile, clickPositionInWorldCoords, Quaternion.identity) as Crocodile;
    //     }
    // }

    private void NewGenerationUpdate()
    {
    	ApplyRules();
    	GenerateCells(monkey, ref monkeyArray, "Monkey");
    	GenerateCells(chicken, ref chickenArray, "Chicken");
    	GenerateCells(crocodile, ref crocodileArray, "Crocodile");
    }

    private void GenerateCells<T>(T animal, ref int[ , ] arr, string tag) where T: Unit
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag(tag))
    	{
    		Destroy(cell);
    	}
    	//arr = new int[gridHeight, gridWidth];
    	for (int i = 0; i < 3; i++)
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
    			// Monkey clone1 = Instantiate(monkey, cellPosition, Quaternion.identity) as Monkey;
    			// Crocodile clone2 = Instantiate(crocodile, cellPosition, Quaternion.identity) as Crocodile;
    			
    		}
    	}
    }

    private void GenerateLava()
    {
    	foreach (GameObject cell in GameObject.FindGameObjectsWithTag("Lava"))
    	{
    		Destroy(cell);
    	}
    	for (int i = 0; i < 3; i++)
    	{
    		lavaArray[Random.Range(20, 70), Random.Range(30, 120)] = 1;
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

    private void ApplyRules()
    {
    	//LavaDestroy();
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
    			// else if (livingNeighbours == 0)
    			// {
    			// 	nextGenGrid[i, j] = 0;
    			// }

    		}
    	}
       	
    	//arr = nextGenGrid;
    	return arr;	// GOING TO THE NEXT GEN!!! 
    }

    // private void LavaDestroy()
    // {
    // 	for(int i = 0; i < gridHeight; i++)
    // 	{
    //         for(int j = 0; j < gridWidth; j++) 
    //         {
    //             if(lavaArray[i, j] == 1) 
    //             {
    //                 if(chickenArray[i, j] == 1)
    //                 {
    //                     chickenArray[i, j] = 0;
    //                 }
    //                 if(monkeyArray[i, j] == 1)
    //                 {
    //                     monkeyArray[i, j] = 0;
    //                 }
    //                 if(crocodileArray[i, j] == 1)
    //                 {
    //                     crocodileArray[i, j] = 0;
    //                 }

    //             }
    //         }
    // 	}
    // }

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