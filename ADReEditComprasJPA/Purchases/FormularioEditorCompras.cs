using Primavera.Extensibility.BusinessEntities;
using Primavera.Extensibility.CustomForm;
using PrimaveraSDK;
using PRISDK100;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
        }

        private void FormularioEditorCompras_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            try
            {
                //Ensure that resources released.
                f41.Termina();
                controlsInitialized = false;
            }
            catch { }
        }
    }
}
