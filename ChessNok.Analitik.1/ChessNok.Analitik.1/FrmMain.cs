using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ChessNok.Analitik._1
{
    public partial class FrmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string sql = "";

        const string AppDataFolderName = "chess";
        const string RibbonLayoutFileName = "Ribbon.xml";
        const string GvMainLayoutFileName = "GvMain.xml";
        const string GvGameLayoutFileName = "GvGame.xml";
        public string AppUserDataFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataFolderName);
            }
        }
        public FrmMain()
        {
            InitializeComponent();
            am.DB.DBManager.Instance.Init("Server=ProgerX.svr.vc;Database=ChessL;User ID=chess;Password=Mizer160378;");
            if (!am.BL.G.CheckDB())
                MessageBox.Show("Error DB");
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            BarButtonItem1_ItemClick(null, null);

            GetFormSettings();
            GetRibbonLayout();

            GetGvMainLayout();
        }
        private void BarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (sql == "Analitics_Game") SaveGvGameLayout();

            gridView1.Columns.Clear();
            sql = "Analitics_Main";
            gridControl1.DataSource = am.BL.G.db_select(sql);
            gridView1.BestFitColumns(true);

            GetGvMainLayout();
        }
        private void BtnGame_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(sql == "Analitics_Main") SaveGvMaimLayout();

            gridView1.Columns.Clear();
            sql = "Analitics_Game";
            gridControl1.DataSource = am.BL.G.db_select(sql);
            gridView1.BestFitColumns(true);

            GetGvGameLayout();
        }
        private void BtnCoinHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (sql == "Analitics_Game") SaveGvGameLayout();
            if (sql == "Analitics_Main") SaveGvMaimLayout();

            gridView1.Columns.Clear();
            sql = "Analitics_Coin";
            gridControl1.DataSource = am.BL.G.db_select(sql);
            gridView1.BestFitColumns(true);

            //GetGvCoinLayout();
        }
        void GetFormSettings()
        {
            this.WindowState = (FormWindowState)Properties.Settings.Default.WindowState;
            if((int)WindowState < 2)
                if (Properties.Settings.Default.FrmMainSize.Width > 100 
                    && Properties.Settings.Default.FrmMainSize.Height > 100)
                {
                    if (Properties.Settings.Default.FrmMainLocation.X > 0 && Properties.Settings.Default.FrmMainLocation.Y > 0)
                        this.Location = Properties.Settings.Default.FrmMainLocation;

                    this.Size = Properties.Settings.Default.FrmMainSize;
                }
            splitContainerControl1.SplitterPosition = Properties.Settings.Default.Splitter;
        }
        void GetRibbonLayout()
        {
            string f = Path.Combine(AppUserDataFolder, RibbonLayoutFileName);
            if (File.Exists(f))
                ribbonControl1.RestoreLayoutFromXml(f);
        }
        void GetGvMainLayout()
        {
            string f = Path.Combine(AppUserDataFolder, GvMainLayoutFileName);
            if (File.Exists(f))
                gridView1.RestoreLayoutFromXml(f);
        }
        void GetGvGameLayout()
        {
            string f = Path.Combine(AppUserDataFolder, GvGameLayoutFileName);
            if (File.Exists(f))
                gridView1.RestoreLayoutFromXml(f);
        }
        void SaveFormSettings()
        {
            Properties.Settings.Default.FrmMainLocation = this.Location;
            Properties.Settings.Default.FrmMainSize = this.Size;
            Properties.Settings.Default.Splitter = this.splitContainerControl1.SplitterPosition;
            Properties.Settings.Default.WindowState = (int)this.WindowState;

            Properties.Settings.Default.Save();
        }
        void SaveRibbonLayout()
        {
            string f = Path.Combine(AppUserDataFolder, RibbonLayoutFileName);
            if (!Directory.Exists(AppUserDataFolder)) Directory.CreateDirectory(AppUserDataFolder);
            if (!File.Exists(f)) File.Create(f).Close();
            ribbonControl1.SaveLayoutToXml(f);
        }
        void SaveGvMaimLayout()
        {
            string f = Path.Combine(AppUserDataFolder, GvMainLayoutFileName);
            if (!Directory.Exists(AppUserDataFolder)) Directory.CreateDirectory(AppUserDataFolder);
            if (!File.Exists(f)) File.Create(f).Close();
            gridView1.SaveLayoutToXml(f);
        }
        void SaveGvGameLayout()
        {
            string f = Path.Combine(AppUserDataFolder, GvGameLayoutFileName);
            if (!Directory.Exists(AppUserDataFolder)) Directory.CreateDirectory(AppUserDataFolder);
            if (!File.Exists(f)) File.Create(f).Close();
            gridView1.SaveLayoutToXml(f);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFormSettings();
            SaveRibbonLayout();

            if (sql == "Analitics_Game") SaveGvGameLayout();
            if (sql == "Analitics_Main") SaveGvMaimLayout();
        }

        private void GridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (sql == "Analitics_Game")
            {
                DataRowView r = (DataRowView)gridView1.GetRow(gridView1.FocusedRowHandle);
                int Game_ID = am.BL.G._I(r["ID"]);
                DataTable dt = am.BL.G.db_select("select * from GMove where Game_ID = {1} order by 1 desc", Game_ID);
                gridControl2.DataSource = dt;

                gridView2.Columns["ID"].Visible = false;
            }
        }
    }
}
