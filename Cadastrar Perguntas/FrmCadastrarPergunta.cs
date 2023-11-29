using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Cadastrar_Perguntas
{
    public partial class frmCadastrarPergunta : Form
    {
        public frmCadastrarPergunta()
        {
            InitializeComponent();
        }

        string conexaoString = 
            "server=localhost;user=root;password=;database=db_quiz;";
        
        public void AlternativaCorreta (long IDcorreto, long perguntaID)
        {
            using (MySqlConnection conexao = new MySqlConnection(conexaoString))
            {
                conexao.Open();
                string insert_tb_resposta = "INSERT INTO tb_resposta (id_alternativa, id_pergunta) VALUE (@id_alternativa, @id_pergunta)";

                using (MySqlCommand comando = new MySqlCommand(insert_tb_resposta, conexao))
                {
                    
                    comando.Parameters.AddWithValue("@id_alternativa", IDcorreto);
                    comando.Parameters.AddWithValue("@id_pergunta", perguntaID);
                    comando.ExecuteNonQuery();

                    IDcorreto = comando.LastInsertedId;
                    
                }
            }
        }



        public long CadastrarAlternativas (string alternativa, long perguntaID)
        {
            long alternativaID = 0;
            using (MySqlConnection conexao = new MySqlConnection(conexaoString))
            {
                conexao.Open ();

                string insert_tb_alternativa = "INSERT INTO tb_alternativa (respostas, id_pergunta) VALUE (@respostas, @id_pergunta)";

                using (MySqlCommand comando = new MySqlCommand(insert_tb_alternativa, conexao))
                {
                    comando.Parameters.AddWithValue("@respostas", alternativa);
                    comando.Parameters.AddWithValue("@id_pergunta", perguntaID);
                    comando.ExecuteNonQuery();

                    alternativaID = comando.LastInsertedId;
                }
            }
            return alternativaID;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            string pergunta = "";
            string alternativaA = "", alternativaB = "", alternativaC = "", alternativaD = "";
            long ultimoID = 0;
            long IDalternativaA = 0, IDalternativaB = 0, IDalternativaC = 0, IDalternativaD = 0;


            pergunta = rtxPergunta.Text;

            alternativaA = txbAlternativaA.Text;
            alternativaB = txbAlternativaB.Text;
            alternativaC = txbAlternativaC.Text;
            alternativaD = txbAlternativaD.Text;

            using (MySqlConnection conexao  = new MySqlConnection(conexaoString))
            {
                conexao.Open();
                string scriptInsert = "INSERT INTO tb_perguntas (questao) VALUE (@questao)";

                using (MySqlCommand comando = new MySqlCommand(scriptInsert, conexao))
                {
                    //Substitui os parâmetros para os valores reais.
                    comando.Parameters.AddWithValue("@questao", pergunta);
                    comando.ExecuteNonQuery();

                    ultimoID = comando.LastInsertedId;
                }
            }

            IDalternativaA = CadastrarAlternativas(alternativaA, ultimoID);
            IDalternativaB = CadastrarAlternativas(alternativaB, ultimoID);
            IDalternativaC = CadastrarAlternativas(alternativaC, ultimoID);
            IDalternativaD = CadastrarAlternativas(alternativaD, ultimoID);



            if (rbAlternativaCorretaA.Checked)
            {
                AlternativaCorreta(IDalternativaA, ultimoID);
            }

            if (rbAlternativaCorretaB.Checked)
            {
                AlternativaCorreta(IDalternativaB, ultimoID);
            }

            if (rbAlternativaCorretaC.Checked)
            {
                AlternativaCorreta(IDalternativaC, ultimoID);
            }

            if (rbAlternativaCorretaD.Checked)
            {
                AlternativaCorreta(IDalternativaD, ultimoID);
            }


            //MessageBox.Show("Pergunta cadastrada com sucesso, ID: " + ultimoID.ToString());


        }

        private void btnJogar_Click(object sender, EventArgs e)
        {
            Form consultarPergunta = new FrmConsultarPergunta();


            consultarPergunta.Show();

        }
    }
}
