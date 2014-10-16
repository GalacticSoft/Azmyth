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


namespace Azmyth.XNA
{
    /// <summary>
    /// Azmyth Main Menu
    /// </summary>
    public class frmCreateWorld : XGControl
    {
        public AzmythGame Game;
        private Viewport m_viewport;

        public XGPanel pnlMain { get; protected set; }

        public XGLabel lblCharacteristics { get; protected set; }

        public XGPanel pnlCharacteristics { get; protected set; }

        public XGButton btnCreateWorld { get; protected set; }

        public XGLabel lblWorldName { get; protected set; }

        public XGTextBox txtWorldName { get; protected set; }

        public XGButton btnRandomName { get; protected set; }

        private MarkovNameGenerator nameGenerator;

        public frmCreateWorld(AzmythGame game)
            : base(new Rectangle(0, 0, 300, 300), true)
        {
            Game = game;
            m_viewport = game.GraphicsDevice.Viewport;

            Rectangle = new Rectangle(0, 0, m_viewport.Width, m_viewport.Height);

            XnaGUIManager.Controls.Add(this);

            pnlMain = new XGPanel(new Rectangle(20, 20, m_viewport.Width - 40, m_viewport.Height - 40));

            btnCreateWorld = new XGButton(new Rectangle(m_viewport.Width - 210, m_viewport.Height - 40, 200, 30), "Create World", this.btnCreateWorld_Clicked);
            
            lblWorldName = new XGLabel(new Rectangle(20, 20, 100, 30), "World Name:");
            txtWorldName = new XGTextBox(new Rectangle(140, 20, pnlMain.Rectangle.Width- 300 , 30));
            btnRandomName = new XGButton(new Rectangle(pnlMain.Rectangle.Width, 20, 150, 30), "Generate Name", this.btnRandomName_Clicked);

            lblCharacteristics = new XGLabel(new Rectangle(20, 70, 150, 30), "Terrain Characteristics");
            pnlCharacteristics = new XGPanel(new Rectangle(0, 0, pnlMain.Rectangle.Width, 500));

            nameGenerator = new MarkovNameGenerator("Acalia,Aldaire,Aldebron,Vulcan,Earth,Romulus,Andor,Adair,Adara,Adriel,Alaire,Alixandra,Altair,Amara,Anatola,Arcadia,Aurelia,Aurelian,Aurelius,Avalon,Bastian,Breen,Briallan,Brielle,Briseis,Cambria,Caspian,Cassia,Cassiel,Cassiopeia,Cassius,Chaniel,Cora,Corbin,Cyprian,Dagen,Daire,Darius,Destin,Devlin,Devlyn,Drake,Drystan,Eira,Eirian,Eliron,Elysia,Eoin,Evadne,Evanth,Fineas,Finian,Fyodor,Gaerwn,Gareth,Gavriel,Ginerva,Griffin,Guinevere,Hadriel,Hannelore,Hermione,Hesperos,Iagan,Ianthe,Ignacia,Ignatius,Iseult,Isolde,Jessalyn,Kara,Katriel,Kerensa,Korbin,Kyler,Kyra,Kyrielle,Leala,Leila,Leira,Lilith,Liora,Liriene,Liron,Lucien,Lyra,Maia,Marius, Mathieu,Maylea,Meira,Mireille,Mireya,Natania,Neirin,Nerys,Nuriel,Nyfain,Nyssa,Oisin,Oleisa,Oralie,Orinthea,Orion,Orpheus,Ozara,Peregrine,Persephone,Perseus,Petronela,Phelan,Pryderi,Pyralia,Pyralis,Qadira,Quinevere,Quintessa,Raisa,Remus,Renfrew,Rhyan,Rhydderch,Riona,Saira,Saoirse,Sarai,Sarielle,Sebastian,Seraphim,Seraphina,Serian,Sirius,Sorcha,Séverin,Tavish,Tearlach,Terra,Thalia,Thaniel,Theia,Torian,Torin,Tressa,Tristana,Ulyssia,Uriela,Urien,Vanora,Vasilis,Vespera,Xanthus,Xara,Xylia,Yadira,Yakira,Yeira,Yeriel,Yestin,Yseult,Zaira,Zaniel,Zarek,Zephyr,Zora,Zorion".Split(','), 3);

            txtWorldName.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.Next());
            
            Children.Add(pnlMain);
            
            pnlMain.Children.Add(lblWorldName);
            pnlMain.Children.Add(txtWorldName);
            pnlMain.Children.Add(btnRandomName);
            pnlMain.Children.Add(lblCharacteristics);
            pnlMain.Children.Add(pnlCharacteristics);
            pnlMain.Children.Add(btnCreateWorld);
        }

        void btnCreateWorld_Clicked(XGControl sender)
        {
           // World world = Assets.GetWorld(worldID);
            //world.Seed = new Random().Next(500, 9999);
            //world.Name = "World " + world.AssetID.ID;

            VectorID worldID = Azmyth.Assets.Assets.CreateWorld();

            Game.World = Azmyth.Assets.Assets.GetWorld(worldID);

            Game.World.ShoreLine = .20f;

            Game.TerrainManager.World = Game.World;
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

            Game.TerrainManager.CenterTile(1, 1);

            Game.State = GameState.Playing;
        }

        void btnRandomName_Clicked(XGControl sender)
        {
            txtWorldName.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.Next());
        }

        public void Show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        public void Close()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            m_viewport = Game.GraphicsDevice.Viewport;

            Rectangle = new Rectangle(0, 0, m_viewport.Width - 0, m_viewport.Height - 0);
            pnlMain.Rectangle = new Rectangle(64, 64, m_viewport.Width - 128, m_viewport.Height - 128);
            btnCreateWorld.Rectangle = new Rectangle(pnlMain.Rectangle.Width - 230, pnlMain.Rectangle.Height - 40, 200, 30);
            txtWorldName.Rectangle = new Rectangle(140, 20, pnlMain.Rectangle.Width - 320, 30);
            btnRandomName.Rectangle = new Rectangle(pnlMain.Rectangle.Width - 170, 20, 150, 30);
            pnlCharacteristics.Rectangle = new Rectangle(20, 100, pnlMain.Rectangle.Width - 40, 200);

            base.Update(gameTime);
        }
    }
}
