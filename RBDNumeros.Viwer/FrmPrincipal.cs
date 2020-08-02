﻿using Microsoft.Extensions.DependencyInjection;
using RBDNumeros.Domain.Interfaces.Repositories;
using RBDNumeros.Domain.Interfaces.Services;
using RBDNumeros.Domain.Services;
using RBDNumeros.Infra.Repositories;
using RBDNumeros.Infra.Repositories.Transactions;
using RBDNumeros.Viwer.Formulario.Barra;
using RBDNumeros.Viwer.Formulario.Configuracao;
using RBDNumeros.Viwer.Formulario.Tecnico;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RBDNumeros.Viwer
{
    public partial class FrmPrincipal : MetroFramework.Forms.MetroForm

    {
        int glb_PosicaoBotaoBase1, glb_PosicaoBotaoBase2, glb_PosicaoBotaoBase3;
        int glb_TopCadastro, glb_TopMovimentacao, glb_TopRelatorio, glb_TopConfiguracao;
        bool glb_HideMenu;

        private IServiceTicket _serviceTicket;
        private IUnitOfWork _unitOfWork;
        frmBarraProgresso frmBarra = new frmBarraProgresso();
        void ConsultarDepedencias()
        {
            _serviceTicket = (IServiceTicket)Program.ServiceProvider.GetService(typeof(IServiceTicket));
            _unitOfWork = (IUnitOfWork)Program.ServiceProvider.GetService(typeof(IUnitOfWork));
        }

        public FrmPrincipal()
        {
            ConsultarDepedencias();
            InitializeComponent();

            glb_PosicaoBotaoBase1 = pnCadastro.Top;
            glb_PosicaoBotaoBase2 = btnMovimentacao.Height;
            glb_PosicaoBotaoBase3 = btnRelatorio.Height;

            glb_TopCadastro = pnCadastro.Top;
            glb_TopMovimentacao = pnMovimentacao.Top;
            glb_TopRelatorio = pnRelatorio.Top;
            glb_TopConfiguracao = pnConfiguracao.Top;

            HideAllMenu();
            CarregarGrafico();

            this.StyleManager = metroStyleManager1;
            pnMenu.Left = -272;
            
            glb_HideMenu = true;
            
        }


        public void HideAllMenu()
        {
            pnCadastro.Visible = false;
            pnMovimentacao.Visible = false;
            pnRelatorio.Visible = false;
            pnConfiguracao.Visible = false;
            btnMovimentacao.Top = glb_PosicaoBotaoBase1;
            btnRelatorio.Top = glb_PosicaoBotaoBase1 + glb_PosicaoBotaoBase2;
            btnConfiguracao.Top = glb_PosicaoBotaoBase1 + glb_PosicaoBotaoBase2 + glb_PosicaoBotaoBase3;

        }

        public void ShowSubMenu(Panel Painel)
        {
            HideAllMenu();
            if (Painel.Name == "pnCadastro")
            {
                btnMovimentacao.Top = btnMovimentacao.Top + pnCadastro.Height;
                btnRelatorio.Top = btnRelatorio.Top + pnCadastro.Height;
                btnConfiguracao.Top = btnConfiguracao.Top + pnCadastro.Height;
                Painel.Top = glb_TopCadastro;
                Painel.Visible = true;
            }
            if (Painel.Name == "pnMovimentacao")
            {
                btnRelatorio.Top = btnRelatorio.Top + pnMovimentacao.Height;
                btnConfiguracao.Top = btnConfiguracao.Top + pnMovimentacao.Height;
                Painel.Top = glb_TopMovimentacao - pnCadastro.Height;
                Painel.Visible = true;
            }
            if (Painel.Name == "pnRelatorio")
            {
                btnConfiguracao.Top = btnConfiguracao.Top + pnRelatorio.Height;
                Painel.Top = glb_TopRelatorio - pnMovimentacao.Height - pnCadastro.Height;
                Painel.Visible = true;
            }
            if (Painel.Name == "pnConfiguracao")
            {
                Painel.Top = glb_TopConfiguracao - pnRelatorio.Height - pnMovimentacao.Height - pnCadastro.Height + 5;
                Painel.Visible = true;
            }

        }


        public  void run()
        {
            frmBarra.CarregaBarraProgresso(0, "");
            while (true)
            {
                frmBarra.CarregaBarraProgresso(_serviceTicket.RetornaPorcentagem(), _serviceTicket.RetornaProcesso());                 
                Thread.Sleep(400);
            }        
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            if(_serviceTicket.ImportarCsv(this.openFileCsv.FileName) > 0)
            {
                frmBarra.CarregaBarraProgresso(90, "Finalizando Commit. Aguarde...");
                _unitOfWork.SaveChanges();
                frmBarra.CarregaBarraProgresso(100, "Importação Realizada com sucesso!");
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {
            tmMenu.Start();
            HideAllMenu();
        }

        private void btnConfPlanilha_Click(object sender, EventArgs e)
        {
            var frmConf = new frmConfiguracaoExcel();
            //frmConf.MdiParent = this;
           // frmConf.Show();
            frmConf.ShowInTaskbar = false;
            frmConf.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = frmConf.ShowDialog(FrmPrincipal.ActiveForm);
        }

        private void btnCadImportarPlanilha_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileCsv.ShowDialog();
                Thread t1 = new Thread(new ThreadStart(run));
                t1.Name = "Secundária";

                backgroundWorker1.RunWorkerAsync(); //Chamo em uma theread separada importação
                t1.Start(); // Inicio theread que vai alimentar barra de progresso.

                frmBarra.ShowInTaskbar = false;
                frmBarra.StartPosition = FormStartPosition.CenterParent;
                DialogResult result = frmBarra.ShowDialog(FrmPrincipal.ActiveForm); //Para que funcione, é nessario exibir em show modal, por este motivo só deixo ele na theread principal.

                t1.Abort();
                if (result == DialogResult.OK)
                {
                    MessageBox.Show("Importação Realizada com sucesso!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                if (result == DialogResult.Cancel)
                {
                    _serviceTicket.CancelarImportacao();
                    MessageBox.Show("Operação Cancelada!", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro ao realizar importação." + ex);
            }
        }

        private void btnCadCliente_Click(object sender, EventArgs e)
        {

        }

        private void btnCadTecnicos_Click(object sender, EventArgs e)
        {
            var frmTecnico = new frmTecnico();

            AbrirFormulario<frmTecnico>();
        }

        private void tmMenu_Tick(object sender, EventArgs e)
        {
            if (glb_HideMenu)
            {
                pnMenu.Left += 16;
                if (pnMenu.Left == 0)
                {
                    glb_HideMenu = false;
                    this.Refresh();
                    tmMenu.Stop();
                }
            }
            else
            {
                pnMenu.Left -= 16;
                if (pnMenu.Left == -272)
                {
                    glb_HideMenu = true;
                    this.Refresh();
                    tmMenu.Stop();
                    
                }
            }
        }

        private void btnTempoSla_Click(object sender, EventArgs e)
        {
            var frmTempoSLA = new frmTempoSla();

            AbrirFormulario<frmTempoSla>();
        }

        private void btnMovDesempenho_Click(object sender, EventArgs e)
        {

        }

        private void btnRelTecnico_Click(object sender, EventArgs e)
        {

        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnCadastro);
        }

        private void btnMovimentacao_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnMovimentacao);
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnRelatorio);
        }

        private void btnConfiguracao_Click(object sender, EventArgs e)
        {
            ShowSubMenu(pnConfiguracao);
        }

        //      private void AbrirFormulario<MiForm>() where MiForm : Form, new()
        private void AbrirFormulario<teste>() where teste : Form, new()
        {
            Form frmForm = new teste();
            //frmConf.MdiParent = this;
            // frmConf.Show();
            frmForm.ShowInTaskbar = false;
            frmForm.StartPosition = FormStartPosition.CenterParent;
            DialogResult result = frmForm.ShowDialog(FrmPrincipal.ActiveForm);
        }
        
        private void CarregarGrafico()
        {
           int CarteiraA = _serviceTicket.RetorneNumerosCarteira(0);
           int CarteiraB = _serviceTicket.RetorneNumerosCarteira(1);
           int CarteiraC = _serviceTicket.RetorneNumerosCarteira(2);
           int CarteiraD = _serviceTicket.RetorneNumerosCarteira(3);

           string[] series = { "Carteira A", "Carteira B", "Carteira C", "Carteira D" };
           int[] pontos = { CarteiraA, CarteiraB, CarteiraC, CarteiraD };

            chart1.Series.Clear();
            chart1.Series.Add("Carteiras");


            for (int i = 0; i < series.Length; i++)
            {

                chart1.Series["Carteiras"].Points.AddXY(series[i], pontos[i]);
                    


            }


        }
    }
}
