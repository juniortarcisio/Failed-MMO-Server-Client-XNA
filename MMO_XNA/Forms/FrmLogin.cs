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
    public partial class FrmLogin : Form
    {
        public String account;
        public String password;

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (txtConta.Text.Trim().Length == 0)
            {
                MessageBox.Show("Preencha a conta");
                return;
            }

            //if (txtSenha.Text.Trim().Length == 0)
            //{
            //    MessageBox.Show("Preencha a senha");
            //    return;
            //}

            this.account = txtConta.Text;
            this.password = txtSenha.Text;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void FrmLogin_Shown(object sender, EventArgs e)
        {
            txtConta.Focus();
        }

        private void txtConta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                btnEntrar_Click(null, null);
            }
        }

    }
}
