using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace PlayerC
{
    public partial class Form1 : Form
    {
        bool tocando = false;
        public Form1()
        {
            InitializeComponent();
            Form f = Form1.ActiveForm;
            
        }

        WMPLib.IWMPPlaylist playList;
        
        private void BtnAbrir_Click(object sender, EventArgs e)
        {
            ofdAbrir.Title = "Abrir mídia";
            ofdAbrir.Filter = "Arquivo mp4|*.mp4|Arquvio mp3|*.mp3";
           if (ofdAbrir.ShowDialog() == DialogResult.OK )
            {
                playList = player.playlistCollection.newPlaylist("Lista");

                foreach (var arquivo in ofdAbrir.FileNames)
                {
                    playList.appendItem(player.newMedia(arquivo));
                    lstPlayList.Items.Add(arquivo);


                    player.currentPlaylist = playList;
                    //player.Ctlcontrols.play();
                }
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if(lstPlayList.Items.Count > 0)
            {
                sfdSalvar.Title = "Salvar PlayList";
                sfdSalvar.Filter = "Arquivo texto|*.txt";
                if(sfdSalvar.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter arquivo = new StreamWriter(sfdSalvar.FileName, false);
                    for (int i = 0; i < lstPlayList.Items.Count ; i++)
                    {
                        arquivo.WriteLine(lstPlayList.Items[i].ToString());
                    }
                    arquivo.Close();
                }
                
            }
            

        }

        private void BtnCarregar_Click(object sender, EventArgs e)
        {
            ofdAbrir.Title = "Abrir PlayList";
            ofdAbrir.Filter = "Arquivo texto|*.txt";
            ofdAbrir.Multiselect = false;
            if (ofdAbrir.ShowDialog() == DialogResult.OK)
            {
                StreamReader arquivo = new StreamReader(ofdAbrir.FileName);
                while (arquivo.Peek() != -1)
                {
                    lstPlayList.Items.Add(arquivo.ReadLine());
                }
                arquivo.Close();
            }
        }

        private void LstPlayList_DoubleClick(object sender, EventArgs e)
        {
            if (lstPlayList.Items.Count > 0)
            {
                player.URL = lstPlayList.SelectedItem.ToString();
                player.Ctlcontrols.play();
                tocando = true;
            }
        }

        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            lstPlayList.Items.Clear();
        }

        /*private void fullScreen(object o, KeyPressEventArgs b)
        {
            
            if (b.KeyChar == (char)Keys.F & player.fullScreen == true)
            {
                o.fullScreen = false;
            }
            else if (b.KeyChar == (char)Keys.F & player.fullScreen == false)
            {
                o.fullScreen=true;
            }
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            int selecionado=lstPlayList.SelectedIndex;
            lstPlayList.Items.RemoveAt(selecionado);
        }

        private void player_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            double t = ((player.currentMedia.duration / 100) * 1);
            if (tocando)
            {
                switch (e.nKeyCode)
                {
                    case 74:
                        player.Ctlcontrols.currentPosition -= t;
                        break;

                    case 76:
                        player.Ctlcontrols.currentPosition += t;
                        break;

                    case 65:
                        player.Ctlcontrols.previous();
                        break;

                    case 68:
                        player.Ctlcontrols.next();
                        break;

                    case 75:
                        tocando = !tocando;
                        break;

                    case 70:
                        player.fullScreen = !player.fullScreen;
                        break;
                }
            }
        }
    }
}
