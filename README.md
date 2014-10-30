# ![Alt text](http://i.imgur.com/3ZGERJM.png)
Azmyth is a generic game framework used to create flexible RPG and adventure style games using procedural content generation.  
	
#### Features include:
- Extensible Asset system.
- Perlin Noise, Simplex Noise, Random Noise
- Terrain generation using Perlin Noise and QuadTrees.
- Flexible Stat/Attribute system.
- Markov Chain name/text generator.
- Chunk loading.

#### Planned Features:
- Biomes.
- Procedurally generated Cities and Economic simulation.
- Races and Classes.
- Additional Noise generators (Worley/Voronoi, Wavelet, etc).
- Procedurally generated NPCS, Items, Quests.
- Dynamic Chunk Loading.

#### Code Sample

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
<strong>Contact:</strong> marissa@galacticsoft.net
<br>
<strong>Website:</strong> http://www.galacticsoft.net
</td>
</tr>
</table>
<p>
