using PRISDK100;

namespace ADReEditComprasJPA.Purchases
{
    partial class FormularioEditorCompras
    {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormularioEditorCompras));
            this.f41 = new PRISDK100.F4();
            this.numerodoc = new System.Windows.Forms.NumericUpDown();
            this.serie = new System.Windows.Forms.ComboBox();
            this.GridLinhasArtigos = new System.Windows.Forms.DataGridView();
            this.Artigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Projeto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Especialidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.salvarToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.novoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numerodoc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridLinhasArtigos)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // f41
            // 
            this.f41.AgrupaOutrosTerceiros = false;
            this.f41.Audit = "mnuTabDocumentosCompra";
            this.f41.AutoComplete = false;
            this.f41.BackColorLocked = System.Drawing.SystemColors.ButtonFace;
            this.f41.CampoChave = "Documento";
            this.f41.CampoChaveFisica = "";
            this.f41.CampoDescricao = "Descricao";
            this.f41.Caption = "Documento:";
            this.f41.CarregarValoresEdicao = false;
            this.f41.Categoria = PRISDK100.clsSDKTypes.EnumCategoria.DocumentosCompra;
            this.f41.ChaveFisica = "";
            this.f41.ChaveNumerica = false;
            this.f41.F4Modal = false;
            this.f41.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.f41.IDCategoria = "DocumentosCompra";
            this.f41.Location = new System.Drawing.Point(25, 38);
            this.f41.MaxLengthDescricao = 0;
            this.f41.MaxLengthF4 = 50;
            this.f41.MinimumSize = new System.Drawing.Size(37, 21);
            this.f41.Modulo = "CMP";
            this.f41.MostraDescricao = true;
            this.f41.MostraLink = true;
            this.f41.Name = "f41";
            this.f41.PainesInformacaoRelacionada = false;
            this.f41.PainesInformacaoRelacionadaMultiplasChaves = false;
            this.f41.PermiteDrillDown = true;
            this.f41.PermiteEnabledLink = true;
            this.f41.PodeEditarDescricao = false;
            this.f41.ResourceID = 154;
            this.f41.ResourcePersonalizada = false;
            this.f41.Restricao = "";
            this.f41.SelectionFormula = "";
            this.f41.Size = new System.Drawing.Size(492, 22);
            this.f41.TabIndex = 0;
            this.f41.TextoDescricao = "";
            this.f41.WidthEspacamento = 60;
            this.f41.WidthF4 = 1590;
            this.f41.WidthLink = 1575;
            this.f41.TextChange += new PRISDK100.F4.TextChangeHandler(this.f41_TextChange);
            this.f41.Load += new System.EventHandler(this.f41_Load);
            // 
            // numerodoc
            // 
            this.numerodoc.Location = new System.Drawing.Point(617, 38);
            this.numerodoc.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numerodoc.Name = "numerodoc";
            this.numerodoc.Size = new System.Drawing.Size(65, 20);
            this.numerodoc.TabIndex = 1;
            this.numerodoc.ValueChanged += new System.EventHandler(this.numerodoc_ValueChanged);
            // 
            // serie
            // 
            this.serie.FormattingEnabled = true;
            this.serie.Location = new System.Drawing.Point(523, 38);
            this.serie.Name = "serie";
            this.serie.Size = new System.Drawing.Size(88, 21);
            this.serie.TabIndex = 2;
            this.serie.SelectedIndexChanged += new System.EventHandler(this.serie_SelectedIndexChanged);
            // 
            // GridLinhasArtigos
            // 
            this.GridLinhasArtigos.AllowUserToAddRows = false;
            this.GridLinhasArtigos.AllowUserToDeleteRows = false;
            this.GridLinhasArtigos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GridLinhasArtigos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridLinhasArtigos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridLinhasArtigos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridLinhasArtigos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Artigo,
            this.Descricao,
            this.Projeto,
            this.Item,
            this.ItemCod,
            this.ItemDesc,
            this.Classe,
            this.Especialidade});
            this.GridLinhasArtigos.Location = new System.Drawing.Point(25, 66);
            this.GridLinhasArtigos.Name = "GridLinhasArtigos";
            this.GridLinhasArtigos.Size = new System.Drawing.Size(657, 284);
            this.GridLinhasArtigos.TabIndex = 3;
            this.GridLinhasArtigos.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridLinhasArtigos_CellValueChanged);
            this.GridLinhasArtigos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridLinhasArtigos_KeyDown);
            // 
            // Artigo
            // 
            this.Artigo.HeaderText = "Artigo";
            this.Artigo.Name = "Artigo";
            this.Artigo.ReadOnly = true;
            // 
            // Descricao
            // 
            this.Descricao.HeaderText = "Descrição";
            this.Descricao.Name = "Descricao";
            this.Descricao.ReadOnly = true;
            // 
            // Projeto
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.Projeto.DefaultCellStyle = dataGridViewCellStyle1;
            this.Projeto.HeaderText = "Projeto";
            this.Projeto.Name = "Projeto";
            // 
            // Item
            // 
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            // 
            // ItemCod
            // 
            this.ItemCod.HeaderText = "Item Cód.";
            this.ItemCod.Name = "ItemCod";
            // 
            // ItemDesc
            // 
            this.ItemDesc.HeaderText = "Item Desc.";
            this.ItemDesc.Name = "ItemDesc";
            // 
            // Classe
            // 
            this.Classe.HeaderText = "Classe de Atividade";
            this.Classe.Name = "Classe";
            // 
            // Especialidade
            // 
            this.Especialidade.HeaderText = "Especialidade";
            this.Especialidade.Name = "Especialidade";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salvarToolStripButton,
            this.novoToolStripButton,
            this.toolStripSeparator});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(708, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // salvarToolStripButton
            // 
            this.salvarToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("salvarToolStripButton.Image")));
            this.salvarToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.salvarToolStripButton.Name = "salvarToolStripButton";
            this.salvarToolStripButton.Size = new System.Drawing.Size(69, 22);
            this.salvarToolStripButton.Text = "&Guardar";
            this.salvarToolStripButton.Click += new System.EventHandler(this.salvarToolStripButton_Click);
            // 
            // novoToolStripButton
            // 
            this.novoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("novoToolStripButton.Image")));
            this.novoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.novoToolStripButton.Name = "novoToolStripButton";
            this.novoToolStripButton.Size = new System.Drawing.Size(56, 22);
            this.novoToolStripButton.Text = "&Novo";
            this.novoToolStripButton.Click += new System.EventHandler(this.novoToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // FormularioEditorCompras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.GridLinhasArtigos);
            this.Controls.Add(this.serie);
            this.Controls.Add(this.numerodoc);
            this.Controls.Add(this.f41);
            this.Name = "FormularioEditorCompras";
            this.Size = new System.Drawing.Size(708, 373);
            this.Text = "Manutenção de Documentos de Compra";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormularioEditorCompras_FormClosed);
            this.Load += new System.EventHandler(this.FormularioEditorCompras_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numerodoc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridLinhasArtigos)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PRISDK100.F4 f41;
        private System.Windows.Forms.NumericUpDown numerodoc;
        private System.Windows.Forms.ComboBox serie;
        private System.Windows.Forms.DataGridView GridLinhasArtigos;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton novoToolStripButton;
        private System.Windows.Forms.ToolStripButton salvarToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Artigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn Projeto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCod;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classe;
        private System.Windows.Forms.DataGridViewTextBoxColumn Especialidade;
    }
}