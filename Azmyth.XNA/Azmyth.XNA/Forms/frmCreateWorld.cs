using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


using XnaGUILib;
using Azmyth;
using Azmyth.Procedural;
using Azmyth.Assets;

namespace Azmyth.XNA
{
    /// <summary>
    /// Azmyth Main Menu
    /// </summary>
    public class frmCreateWorld : XGControl
    {
        private World m_world = null;

        public AzmythGame Game;
        private Viewport m_viewport;

        public XGPanel pnlMain { get; protected set; }

        public XGPanel pnlTitle{ get; protected set; }

        public XGLabel lblTitle { get; protected set; }

        public XGLabel lblCharacteristics { get; protected set; }

        public XGMiniMap mapMiniMap { get; protected set; }

        public XGButton btnCreateWorld { get; protected set; }

        public XGLabel lblSeed { get; protected set; }

        public XGTextBox txtSeed { get; protected set; }

        public XGButton btnRandomSeed { get; protected set; }

        public XGLabel lblWorldName { get; protected set; }

        public XGTextBox txtWorldName { get; protected set; }

        public XGButton btnRandomName { get; protected set; }

        public XGLabeledSlider sldContinentSize { get; protected set; }

        public XGLabeledSlider sldCoastLine { get; protected set; }

        public XGLabeledSlider sldShoreLine { get; protected set; }

        public XGLabeledSlider sldTreeLine { get; protected set; }

        public XGLabel lblTerrainNoise { get; protected set; }
        public XGListBox lstTerrainNoise { get; protected set; }

        public XGLabel lblRiverNoise { get; protected set; }
        public XGListBox lstRiverNoise { get; protected set; }

        private MarkovNameGenerator nameGenerator;

        private XGListBoxItem liPerlin = new XGListBoxItem("Perlin Noise", NoiseTypes.Perlin);
        private XGListBoxItem liSimplex = new XGListBoxItem("Simplex Noise", NoiseTypes.Simplex);

        public frmCreateWorld(AzmythGame game)
            : base(new Rectangle(0, 0, 300, 300), true)
        {
            Game = game;
            m_viewport = game.GraphicsDevice.Viewport;
            
            Rectangle = new Rectangle(0, 0, m_viewport.Width, m_viewport.Height);

            m_world = new World(new VectorID(1, 1), new Random().Next(500, 9999));

            XnaGUIManager.Controls.Add(this);

            pnlMain             = new XGPanel(        new Rectangle(20, 20, 600, 600));
            pnlTitle            = new XGPanel(        new Rectangle(20, 20, 600, 600));
            lblTitle            = new XGLabel(        new Rectangle(20, 30, 50, 30), "Generate a New Adventure");
            lblSeed             = new XGLabel(        new Rectangle(20, 40, 50, 30), "Seed:");
            txtSeed             = new XGTextBox(      new Rectangle(80, 40, 100, 30));
            btnRandomSeed       = new XGButton(       new Rectangle(190, 40, 125, 30), "Random Seed", this.btnRandomSeed_Clicked);
            lblWorldName        = new XGLabel(        new Rectangle(20, 20, 100, 30), "Name:");
            txtWorldName        = new XGTextBox(      new Rectangle(140, 20, pnlMain.Rectangle.Width- 300 , 30));
            btnRandomName       = new XGButton(       new Rectangle(pnlMain.Rectangle.Width, 20, 125, 30), "Random Name", this.btnRandomName_Clicked);
            lblTerrainNoise     = new XGLabel(        new Rectangle(20, 80, 200, 30), "Terrain Noise");
            lstTerrainNoise     = new XGListBox(      new Rectangle(20, 110, 200, 60));
            lblRiverNoise       = new XGLabel(        new Rectangle(250, 80, 200, 30), "River Noise");
            lstRiverNoise       = new XGListBox(      new Rectangle(250, 110, 200, 60));
            lblCharacteristics  = new XGLabel(        new Rectangle(20, 180, 150, 30), "Terrain Characteristics");
            sldContinentSize    = new XGLabeledSlider(new Rectangle(20, 210, 425, 30), 150, "Continent Size", 2, ((0.1f-m_world.ContinentSize) * 100), 0, 10);
            sldCoastLine        = new XGLabeledSlider(new Rectangle(20, 240, 425, 30), 150, "Coast Line", 2, m_world.CoastLine, m_world.TerrainHeight * -1, m_world.TerrainHeight);
            sldShoreLine        = new XGLabeledSlider(new Rectangle(20, 270, 425, 30), 150, "Shore Line", 2, m_world.ShoreLine * 100, 1, 100);
            sldTreeLine         = new XGLabeledSlider(new Rectangle(20, 300, 425, 30), 150, "Tree Line", 2, m_world.TreeLine * 100, 2, 100);
            mapMiniMap          = new XGMiniMap(      new Rectangle(0, 0, 100, 100), m_world, 3);
            btnCreateWorld      = new XGButton(       new Rectangle(m_viewport.Width - 210, m_viewport.Height - 40, 200, 30), "Create World", this.btnCreateWorld_Clicked);

            sldContinentSize.ValueLabelFormat = "0";
            sldCoastLine.ValueLabelFormat = "0";
            sldShoreLine.ValueLabelFormat = "0";
            sldTreeLine.ValueLabelFormat = "0";

            pnlMain.CanFocus = false;
            lblWorldName.CanFocus = false;
            lblCharacteristics.CanFocus = false;
            mapMiniMap.CanFocus = false;

            nameGenerator = new MarkovNameGenerator("Acalia,Aldaire,Aldebron,Vulcan,Earth,Romulus,Andor,Adair,Adara,Adriel,Alaire,Alixandra,Altair,Amara,Anatola,Arcadia,Aurelia,Aurelian,Aurelius,Avalon,Bastian,Breen,Briallan,Brielle,Briseis,Cambria,Caspian,Cassia,Cassiel,Cassiopeia,Cassius,Chaniel,Cora,Corbin,Cyprian,Dagen,Daire,Darius,Destin,Devlin,Devlyn,Drake,Drystan,Eira,Eirian,Eliron,Elysia,Eoin,Evadne,Evanth,Fineas,Finian,Fyodor,Gaerwn,Gareth,Gavriel,Ginerva,Griffin,Guinevere,Hadriel,Hannelore,Hermione,Hesperos,Iagan,Ianthe,Ignacia,Ignatius,Iseult,Isolde,Jessalyn,Kara,Katriel,Kerensa,Korbin,Kyler,Kyra,Kyrielle,Leala,Leila,Leira,Lilith,Liora,Liriene,Liron,Lucien,Lyra,Maia,Marius,Mathieu,Maylea,Meira,Mireille,Mireya,Natania,Neirin,Nerys,Nuriel,Nyfain,Nyssa,Oisin,Oleisa,Oralie,Orinthea,Orion,Orpheus,Ozara,Peregrine,Persephone,Perseus,Petronela,Phelan,Pryderi,Pyralia,Pyralis,Qadira,Quinevere,Quintessa,Raisa,Remus,Renfrew,Rhyan,Rhydderch,Riona,Saira,Saoirse,Sarai,Sarielle,Sebastian,Seraphim,Seraphina,Serian,Sirius,Sorcha,Severin,Tavish,Tearlach,Terra,Thalia,Thaniel,Theia,Torian,Torin,Tressa,Tristana,Ulyssia,Uriela,Urien,Vanora,Vasilis,Vespera,Xanthus,Xara,Xylia,Yadira,Yakira,Yeira,Yeriel,Yestin,Yseult,Zaira,Zaniel,Zarek,Zephyr,Zora,Zorion".Split(','), 3);

            txtSeed.Text = m_world.Seed.ToString();
            lblTitle.Alignment = GUIAlignment.HCenter | GUIAlignment.VCenter;
            txtWorldName.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.Next());

            lstTerrainNoise.Items.Add(liPerlin);
            lstTerrainNoise.Items.Add(liSimplex);

            lstTerrainNoise.SelectedIndex = 0;

            lstRiverNoise.Items.Add(liPerlin);
            lstRiverNoise.Items.Add(liSimplex);

            lstRiverNoise.SelectedIndex = 0;

            Children.Add(pnlMain);
            Children.Add(pnlTitle);
            pnlTitle.Children.Add(lblTitle);
            pnlMain.Children.Add(lblSeed);
            pnlMain.Children.Add(txtSeed);
            pnlMain.Children.Add(btnRandomSeed);
            pnlMain.Children.Add(lblWorldName);
            pnlMain.Children.Add(txtWorldName);
            pnlMain.Children.Add(btnRandomName);
            pnlMain.Children.Add(lblCharacteristics);
            pnlMain.Children.Add(sldContinentSize);
            pnlMain.Children.Add(sldCoastLine);
            pnlMain.Children.Add(sldShoreLine);
            pnlMain.Children.Add(sldTreeLine);
            pnlMain.Children.Add(lblTerrainNoise);
            pnlMain.Children.Add(lstTerrainNoise);
            pnlMain.Children.Add(lblRiverNoise);
            pnlMain.Children.Add(lstRiverNoise);
            pnlMain.Children.Add(mapMiniMap);
            pnlMain.Children.Add(btnCreateWorld);

            focusControl = pnlMain;

            pnlMain.ActivateFirst();

            XnaGUIManager.ActivateNext();
        }

        void btnRandomSeed_Clicked(XGControl sender)
        {
            txtSeed.Text = new Random().Next(500, 9999).ToString();
        }

        void btnCreateWorld_Clicked(XGControl sender)
        {
            Game.World = m_world;

            Game.TerrainManager.LoadChunk(-1, -2);
            Game.TerrainManager.LoadChunk(-1, -1);
            Game.TerrainManager.LoadChunk(-1, 0);
            Game.TerrainManager.LoadChunk(-1, 1);

            Game.TerrainManager.LoadChunk(0, -2);
            Game.TerrainManager.LoadChunk(0, -1);
            Game.TerrainManager.LoadChunk(0, 0);
            Game.TerrainManager.LoadChunk(0, 1);

            Game.TerrainManager.LoadChunk(1, -2);
            Game.TerrainManager.LoadChunk(1, -1);
            Game.TerrainManager.LoadChunk(1, 0);
            Game.TerrainManager.LoadChunk(1, 1);

            Game.TerrainManager.LoadChunk(2, -2);
            Game.TerrainManager.LoadChunk(2, -1);
            Game.TerrainManager.LoadChunk(2, 0);
            Game.TerrainManager.LoadChunk(2, 1);

            Game.TerrainManager.LoadChunk(3, -2);
            Game.TerrainManager.LoadChunk(3, -1);
            Game.TerrainManager.LoadChunk(3, 0);
            Game.TerrainManager.LoadChunk(3, 1);

            Game.TerrainManager.LoadChunk(-2, -2);
            Game.TerrainManager.LoadChunk(-2, -1);
            Game.TerrainManager.LoadChunk(-2, 0);
            Game.TerrainManager.LoadChunk(-2, 1);

            Game.TerrainManager.LoadChunk(-3, -2);
            Game.TerrainManager.LoadChunk(-3, -1);
            Game.TerrainManager.LoadChunk(-3, 0);
            Game.TerrainManager.LoadChunk(-3, 1);

            Game.m_stateManager.SetState(GameStates.Playing);
        }

        void btnRandomName_Clicked(XGControl sender)
        {
            txtWorldName.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.Next());
        }

        public override void Update(GameTime gameTime)
        {
            int seed = 0;

            m_viewport = Game.GraphicsDevice.Viewport;

            Rectangle = new Rectangle(0, 0, m_viewport.Width - 0, m_viewport.Height - 0);
            pnlMain.Rectangle = new Rectangle((Rectangle.Width / 2) - 385, (Rectangle.Height / 2) - 205, 770, 420);
            pnlTitle.Rectangle = new Rectangle((Rectangle.Width / 2) - 275, (Rectangle.Height / 2) - 225, 550, 40);
            lblTitle.Rectangle = new Rectangle(0, 0, pnlTitle.Rectangle.Width, pnlTitle.Rectangle.Height);
            btnCreateWorld.Rectangle = new Rectangle(pnlMain.Rectangle.Width - 220, pnlMain.Rectangle.Height - 50, 200, 30);

            lblWorldName.Rectangle = new Rectangle(pnlMain.Rectangle.Width - 445, 40, 100, 30);
            txtWorldName.Rectangle = new Rectangle(pnlMain.Rectangle.Width - 390, 40, 210 , 30);
            btnRandomName.Rectangle = new Rectangle(pnlMain.Rectangle.Width - 170, 40, 150, 30);

            mapMiniMap.Rectangle = new Rectangle(pnlMain.Rectangle.Width - 270, 100, 250, 250);

            if (lstTerrainNoise.SelectedIndex == 0)
            {
                m_world.TerrainNoise = NoiseTypes.Perlin;
            }
            else
            {
                m_world.TerrainNoise = NoiseTypes.Simplex;
            }

            if (lstRiverNoise.SelectedIndex == 0)
            {
                m_world.RiverNoise = NoiseTypes.Perlin;
            }
            else
            {
                m_world.RiverNoise = NoiseTypes.Simplex;
            }

            if (int.TryParse(txtSeed.Text, out seed))
            {
                m_world.Seed = seed;
            }

            m_world.ContinentSize = ((10 - sldContinentSize.Value) / 100);
            m_world.CoastLine = sldCoastLine.Value;
            m_world.ShoreLine = sldShoreLine.Value / 100;
            m_world.TreeLine = sldTreeLine.Value / 100;

            m_world.Name = txtWorldName.Text;

            base.Update(gameTime);
        }
    }
}
