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
    class frmCreatePlayer : XGControl
    {
        private World m_world = null;

        public AzmythGame Game;
        private Viewport m_viewport;

        public XGPanel pnlMain { get; protected set; }

        public XGPanel pnlTitle{ get; protected set; }

        public XGLabel lblTitle { get; protected set; }

        //public XGLabel lblCharacteristics { get; protected set; }

        //public XGMiniMap mapMiniMap { get; protected set; }

        public XGButton btnCreatePlayer { get; protected set; }

        //public XGLabel lblSeed { get; protected set; }

        //public XGTextBox txtSeed { get; protected set; }

        //public XGButton btnRandomSeed { get; protected set; }

        public XGLabel lblPlayerName { get; protected set; }

        public XGTextBox txtPlayerName { get; protected set; }

        public XGButton btnRandomName { get; protected set; }

        //public XGLabeledSlider sldContinentSize { get; protected set; }

       // public XGLabeledSlider sldCoastLine { get; protected set; }

       // public XGLabeledSlider sldShoreLine { get; protected set; }

       // public XGLabeledSlider sldTreeLine { get; protected set; }

        //public XGLabel lblTerrainNoise { get; protected set; }
       // public XGListBox lstTerrainNoise { get; protected set; }

        //public XGLabel lblRiverNoise { get; protected set; }
        //public XGListBox lstRiverNoise { get; protected set; }

        private MarkovNameGenerator nameGenerator;

        //private XGListBoxItem liPerlin = new XGListBoxItem("Perlin Noise", NoiseTypes.Perlin);
        //private XGListBoxItem liSimplex = new XGListBoxItem("Simplex Noise", NoiseTypes.Simplex);

        public frmCreatePlayer(AzmythGame game)
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
            lblPlayerName        = new XGLabel(        new Rectangle(20, 20, 100, 30), "Name:");
            txtPlayerName        = new XGTextBox(      new Rectangle(140, 20, pnlMain.Rectangle.Width- 300 , 30));
            btnRandomName       = new XGButton(       new Rectangle(pnlMain.Rectangle.Width, 20, 125, 30), "Random Name", this.btnRandomName_Clicked);
            btnCreatePlayer     = new XGButton(       new Rectangle(m_viewport.Width - 210, m_viewport.Height - 40, 200, 30), "Create World", this.btnCreatePlayer_Clicked);


            pnlMain.CanFocus = false;
            lblPlayerName.CanFocus = false;

            nameGenerator = new MarkovNameGenerator("Acalia,Aldaire,Aldebron,Vulcan,Earth,Romulus,Andor,Adair,Adara,Adriel,Alaire,Alixandra,Altair,Amara,Anatola,Arcadia,Aurelia,Aurelian,Aurelius,Avalon,Bastian,Breen,Briallan,Brielle,Briseis,Cambria,Caspian,Cassia,Cassiel,Cassiopeia,Cassius,Chaniel,Cora,Corbin,Cyprian,Dagen,Daire,Darius,Destin,Devlin,Devlyn,Drake,Drystan,Eira,Eirian,Eliron,Elysia,Eoin,Evadne,Evanth,Fineas,Finian,Fyodor,Gaerwn,Gareth,Gavriel,Ginerva,Griffin,Guinevere,Hadriel,Hannelore,Hermione,Hesperos,Iagan,Ianthe,Ignacia,Ignatius,Iseult,Isolde,Jessalyn,Kara,Katriel,Kerensa,Korbin,Kyler,Kyra,Kyrielle,Leala,Leila,Leira,Lilith,Liora,Liriene,Liron,Lucien,Lyra,Maia,Marius,Mathieu,Maylea,Meira,Mireille,Mireya,Natania,Neirin,Nerys,Nuriel,Nyfain,Nyssa,Oisin,Oleisa,Oralie,Orinthea,Orion,Orpheus,Ozara,Peregrine,Persephone,Perseus,Petronela,Phelan,Pryderi,Pyralia,Pyralis,Qadira,Quinevere,Quintessa,Raisa,Remus,Renfrew,Rhyan,Rhydderch,Riona,Saira,Saoirse,Sarai,Sarielle,Sebastian,Seraphim,Seraphina,Serian,Sirius,Sorcha,Severin,Tavish,Tearlach,Terra,Thalia,Thaniel,Theia,Torian,Torin,Tressa,Tristana,Ulyssia,Uriela,Urien,Vanora,Vasilis,Vespera,Xanthus,Xara,Xylia,Yadira,Yakira,Yeira,Yeriel,Yestin,Yseult,Zaira,Zaniel,Zarek,Zephyr,Zora,Zorion".Split(','), 3);

            lblTitle.Alignment = GUIAlignment.HCenter | GUIAlignment.VCenter;
            txtPlayerName.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.Next());

            Children.Add(pnlMain);
            Children.Add(pnlTitle);
            pnlTitle.Children.Add(lblTitle);
            pnlMain.Children.Add(lblPlayerName);
            pnlMain.Children.Add(txtPlayerName);
            pnlMain.Children.Add(btnRandomName);
            pnlMain.Children.Add(btnCreatePlayer);

            focusControl = pnlMain;

            pnlMain.ActivateFirst();

            XnaGUIManager.ActivateNext();
        }

        void btnCreatePlayer_Clicked(XGControl sender)
        {
            //Game.World = m_world;
        }

        void btnRandomName_Clicked(XGControl sender)
        {
            txtPlayerName.Text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.Next());
        }

        public override void Update(GameTime gameTime)
        {
            //int seed = 0;

            m_viewport = Game.GraphicsDevice.Viewport;

            Rectangle                = new Rectangle(0, 0, m_viewport.Width, m_viewport.Height);
            pnlMain.Rectangle        = new Rectangle((Rectangle.Width / 2) - 385, (Rectangle.Height / 2) - 205, 770, 420);
            pnlTitle.Rectangle       = new Rectangle((Rectangle.Width / 2) - 275, (Rectangle.Height / 2) - 225, 550, 40);
            lblTitle.Rectangle       = new Rectangle(0, 0, pnlTitle.Rectangle.Width, pnlTitle.Rectangle.Height);
            btnCreatePlayer.Rectangle = new Rectangle(pnlMain.Rectangle.Width - 220, pnlMain.Rectangle.Height - 50, 200, 30);
            lblPlayerName.Rectangle   = new Rectangle(pnlMain.Rectangle.Width - 445, 40, 100, 30);
            txtPlayerName.Rectangle   = new Rectangle(pnlMain.Rectangle.Width - 390, 40, 210 , 30);
            btnRandomName.Rectangle  = new Rectangle(pnlMain.Rectangle.Width - 170, 40, 150, 30);

            //m_world.Name = txtWorldName.Text;

            base.Update(gameTime);
        }
    }
}
