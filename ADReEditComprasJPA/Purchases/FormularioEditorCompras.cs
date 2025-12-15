using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using PrimaveraSDK;
using PRISDK100;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            catch
            {

            }
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
                                        O.Codigo AS CodigoObra,
                                         L.ItemId,
                                        L.ItemCod,
                                        L.ItemDesc,
                                        L.ClasseID,
                                        L.SubEmpID
                                    FROM
                                        CabecCompras C
                                    INNER JOIN
                                        LinhasCompras L ON C.Id = L.IdCabecCompras
                                    LEFT JOIN
                                        COP_Obras O ON L.ObraID = O.ID
            WHERE
                C.TipoDoc = '{tipoDoc}' AND
                C.Serie = '{serieSelecionada}' AND
                C.NumDoc = {numDoc}";

                var cursor = BSO.Consulta(sqlLinhas);

                // Limpar a grid
                GridLinhasArtigos.Rows.Clear();

                // Preencher a grid
                int numeroLinha = 1;
                while (!cursor.NoFim())
                {
                    GridLinhasArtigos.Rows.Add(
                        numeroLinha++, // Adiciona o número da linha
                        cursor.Valor("Artigo")?.ToString() ?? "",
                        cursor.Valor("Descricao")?.ToString() ?? "",
                        cursor.Valor("CodigoObra")?.ToString() ?? "",
                        cursor.Valor("ItemId")?.ToString() ?? "",
                        cursor.Valor("ItemCod")?.ToString() ?? "",
                        cursor.Valor("ItemDesc")?.ToString() ?? "",
                        cursor.Valor("ClasseID")?.ToString() ?? "",
                        cursor.Valor("SubEmpID")?.ToString() ?? ""
                    );

                    cursor.Seguinte();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar linhas do documento: " + ex.Message);
            }
        }

        private void GridLinhasArtigos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                var cell = GridLinhasArtigos.CurrentCell;
                if (cell != null)
                {
                    int coluna = cell.ColumnIndex;

                    // Coluna 3: abrir lista (TrataF4Id) - Obra
                    if (coluna == 3)
                    {
                        try
                        {
                            MetodoGetProjectos();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao abrir a lista de projetos: " + ex.Message);
                        }

                        e.Handled = true;
                    }

                    // Coluna 4: abrir lista de itens
                    if (coluna == 4)
                    {
                        try
                        {
                            MetodoGetItem();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao abrir a lista de itens: " + ex.Message);
                        }

                        e.Handled = true;
                    }

                    // Coluna 7: abrir lista de especialidades
                    if (coluna == 7)
                    {
                        try
                        {
                            MetodoGetEspecialidade();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao abrir a lista de especialidades: " + ex.Message);
                        }

                        e.Handled = true;
                    }
                    // Coluna 6: abrir lista de classes
                    else if (coluna == 6)
                    {

                        try
                        {
                            MetodoGetClasse();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao abrir a lista de classes: " + ex.Message);
                        }

                        e.Handled = true;
                    }
                }
            }
        }

        private void MetodoGetEspecialidade()
        {
            Dictionary<string, string> especialidades = new Dictionary<string, string>();

            GetEspecialidades(ref especialidades);

            if (especialidades.Count > 0)
            {
                string codigoespecialidade = especialidades["SubEmp"];

                if (GridLinhasArtigos.CurrentCell != null)
                {
                    int rowIndex = GridLinhasArtigos.CurrentCell.RowIndex;

                    // Coluna 8 recebe o código da especialidade
                    GridLinhasArtigos.Rows[rowIndex].Cells[8].Value = codigoespecialidade;

                }
            }
        }

        private void GetEspecialidades(ref Dictionary<string, string> especialidades)
        {
            string NomeLista = "Sumempreitadas na Obra";
            string Campos = "SubEmp,Descricao,SubEmpId";
            string Tabela = "Geral_SubEmpreitada WITH (NOLOCK)";
            string Where = "";
            string CamposF4 = "SubEmp";
            string orderby = "SubEmp";

            List<string> ResQuery = new List<string>();

            OpenF4List(Campos, Tabela, Where, CamposF4, orderby, NomeLista, this, ref ResQuery);

            if (ResQuery.Count > 0)
            {
                string[] colunas = CamposF4.Split(',');
                for (int i = 0; i < colunas.Length; i++)
                {
                    if (i < ResQuery.Count)
                    {
                        especialidades[colunas[i].Trim()] = ResQuery[i].ToString();
                    }
                }
            }
        }

        private void MetodoGetClasse()
        {
            Dictionary<string, string> classes = new Dictionary<string, string>();

            GetClasses(ref classes);

            if (classes.Count > 0)
            {
                string codigoClasse = classes["ClasseId"];

                if (GridLinhasArtigos.CurrentCell != null)
                {
                    int rowIndex = GridLinhasArtigos.CurrentCell.RowIndex;

                    // Coluna 7 recebe o código da classe
                    GridLinhasArtigos.Rows[rowIndex].Cells[7].Value = codigoClasse;

                }
            }
        }

        private void GetClasses(ref Dictionary<string, string> classes)
        {
            string NomeLista = "Projetos";
            string Campos = "Classe,Descricao,ClasseId";
            string Tabela = "Geral_Classe WITH (NOLOCK)";
            string Where = "";
            string CamposF4 = "ClasseId";
            string orderby = "ClasseId";

            List<string> ResQuery = new List<string>();

            OpenF4List(Campos, Tabela, Where, CamposF4, orderby, NomeLista, this, ref ResQuery);

            if (ResQuery.Count > 0)
            {
                string[] colunas = CamposF4.Split(',');
                for (int i = 0; i < colunas.Length; i++)
                {
                    if (i < ResQuery.Count)
                    {
                        classes[colunas[i].Trim()] = ResQuery[i].ToString();
                    }
                }
            }
        }

        private void MetodoGetItem()
        {
            Dictionary<string, string> items = new Dictionary<string, string>();

            GetItems(ref items);

            if (items.Count > 0)
            {

                string codigoItem = items["ITEMID"]; //
                string ItemCod = items["ItemCod"];
                string ItemDesc = items["ItemDesc"];

                // Atribuir o valor à célula atual
                if (GridLinhasArtigos.CurrentCell != null)
                {
                    int rowIndex = GridLinhasArtigos.CurrentCell.RowIndex;

                    // Coluna 4 (índice 4) recebe ITEMID
                    GridLinhasArtigos.Rows[rowIndex].Cells[4].Value = codigoItem;

                    // Coluna 5 (índice 5) recebe ItemCod
                    GridLinhasArtigos.Rows[rowIndex].Cells[5].Value = ItemCod;

                    // Coluna 6 (índice 6) recebe ItemDesc
                    GridLinhasArtigos.Rows[rowIndex].Cells[6].Value = ItemDesc;
                }
            }
        }

        private void MetodoGetProjectos()
        {
            Dictionary<string, string> projetos = new Dictionary<string, string>();

            GetProjetos(ref projetos);

            if (projetos.Count > 0)
            {
                string codigoProjeto = projetos["Codigo"];

                if (GridLinhasArtigos.CurrentCell != null)
                {
                    int rowIndex = GridLinhasArtigos.CurrentCell.RowIndex;

                    // Coluna 3 recebe o código do projeto
                    GridLinhasArtigos.Rows[rowIndex].Cells[3].Value = codigoProjeto;

                    // Limpa colunas 4, 5 e 6
                    LimparColunas456(rowIndex);
                }
            }
        }

        private void GetItems(ref Dictionary<string, string> items)
        {
            string NomeLista = "Items";
            string CamposF4 = "ITEMID,ItemCod,ItemDesc";

            List<string> ResQuery = new List<string>();

            // Obter o ID da coluna 3 da linha atual (que agora contém o CódigoObra)
            string idProjeto = "";

            if (GridLinhasArtigos.CurrentCell != null)
            {
                int rowIndex = GridLinhasArtigos.CurrentCell.RowIndex;
                object valorCelula = GridLinhasArtigos.Rows[rowIndex].Cells[3].Value; // Coluna 3 para CodigoObra

                idProjeto = valorCelula?.ToString() ?? "";

                // Obter o ID da obra a partir do código da obra
                idProjeto = BSO.Consulta($"SELECT  ID FROM COP_Obras WHERE Codigo = '{idProjeto}'").DaValor<string>("ID");

            }

            if (string.IsNullOrWhiteSpace(idProjeto))
            {
                MessageBox.Show("Código da Obra não encontrado na coluna 3.");
                return;
            }

            OpenF4ListSP(CamposF4, NomeLista, idProjeto, ref ResQuery);

            if (ResQuery.Count > 0)
            {
                string[] colunas = CamposF4.Split(',');
                for (int i = 0; i < colunas.Length; i++)
                {
                    if (i < ResQuery.Count)
                    {
                        items[colunas[i].Trim()] = ResQuery[i].ToString();
                    }
                }
            }
        }

        private void OpenF4ListSP(string camposF4, string nomeLista, string idProjeto, ref List<string> resQuery)
        {
            var strSQL = $"EXEC [COP_GetWorkItems] '{idProjeto.Replace("'", "''")}'";

            Form form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;

            string result = Convert.ToString(PSO.Listas.GetF4SQL(nomeLista, strSQL, camposF4, form));

            if (!string.IsNullOrEmpty(result))
            {
                string[] itemQuery = result.Split('\t');
                resQuery.AddRange(itemQuery);
            }
        }

        private void GetProjetos(ref Dictionary<string, string> projetos)
        {
            string NomeLista = "Projetos";
            string Campos = "Codigo,Titulo,Descricao";
            string Tabela = "COP_Obras WITH (NOLOCK)";
            string Where = "";
            string CamposF4 = "Codigo";
            string orderby = "Codigo";

            List<string> ResQuery = new List<string>();

            OpenF4List(Campos, Tabela, Where, CamposF4, orderby, NomeLista, this, ref ResQuery);

            if (ResQuery.Count > 0)
            {
                string[] colunas = CamposF4.Split(',');
                for (int i = 0; i < colunas.Length; i++)
                {
                    if (i < ResQuery.Count)
                    {
                        projetos[colunas[i].Trim()] = ResQuery[i].ToString();
                    }
                }
            }
        }

        private void OpenF4List(string campos, string tabela, string where, string camposF4, string orderby, string nomeLista, FormularioEditorCompras formularioEditorCompras, ref List<string> resQuery)
        {
            string strSQL = "select distinct " + campos + " FROM " + tabela;

            if (where.Length > 0)
            {
                strSQL += " WHERE " + where;
            }

            strSQL += " Order by " + orderby;
            Form form = new Form();
            form.StartPosition = FormStartPosition.CenterScreen;
            string result = Convert.ToString(PSO.Listas.GetF4SQL(nomeLista, strSQL, camposF4, form));

            if (!string.IsNullOrEmpty(result))
            {
                string[] itemQuery = result.Split('\t');
                resQuery.AddRange(itemQuery);
            }
        }


        private void EstilizarCabecalhosSelecionados()
        {
            // Ativa a personalização de cabeçalhos
            GridLinhasArtigos.EnableHeadersVisualStyles = false;

            // Adiciona a coluna numeradora no início se ainda não existir
            if (!GridLinhasArtigos.Columns.Contains("LinhaNum"))
            {
                var colunaLinha = new DataGridViewTextBoxColumn
                {
                    Name = "LinhaNum",
                    HeaderText = "Linha",
                    Width = 50,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };
                colunaLinha.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                colunaLinha.HeaderCell.Style.ForeColor = Color.White;
                colunaLinha.HeaderCell.Style.BackColor = Color.SteelBlue;
                GridLinhasArtigos.Columns.Insert(0, colunaLinha);
            }

            // Índices das colunas que você quer estilizar: 3, 4, 6, 7, 8 (ajustados após adicionar coluna numeradora)
            int[] colunas = { 3, 4, 6, 7, 8 };

            // Define a cor de fundo e a cor do texto para os cabeçalhos
            GridLinhasArtigos.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            GridLinhasArtigos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            foreach (int indice in colunas)
            {
                if (indice < GridLinhasArtigos.Columns.Count)
                {
                    var coluna = GridLinhasArtigos.Columns[indice];
                    coluna.HeaderCell.Style.ForeColor = Color.White; // Cor do texto
                    coluna.HeaderCell.Style.BackColor = Color.SteelBlue; // Cor de fundo
                    coluna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }


        private void FormularioEditorCompras_Load(object sender, EventArgs e)
        {
            EstilizarCabecalhosSelecionados();
        }

        private void salvarToolStripButton_Click(object sender, EventArgs e)
        {
            var tipoDoc = f41.Text;
            var serieSelecionada = serie.SelectedItem?.ToString();
            var numeroDoc = (int)numerodoc.Value;

            if (string.IsNullOrWhiteSpace(tipoDoc) || string.IsNullOrWhiteSpace(serieSelecionada) || numeroDoc <= 0)
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                return;
            }

            // Obter ID do documento
            var idCabec = BSO.Consulta($@"
        SELECT Id FROM CabecCompras
        WHERE TipoDoc = '{tipoDoc}'
          AND Serie = '{serieSelecionada}'
          AND NumDoc = {numeroDoc}
    ").DaValor<string>("Id");

            if (string.IsNullOrWhiteSpace(idCabec))
            {
                MessageBox.Show("Documento não encontrado.");
                return;
            }

            // Criar um dicionário para mapear artigos a números de linha
            var linhasSQL = BSO.Consulta($@"
        SELECT NumLinha, Artigo
        FROM LinhasCompras
        WHERE IdCabecCompras = '{idCabec}'
        ORDER BY NumLinha
    ");

            if (linhasSQL.Vazia())
            {
                MessageBox.Show("Nenhuma linha encontrada para o documento.");
                return;
            }

            Dictionary<string, int> artigoToNumLinha = new Dictionary<string, int>();
            linhasSQL.Inicio();
            while (!linhasSQL.NoFim())
            {
                var artigo = linhasSQL.DaValor<string>("Artigo");
                var numLinha = linhasSQL.DaValor<int>("NumLinha");
                if (!string.IsNullOrWhiteSpace(artigo))
                {
                    artigoToNumLinha[artigo] = numLinha;
                }
                linhasSQL.Seguinte();
            }

            // Agora iterar pela grid e atualizar cada linha correspondente
            for (int i = 0; i < GridLinhasArtigos.Rows.Count; i++)
            {
                var row = GridLinhasArtigos.Rows[i];
                if (row.IsNewRow) continue;

                var artigo = row.Cells[1].Value?.ToString()?.Trim(); // Artigo é a segunda coluna (índice 1)

                // Pular se não há artigo
                if (string.IsNullOrWhiteSpace(artigo))
                {
                    continue;
                }

                // Encontrar o número da linha correspondente no banco de dados
                if (!artigoToNumLinha.ContainsKey(artigo))
                {
                    continue; // Artigo não encontrado, pular
                }

                var numLinha = artigoToNumLinha[artigo];

                // Obter projeto (pode estar vazio)
                var valorProjeto = row.Cells[3].Value;
                var codigoProjeto = valorProjeto?.ToString()?.Trim() ?? "";

                // Obter ID da obra (pode ser NULL se projeto foi removido)
                string idObra = "NULL";
                if (!string.IsNullOrWhiteSpace(codigoProjeto))
                {
                    var obraResult = BSO.Consulta($@"SELECT ID FROM COP_Obras WHERE Codigo = '{codigoProjeto.Replace("'", "''")}'");
                    if (!obraResult.Vazia())
                    {
                        idObra = $"'{obraResult.DaValor<string>("ID")}'";
                    }
                    else
                    {
                        // Se não encontrar a obra, define como NULL
                        idObra = "NULL";
                    }
                }

                // Obter os campos opcionais
                var IDItem = row.Cells[4].Value?.ToString()?.Trim()?.Replace("'", "''");
                var ItemCod = row.Cells[5].Value?.ToString()?.Trim()?.Replace("'", "''");
                var ItemDesc = row.Cells[6].Value?.ToString()?.Trim()?.Replace("'", "''");

                var valorClasse = row.Cells[7].Value?.ToString()?.Trim();
                var ClasseActividade = string.IsNullOrWhiteSpace(valorClasse) || !int.TryParse(valorClasse, out var classeInt)
                    ? "NULL" : classeInt.ToString();

                var valorSubEmp = row.Cells[8].Value?.ToString()?.Trim();
                var SubEmpreitada = string.IsNullOrWhiteSpace(valorSubEmp) || !int.TryParse(valorSubEmp, out var subEmpInt)
                    ? "NULL" : subEmpInt.ToString();

                // Construir e executar o UPDATE
                var updateQuery = $@"
            UPDATE LinhasCompras
            SET
                ObraID = {idObra},
                ItemId = {(string.IsNullOrWhiteSpace(IDItem) ? "NULL" : $"'{IDItem}'")},
                ItemCod = {(string.IsNullOrWhiteSpace(ItemCod) ? "NULL" : $"'{ItemCod}'")},
                ItemDesc = {(string.IsNullOrWhiteSpace(ItemDesc) ? "NULL" : $"'{ItemDesc}'")},
                ClasseID = {ClasseActividade},
                SubEmpID = {SubEmpreitada}
            WHERE
                IdCabecCompras = '{idCabec}' AND NumLinha = {numLinha}
        ";

                BSO.DSO.ExecuteSQL(updateQuery);
            }

            MessageBox.Show("Documento atualizado com sucesso!");
        }

        private void novoToolStripButton_Click(object sender, EventArgs e)
        {
            LimparFormulario();
        }

        private void LimparFormulario()
        {
            // Limpa o campo F4
            f41.Text = "";

            // Limpa ComboBox de séries
            serie.Items.Clear();
            serie.Text = "";

            // Limpa o número do documento
            numerodoc.Value = 0;
            numerodoc.Maximum = 1; // ou outro valor seguro

            // Limpa a grid
            GridLinhasArtigos.Rows.Clear();
        }

        private void LimparColunas456(int rowIndex)
        {
            GridLinhasArtigos.Rows[rowIndex].Cells[4].Value = string.Empty; // ItemId
            GridLinhasArtigos.Rows[rowIndex].Cells[5].Value = string.Empty; // ItemCod
            GridLinhasArtigos.Rows[rowIndex].Cells[6].Value = string.Empty; // ItemDesc
        }

        private void LimparColunas56(int rowIndex)
        {
            GridLinhasArtigos.Rows[rowIndex].Cells[5].Value = string.Empty; // ItemCod
            GridLinhasArtigos.Rows[rowIndex].Cells[6].Value = string.Empty; // ItemDesc
        }

        private void GridLinhasArtigos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Ignora cabeçalhos

            // Limpar colunas 4,5,6 se coluna 3 (Obra) foi limpa
            if (e.ColumnIndex == 3)
            {
                var valor = GridLinhasArtigos.Rows[e.RowIndex].Cells[3].Value;
                var valorStr = valor?.ToString()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(valorStr))
                {
                    // Define explicitamente como vazio e limpa as colunas dependentes
                    GridLinhasArtigos.Rows[e.RowIndex].Cells[3].Value = string.Empty;
                    LimparColunas456(e.RowIndex);
                }
            }
            // Limpar colunas 5,6 se coluna 4 (ItemId) foi limpa
            else if (e.ColumnIndex == 4)
            {
                var valor = GridLinhasArtigos.Rows[e.RowIndex].Cells[4].Value;
                var valorStr = valor?.ToString()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(valorStr))
                {
                    GridLinhasArtigos.Rows[e.RowIndex].Cells[4].Value = string.Empty;
                    LimparColunas56(e.RowIndex);
                }
            }
        }
    }
}