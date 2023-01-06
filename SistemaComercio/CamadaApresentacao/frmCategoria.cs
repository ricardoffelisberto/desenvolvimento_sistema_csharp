using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CamadaNegocio;

namespace CamadaApresentacao {
    public partial class frmCategoria : Form {
        private bool eNovo = false;
        private bool eEditar = false;

        public frmCategoria() {
            InitializeComponent();
            this.ttMensagem.SetToolTip(this.txtNome, "Insira o nome da categoria.");
        }

        //Mostrar mensagem de confirmação
        private void MensagemOk (string mensagem) {
            MessageBox.Show(mensagem, "Sistema Comércio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Mostrar mensagem de erro
        private void MensagemErro(string mensagem) {
            MessageBox.Show(mensagem, "Sistema Comércio", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Limpar campos
        private void Limpar() {
            this.txtNome.Text = string.Empty;
            this.txtIdCategoria.Text = string.Empty;
            this.txtDescricao.Text = string.Empty;
        }

        //Habilitar os textbox
        private void HabilitarChk (bool valor) {
            this.txtNome.ReadOnly = !valor;
            this.txtIdCategoria.ReadOnly = !valor;
            this.txtDescricao.ReadOnly = !valor;
        }

        //Habilitar os botões
        private void HabilitarButton () {
            if (this.eNovo || this.eEditar) {
                this.HabilitarChk(true);
                this.btnNovo.Enabled = false;
                this.btnEditar.Enabled = false;
                this.btnSalvar.Enabled = true;
                this.btnCancelar.Enabled = true;
            } else {
                this.HabilitarChk(false);
                this.btnNovo.Enabled = true;
                this.btnEditar.Enabled = true;
                this.btnSalvar.Enabled = false;
                this.btnCancelar.Enabled = false;
            }
        }

        //Ocultar colunas grid
        private void ocultarColunas() {
            this.dataLista.Columns[0].Visible = false; 
            this.dataLista.Columns[1].Visible = false;
        }

        //Mostrar no data grid
        private void Mostrar() {
            this.dataLista.DataSource = NCategoria.Mostrar();
            this.ocultarColunas();
            lblTotal.Text = "Total de registros: " + Convert.ToString(dataLista.Rows.Count);
        }

        //Buscar pelo nome
        private void BuscarNome() {
            this.dataLista.DataSource = NCategoria.BuscarNome(this.txtBuscar.Text);
            this.ocultarColunas();
            lblTotal.Text = "Total de registros: " + Convert.ToString(dataLista.Rows.Count);
        }

        private void frmCategoria_Load(object sender, EventArgs e) {
            this.Top = 0;
            this.Left = 0;
            this.Mostrar();
            this.HabilitarChk(false);
            this.HabilitarButton();
        }

        private void btnBuscar_Click(object sender, EventArgs e) {
            this.BuscarNome();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e) {
            this.BuscarNome();
        }

        private void btnNovo_Click(object sender, EventArgs e) {
            this.eNovo = true;
            this.eEditar = true;
            this.HabilitarButton();
            this.Limpar();
            this.HabilitarChk(true);
            this.txtNome.Focus();
            this.txtIdCategoria.Enabled = false;
        }

        private void btnSalvar_Click(object sender, EventArgs e) {
            try {
                string resp = "";
                if(this.txtNome.Text == string.Empty) {
                    MensagemErro("Preencha todos os campos.");
                    errorIcone.SetError(txtNome, "Insira o nome.");
                } else {
                    if(this.eNovo) {
                        resp = NCategoria.Inserir(this.txtNome.Text.Trim().ToUpper(), 
                            this.txtDescricao.Text.Trim());
                    }else {
                        resp = NCategoria.Editar(Convert.ToInt32(this.txtIdCategoria.Text), 
                            this.txtNome.Text.Trim().ToUpper(), this.txtDescricao.Text.Trim());
                    }

                    if (resp.Equals("Ok")) {
                        if (this.eNovo) {
                            this.MensagemOk("Registro gravado com sucesso.");
                        }else {
                            this.MensagemOk("Registro editado com sucesso.");
                        }
                    } else {
                        this.MensagemErro(resp);
                    }

                    this.eNovo = false;
                    this.eEditar = false;
                    this.HabilitarButton();
                    this.Limpar();
                    this.Mostrar();
                }
            } catch(Exception ex) {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void dataLista_DoubleClick(object sender, EventArgs e) {
            this.txtIdCategoria.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["idcategoria"].Value);
            this.txtNome.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["nome"].Value);
            this.txtDescricao.Text = Convert.ToString(this.dataLista.CurrentRow.Cells["descricao"].Value);
            this.tabControl1.SelectedIndex = 1;
        }

        private void btnEditar_Click(object sender, EventArgs e) {
            if(this.txtIdCategoria.Text.Equals("")) {
                this.MensagemErro("Selecione um registro pra editar.");
            } else {
                this.eEditar = true;
                this.HabilitarButton();
                this.HabilitarChk(true);
                this.txtIdCategoria.Enabled = false;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) {
            this.eNovo = false;
            this.eEditar = false; 
            this.HabilitarButton();
            this.HabilitarChk(false);
            this.Limpar();
        }

        private void chkDeletar_CheckedChanged(object sender, EventArgs e) {
           if(chkDeletar.Checked) {
                this.dataLista.Columns[0].Visible = true;
            } else {
                this.dataLista.Columns[0].Visible = false;
            }
        }

        private void dataLista_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if(e.ColumnIndex == dataLista.Columns["Deletar"].Index) {
                DataGridViewCheckBoxCell ChkDeletar = (DataGridViewCheckBoxCell)dataLista.Rows[e.RowIndex].Cells["Deletar"];
                ChkDeletar.Value = !Convert.ToBoolean(ChkDeletar.Value);
            }
        }

        private void btnDeletar_Click(object sender, EventArgs e) {
            try {
                DialogResult Opcao;
                Opcao = MessageBox.Show("Deseja realmente apagar os registros", "Sistema Comércio", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if(Opcao == DialogResult.OK) {
                    string Codigo;
                    string Resp = "";

                    foreach(DataGridViewRow Row in dataLista.Rows) {
                        if (Convert.ToBoolean(Row.Cells[0].Value)) {
                            Codigo = Convert.ToString(Row.Cells[1].Value);
                            Resp = NCategoria.Excluir(Convert.ToInt32(Codigo));

                            if (Resp.Equals("Ok")) {
                                this.MensagemOk("Registro excluído com sucesso.");
                                this.chkDeletar.Checked = false;
                            } else {
                                this.MensagemErro(Resp);
                            }
                        }
                    }

                    this.Mostrar();
                }
            }catch(Exception ex) {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
    }
}
