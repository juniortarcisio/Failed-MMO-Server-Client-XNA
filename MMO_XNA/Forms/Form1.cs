using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Teste
{
    public partial class Form1 : Form
    {
        public Game1 game;
        public IntPtr getDrawSurface()
        {
            return pictureBox1.Handle;
        }

        public Form1()
        {
            InitializeComponent();
        }

        public void hideThings()
        {
            this.textBox1.Focus();
            this.groupBox1.Visible = false;
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            DialogResult r = frmLogin.ShowDialog();

            if (r == DialogResult.Yes)
            {
                game.Connect(frmLogin.account, frmLogin.password);
            }

            frmLogin.Dispose();
        }

        private void brnSair_Click(object sender, EventArgs e)
        {
            this.Close();
            game.Exit();
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            game.Exit();
            Application.Exit();
        }
        
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.Right) && e.Shift == false)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyCode == Keys.Enter){
                game.SendSay(textBox1.Text);
                textBox1.Text = "";
                e.Handled = true;
                return;
            }

        }

    }
}
