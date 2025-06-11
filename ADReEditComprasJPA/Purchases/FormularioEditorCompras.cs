using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using PrimaveraSDK;
using PRISDK100;
using System;
using System.Windows.Forms;

namespace ADReEditComprasJPA.Purchases
{
    public partial class FormularioEditorCompras : CustomForm
    {
        private bool controlsInitialized;

        public FormularioEditorCompras()
        {
            InitializeComponent();
        }

        private void f41_Load(object sender, EventArgs e)
        {
            PriSDKContext.Initialize(BSO, PSO);
            InitializeSDKControls();
        }

        private void InitializeSDKControls()
        {
            if (!controlsInitialized)
            {
                f41.Inicializa(PriSDKContext.SdkContext);
                controlsInitialized = true;
            }
        }

        private void FormularioEditorCompras_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                f41.Termina();
                controlsInitialized = false;
            }
            catch { }
        }

        // Quando o tipo de documento muda, preenche o ComboBox das séries
        private void f41_TextChange(object sender, F4.TextChangeEventArgs e)
        {
            if (!controlsInitialized || string.IsNullOrWhiteSpace(f41.Text))
                return;

            try
            {
                // Obter a série por defeito
                string seriePorDefeito = BSO.Base.Series.DaSerieDefeito("C", f41.Text);

                string sqlSeries = $@"
            SELECT Serie 
            FROM SeriesCompras 
            WHERE Tipodoc = '{f41.Text.Replace("'", "''")}'";
                var cursorSeries = BSO.Consulta(sqlSeries);

                serie.Items.Clear();
                while (!cursorSeries.NoFim())
                {
                    serie.Items.Add(cursorSeries.DaValor<string>("Serie"));
                    cursorSeries.Seguinte();
                }

                // Selecionar a série por defeito
                if (serie.Items.Count > 0)
                {
                    int idx = serie.Items.IndexOf(seriePorDefeito);
                    serie.SelectedIndex = idx >= 0 ? idx : 0;
                }
                else
                {
                    numerodoc.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar séries: " + ex.Message);
            }
        }







        //  Quando a série muda, calcula o próximo número de documento
        private void serie_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!controlsInitialized || serie.SelectedItem == null || string.IsNullOrWhiteSpace(f41.Text))
                return;

            try
            {
                string serieSelecionada = serie.SelectedItem.ToString().Replace("'", "''");
                string sqlUltimoNum = $@"
            SELECT ISNULL(MAX(numdoc), 0) AS Ultimo
            FROM CabecCompras 
            WHERE Tipodoc = '{f41.Text.Replace("'", "''")}'
              AND Serie   = '{serieSelecionada}'";

                var cursorNum = BSO.Consulta(sqlUltimoNum);
                int ultimo = cursorNum.DaValor<int>("Ultimo");

                numerodoc.Maximum = ultimo + 1;
                numerodoc.Value = ultimo + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao definir número do documento: " + ex.Message);
            }
        }

        private void numerodoc_ValueChanged(object sender, EventArgs e)
        {
            if (!controlsInitialized || string.IsNullOrWhiteSpace(f41.Text) || serie.SelectedItem == null)
                return;

            try
            {
                string tipoDoc = f41.Text.Replace("'", "''");
                string serieSelecionada = serie.SelectedItem.ToString().Replace("'", "''");
                int numDoc = (int)numerodoc.Value;

                string sqlLinhas = $@"
            SELECT 
                L.Artigo,
                L.Descricao,
                L.ItemCod,
                L.ItemDesc,
                L.ClasseID,
	            L.SubEmpID
            FROM 
                CabecCompras C
            INNER JOIN 
                LinhasCompras L ON C.Id = L.IdCabecCompras
            WHERE 
                C.TipoDoc = '{tipoDoc}' AND 
                C.Serie = '{serieSelecionada}' AND 
                C.NumDoc = {numDoc}";

                var cursor = BSO.Consulta(sqlLinhas);

                // Limpar a grid
                GridLinhasArtigos.Rows.Clear();

                // Preencher a grid
                while (!cursor.NoFim())
                {
                    GridLinhasArtigos.Rows.Add(
                        cursor.Valor("Artigo")?.ToString() ?? "",
                        cursor.Valor("Descricao")?.ToString() ?? "",
                        cursor.Valor("ItemCod")?.ToString() ?? "",
                        cursor.Valor("ItemDesc")?.ToString() ?? "",
                        cursor.Valor("ClasseID")?.ToString() ?? "",
                        cursor.Valor("SubEmpID")?.ToString() ?? "" //select * from Geral_SubEmpreitada quando der f4 aparecer lista
                    );

                    cursor.Seguinte();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar linhas do documento: " + ex.Message);
            }
        }

    }
}
