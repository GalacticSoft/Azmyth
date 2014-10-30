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

<pre style='color:#000020;background:#f6f8ff;'>
<span style='color:#595979; '>//Create World with random seed</span>
World world <span style='color:#308080; '>=</span> <span style='color:#200080; font-weight:bold; '>new</span> World<span style='color:#308080; '>(</span><span style='color:#008c00; '>12345</span><span style='color:#308080; '>)</span><span style='color:#406080; '>;</span>

<span style='color:#595979; '>//Set parameters</span>
world<span style='color:#308080; '>.</span>CoastLine <span style='color:#308080; '>=</span> <span style='color:#008000; '>0.00</span><span style='color:#006600; '>f</span><span style='color:#406080; '>;</span>  <span style='color:#595979; '>//Coast starts at height 0</span>
world<span style='color:#308080; '>.</span>ShoreLine <span style='color:#308080; '>=</span> <span style='color:#008000; '>0.05</span><span style='color:#006600; '>f</span><span style='color:#406080; '>;</span>  <span style='color:#595979; '>//Shore line is 5% higher than coast line</span>
world<span style='color:#308080; '>.</span>TreeLine  <span style='color:#308080; '>=</span> <span style='color:#008000; '>0.40</span><span style='color:#006600; '>f</span><span style='color:#406080; '>;</span>  <span style='color:#595979; '>//Tree line is 40% higher than coast line</span>
world<span style='color:#308080; '>.</span>SnowLine  <span style='color:#308080; '>=</span> <span style='color:#008000; '>0.50</span><span style='color:#006600; '>f</span><span style='color:#406080; '>;</span>  <span style='color:#595979; '>//Snow line is 50% higher than coast line</span>

<span style='color:#595979; '>//Load 50x50 chunk at tile 0, 0</span>
TerrainChunk chunk <span style='color:#308080; '>=</span> world<span style='color:#308080; '>.</span>LoadChunk<span style='color:#308080; '>(</span><span style='color:#200080; font-weight:bold; '>new</span> RectangleF<span style='color:#308080; '>(</span><span style='color:#008c00; '>0</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>0</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>50</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>50</span><span style='color:#308080; '>)</span><span style='color:#308080; '>)</span><span style='color:#406080; '>;</span>

<span style='color:#595979; '>//Load 10x10 chunk at tile 5, 6</span>
TerrainChunk chunk2 <span style='color:#308080; '>=</span> world<span style='color:#308080; '>.</span>LoadChunk<span style='color:#308080; '>(</span><span style='color:#200080; font-weight:bold; '>new</span> RectangleF<span style='color:#308080; '>(</span><span style='color:#008c00; '>5</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>6</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>10</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>10</span><span style='color:#308080; '>)</span><span style='color:#308080; '>)</span><span style='color:#406080; '>;</span>

<span style='color:#595979; '>//Load 100x100 chunk at tile 0, 0</span>
TerrainChunk chunk3 <span style='color:#308080; '>=</span> world<span style='color:#308080; '>.</span>LoadChunk<span style='color:#308080; '>(</span><span style='color:#200080; font-weight:bold; '>new</span> RectangleF<span style='color:#308080; '>(</span><span style='color:#008c00; '>100</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>100</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>100</span><span style='color:#308080; '>,</span> <span style='color:#008c00; '>100</span><span style='color:#308080; '>)</span><span style='color:#308080; '>)</span><span style='color:#406080; '>;</span>
</pre>

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
