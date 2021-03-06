using UnityEngine;
using System.Collections;

public class MainGame : MonoBehaviour {
	private Player player;
	private Level currentLevel;
	
	// Use this for initialization
	void Start () {
		FutileParams fParams = new FutileParams(true,true,false,false);
		fParams.AddResolutionLevel(640.0f,2.0f,1.0f,"");
		fParams.origin = new Vector2(0,0);
		Futile.instance.Init(fParams);
		
		Futile.atlasManager.LoadAtlas("Atlases/spritesheet");
		Futile.stage.shouldSortByZ = true;
		
		// this is the tmx file located in Resources/TileMaps
		currentLevel = new Level("demo");
		Futile.stage.AddChild(currentLevel);
		
		player = new Player();
		player.sortZ = 69f;
		
		// find spawn point in collision layer
		int lengthI = currentLevel.Grid_Collision.GetLength(0);
        int lengthJ = currentLevel.Grid_Collision.GetLength(1);

        for (int i = 0; i < lengthI; i++) {
            for (int j = 0; j < lengthJ; j++) {
                if (currentLevel.Grid_Collision[i, j] == 16) {
					player.SetPosition(new Vector2(j * 16 + 8, (-i * 16)));
					break;
				}
            }
        } 
		
		currentLevel.AddChild(player);
	}
	
	// Update is called once per frame
	void Update () {
		currentLevel.Update();
		this.followPlayer();
	}
	
	private void followPlayer() {
		Vector2 playerPosition =  player.GetPosition();
		Vector2 levelSize = currentLevel.mapSize;
		
	    float halfOfTheScreenX = Futile.screen.halfWidth; 
	    float halfOfTheScreenY = Futile.screen.halfHeight;
		
		float newXPosition = currentLevel.x;
		float newYPosition = currentLevel.y;
		
		newXPosition = (halfOfTheScreenX - playerPosition.x);
	   	newYPosition = (halfOfTheScreenY - playerPosition.y);
	    
		// limit screen movement
		if (newXPosition > -currentLevel.tileWidth) 
			newXPosition = -currentLevel.tileWidth;
		if (newXPosition < -levelSize.x + halfOfTheScreenX*2 + currentLevel.tileWidth) 
			newXPosition = -levelSize.x + halfOfTheScreenX*2 + currentLevel.tileWidth;
		
		if (newYPosition < halfOfTheScreenY*2.0f)
			newYPosition = halfOfTheScreenY*2.0f;
		if (newYPosition > levelSize.y)
			newYPosition = levelSize.y;
		
		// center on screen for small maps
		if (halfOfTheScreenX*2.0f >= levelSize.x)
			newXPosition = ((halfOfTheScreenX*2.0f - levelSize.x) / 2.0f);
		if (halfOfTheScreenY*2.0f >= levelSize.y)
			newYPosition = halfOfTheScreenY*2.0f - ((halfOfTheScreenY*2.0f - levelSize.y) / 2.0f);
		
		// move the map
		currentLevel.SetPosition(new Vector2((int)newXPosition, (int)newYPosition));
	}
}
