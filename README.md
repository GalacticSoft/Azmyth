# ![Alt text](http://i.imgur.com/3ZGERJM.png)
Azmyth is a generic game framework used to create flexible RPG and adventure style games using procedural content generation in C#. It can be used to produce procedurally generated height maps and temperature maps that can be rendered in a variety of ways. It uses QuadTrees and Perlin Noise to generate psuedo-infinite maps in real time. It can also be configured to use different noise algorithms such as Simplex and Value noise. Demos are available in WinForms/GDI+ and XNA.   
	
#### Features include:
- Extensible Asset system.
- Perlin Noise, Simplex Noise, Random Noise.
- Terrain noise generation using strategy pattern.
- Flexible Stat/Attribute system.
- Markov Chain name/text generator.
- Dynamic Chunk loading using QuadTrees.

#### Planned Features:
- Biomes.
- Procedurally generated Cities and Economic simulation.
- Races and Classes.
- Additional Noise generators (Worley/Voronoi, Wavelet, etc).
- Procedurally generated NPCS, Items, Quests.

#### Code Samples

- Creating a World and Loading Chunks

		//Create World with random seed
		World world = new World(12345);
		
		//Set parameters
		world.CoastLine = 0.00f;  //Coast starts at height 0
		world.ShoreLine = 0.05f;  //Shore line is 5% higher than coast line
		world.TreeLine  = 0.40f;  //Tree line is 40% higher than coast line
		world.SnowLine  = 0.50f;  //Snow line is 50% higher than coast line
		
		//Load 50x50 chunk at tile 0, 0
		TerrainChunk chunk = world.LoadChunk(new RectangleF(0, 0, 50, 50));
		
		//Load 10x10 chunk at tile 5, 6
		TerrainChunk chunk2 = world.LoadChunk(new RectangleF(5, 6, 10, 10));
		
		//Load 100x100 chunk at tile 0, 0
		TerrainChunk chunk3 = world.LoadChunk(new RectangleF(100, 100, 100, 100));

- Chunk Loading

		/// <summary>
		/// Loads tiles from world that are contained within chunkBounds
		/// </summary>
		/// <param name="world"></param>
		/// <param name="chunkBounds"></param>
		public TerrainChunk(World world, RectangleF chunkBounds) : this()
		{
		    //Calculate total tiles in chunk
		    int totalTiles = (int)chunkBounds.Width * (int)chunkBounds.Height;
		
		    //Loop through each tile
		    for (int index = 0; index < totalTiles; index++)
		    {
		        //Convert index value into x and y coordinates.
		        int cellX = (int)((index / chunkBounds.Height)) + (int)chunkBounds.X;
		        int cellY = (int)((index % chunkBounds.Height)) + (int)chunkBounds.Y;
		
		        //Load tile.
		        TerrainTile tile = world.LoadTile(cellX, cellY, this);
		
		        //Insert new tile into chunk QuadTree
		        m_tiles.Insert(tile);
		     }
		
		     //Assign local variables
		     m_world = world;
		     m_bounds = chunkBounds;
		}

- Chunk Rendering

		/// <summary>
		/// Draws the loaded chunks visible in the viewport
		/// </summary>
		public void Draw()
		{
		    //Convert the viewport rectangle into tiles
		    RectangleF viewportRect = ScreenToTile(new Rectangle(Viewport.X, 
		        Viewport.Y, Viewport.Width, Viewport.Height));
		 
		    //Load only the chunks visible in the viewport. (Includes partial chunks)
		    List<TerrainChunk> chunks = m_world.GetChunks(viewportRect);
		
		    m_spriteBatch.Begin(SpriteSortMode.Deferred, 
		        null, null, null, null, null, Matrix.CreateTranslation(0, 0, 0));
		
		    //Loop through each visible chunk.
		    foreach (TerrainChunk chunk in chunks)
		    {
		        //Load only tiles visible in the viewport. (Includes partial tiles)
		        List<TerrainTile> tiles = chunk.GetTiles(viewportRect);
		
		        //Loop through each visible tile.
		        foreach (TerrainTile t in tiles)
		        {
		            //Draw tile to screen.
		            m_spriteBatch.Draw(m_cellTextures[t.Terrain], 
		                TileToScreen(t.Bounds), Color.White);
		
		            //Add any additional rendering for tiles here.
		        }
		
		        // Add any additional rendering for chunks here.
		    }
		
		    m_spriteBatch.End();
		}

- Dynamic Chunk Loading

		/// <summary>
		/// Loads Moore Neighborhood of chunks around central chunk specified by X/Y paramters.
		/// </summary>
		/// <param name="x">X Origin of central chunk</param>
		/// <param name="y">Y Origin of central chunk<</param>
		/// <param name="chunkSize">size of chunks to load</param>
		public void UpdateChunks(int x, int y, int chunkSize)
		{
			System.Drawing.RectangleF chunkBounds;
			List<Vector> newChunks = new List<Vector>();
	            
			// Calculate Coordinates
			int chunkX = (int)Numbers.ConvertCoordinate(x, chunkSize);
			int chunkY = (int)Numbers.ConvertCoordinate(y, chunkSize);
	
			// Calculate Moore neighborhood.
			for(int offsetY = chunkY - 1; offsetY <= chunkY + 1; offsetY++)
			{
				for(int offsetX = chunkX - 1; offsetX <= chunkX + 1; offsetX++)
				{
					newChunks.Add(new Vector(offsetX, offsetY, 0));
				}
			}
	
			// Get all the loaded Chunks.
			List<TerrainChunk> AllChunks = GetChunks();
	
			// Cull Chunks outside of neighbor range.
			foreach (TerrainChunk chunk in AllChunks)
			{
				if (!newChunks.Contains(new Vector((int)chunk.Bounds.X, (int)chunk.Bounds.Y, 0)))
				{
					chunkBounds = new System.Drawing.RectangleF(chunk.Bounds.X, chunk.Bounds.Y, 
			            						chunkSize, 	chunkSize);
			
					UnloadChunk(chunkBounds);
				}
			}
	
			// Load Neighbor Chunks
			foreach (Vector chunk in newChunks)
			{
				chunkBounds = new System.Drawing.RectangleF(chunk.X * chunkSize, chunk.Y * chunkSize, 
	                					            chunkSize, 		 chunkSize);
	
				LoadChunk(chunkBounds);
			}
		}
		

		
#### XNA
- Intro
![ScreenShot](http://i.imgur.com/4jJVU4e.png)
![ScreenShot](http://i.imgur.com/kjeI0zo.png)

- Chunk Loading
![ScreenShot](http://i.imgur.com/dXJ3UPk.png)
![ScreenShot](http://i.imgur.com/nkxVyAN.png)

- Mini Map
![ScreenShot](http://i.imgur.com/9FyRYfY.png)
![ScreenShot](http://i.imgur.com/zuaQza5.png)

#### GDI
- Terrain Map
![ScreenShot](http://i.imgur.com/V0Nfvx6.png)

- Height Map
![ScreenShot](http://i.imgur.com/9lYTbOv.png)

- Temperature Map
![ScreenShot](http://i.imgur.com/PQq61hR.png)

#### License & Copyright

Copyright (c) 2014 Marissa du Bois

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

#### Author
<p>
<table width="100%" cellpadding="0" cellspacing="0">
<tr  width="100%" height="211px" cellpadding="0" cellspacing="0">
<td cellpadding="0" cellspacing="0" height="211px" width="135px"><center><img src="http://i.imgur.com/H98nfdu.jpg"  /></center></td>
<td >
<strong>Marissa Willow du Bois</strong>
<br>
Marissa is an avid programmer and has been developing software for the past sixteen years. She is currently employed as a programmer analyst developing accounting software for the wine industry.
<br>
<br>
<strong>Contact:</strong> <a href="mailto:marissa@galacticsoft.net">marissa@galacticsoft.net</a>
<br>
<strong>Website:</strong> <a href="http://www.galacticsoft.net">http://www.galacticsoft.net</a>
<br>
<a href="http://www.linkedin.com/in/marissadubois" style="text-decoration:none;"><span style="font: 80% Arial,sans-serif; color:#0783B6;"><img src="https://static.licdn.com/scds/common/u/img/webpromo/btn_in_20x15.png" width="20" height="15" alt="View Marissa du Bois's LinkedIn profile" style="vertical-align:middle" border="0">View Marissa du Bois's profile</span></a>

</td>
</tr>
</table>
<p>
