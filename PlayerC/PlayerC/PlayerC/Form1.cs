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

        private void player_KeyPressEvent(object sender, AxWMPLib._WMPOCXEvents_KeyPressEvent e)
        {
            if(e.nKeyAscii == 102 & player.fullScreen == true)
            {
                player.fullScreen = false;
            }
            else if(e.nKeyAscii == 102 & player.fullScreen == false)
            {
                player.fullScreen = true;
            }

            if(tocando & e.nKeyAscii == 107)
            {
                player.Ctlcontrols.pause();
                tocando = !tocando;
            }
            else if(!tocando & e.nKeyAscii == 107)
            {
                player.Ctlcontrols.play();
                tocando = !tocando;
            }

            if(e.nKeyAscii == 97)
            {
                player.Ctlcontrols.previous();
            }
            if(e.nKeyAscii == 100)
            {
                player.Ctlcontrols.next();
            }

            if(e.nKeyAscii == 48)
            {
                MessageBox.Show(player.Ctlcontrols.currentPosition.ToString());
            }
        }

        private void player_KeyDownEvent(object sender, AxWMPLib._WMPOCXEvents_KeyDownEvent e)
        {
            if(tocando & e.nKeyCode == 74)
            {
                double t = ((player.currentMedia.duration/100)*1);
                player.Ctlcontrols.currentPosition -= t;
            }

            if(tocando & e.nKeyCode == 76)
            {
                double t = ((player.currentMedia.duration / 100) * 1);
                player.Ctlcontrols.currentPosition += t;
            }
        }
    }
}
