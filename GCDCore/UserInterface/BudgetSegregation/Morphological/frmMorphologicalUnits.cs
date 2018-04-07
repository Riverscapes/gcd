using System;
using System.Windows.Forms;

namespace GCDCore.UserInterface.BudgetSegregation.Morphological
{
    public partial class frmMorphologicalUnits : Form
    {
        public UnitsNet.Units.VolumeUnit VolumeUnit { get; internal set; }
        public UnitsNet.Units.MassUnit MassUnit { get; internal set; }
        public UnitsNet.Units.DurationUnit DurationUnit { get; internal set; }

        public frmMorphologicalUnits(UnitsNet.Units.VolumeUnit vol, UnitsNet.Units.MassUnit mass, UnitsNet.Units.DurationUnit duration)
        {
            InitializeComponent();

            VolumeUnit = vol;
            MassUnit = mass;
            DurationUnit = duration;
        }

        private void frmMorphologicalUnits_Load(object sender, EventArgs e)
        {
            cboVolume.DataSource = GCDUnits.GCDVolumeUnits();
            cboMass.DataSource = GCDUnits.GCDMassUnits();
            cboDuration.DataSource = GCDUnits.GCDDurationUnits();

            GCDUnits.SelectUnit(cboVolume, VolumeUnit);
            GCDUnits.SelectUnit(cboMass, MassUnit);
            GCDUnits.SelectUnit(cboDuration, DurationUnit);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            VolumeUnit = ((GCDUnits.FormattedUnit<UnitsNet.Units.VolumeUnit>)cboVolume.SelectedItem).Unit;
            MassUnit = ((GCDUnits.FormattedUnit<UnitsNet.Units.MassUnit>)cboMass.SelectedItem).Unit;
            DurationUnit = ((GCDUnits.FormattedUnit<UnitsNet.Units.DurationUnit>)cboDuration.SelectedItem).Unit;
        }
    }
}
