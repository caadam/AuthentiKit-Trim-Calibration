﻿using AuthentiKitTrimCalibration.ViewModel;
using System;
using System.Windows.Forms;


namespace ATC_Windows_Forms_App.Controls
{
    public partial class EncoderAxisControl : UserControl
    {
        private BindingSource MappingBindingSource;
        public EncoderAxisControl()
        {
            InitializeComponent();
        }
        public void LoadFormData(ref MainViewModel viewModel, ref BindingSource mappingBindingSource)
        {
            MappingBindingSource = mappingBindingSource;

            // Input A
            cbInputA.DataSource = viewModel.InputChannelsA;
            cbInputA.DisplayMember = "Name";
            cbInputA.ValueMember = "Hash";
            cbInputA.DataBindings.Add("SelectedValue", MappingBindingSource, "InputChannelAHash");

            // Input B
            cbInputB.DataSource = viewModel.InputChannelsB;
            cbInputB.DisplayMember = "Name";
            cbInputB.ValueMember = "Hash";
            cbInputB.DataBindings.Add("SelectedValue", MappingBindingSource, "InputChannelBHash");

            // Output
            cbOutputAxis.DataSource = viewModel.OutputAxes;
            cbOutputAxis.DisplayMember = "Name";
            cbOutputAxis.ValueMember = "Hash";
            cbOutputAxis.DataBindings.Add("SelectedValue", MappingBindingSource, "OutputAxisHash");

            // Encoder Pulses per Revolution
            tbEncoderPPR.DataBindings.Add("Text", MappingBindingSource, "EncoderPPR");
                
            // Encoder Pulses per Revolution
            tbRevsInPerRevsOut.DataBindings.Add("Text", MappingBindingSource, "RevsInPerRevsOut");

            // Panel Activation
            pnlEncoderAxisConfig.DataBindings.Add("Enabled", MappingBindingSource, "Deactivated");

        }


        /*
         * The following methods only exist to trigger an update of the data binding without having to
         * select another control. The main way that data is updated in the mapping is via the databindings
         * specified in the LoadFormData above.
         */

        private void cbInputA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbInputA.Focused)
            {
                foreach (Binding b in cbInputA.DataBindings)
                {
                    b.WriteValue();
                }
            }
        }

        private void cbInputB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbInputB.Focused)
            {
                foreach (Binding b in cbInputB.DataBindings)
                {
                    b.WriteValue();
                }
            }
        }

        private void cbOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOutputAxis.Focused)
            {
                foreach (Binding b in cbOutputAxis.DataBindings)
                {
                    b.WriteValue();
                }
            }
        }
        private void tbEncoderPPR_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbEncoderPPR.Focused)
            {
                foreach (Binding b in tbEncoderPPR.DataBindings)
                {
                    b.WriteValue();
                }
                tbEncoderPPR.Select(Right, 0);
            }
        }

        private void tbEncoderPPR_Click(object sender, EventArgs e)
        {
            if (tbEncoderPPR.Focused)
            {
                foreach (Binding b in tbEncoderPPR.DataBindings)
                {
                    b.WriteValue();
                }
            }
        }

        private void tbRevsInPerRevsOut_TextChanged(object sender, EventArgs e)
        {
            if (tbRevsInPerRevsOut.Focused)
            {
                foreach (Binding b in tbRevsInPerRevsOut.DataBindings)
                {
                    b.WriteValue();
                }
                tbRevsInPerRevsOut.SelectionStart = tbRevsInPerRevsOut.Text.Length;
                tbRevsInPerRevsOut.SelectionLength = 0;
            }
        }

        private void DetectButton_Click(object sender, EventArgs e)
        {
            try
            {
                DetectButton1.Text = "Listening...";
                cbInputA.Enabled = false;
                pnlEncoderAxisConfig.Refresh();
                if (MappingBindingSource.Current is MappingViewModel mappingViewModel
                    && mappingViewModel.Deactivated)
                {
                    mappingViewModel.DetectInputA();
                }
                DetectButton1.Text = "Detect";
                cbInputA.Enabled = true;
                pnlEncoderAxisConfig.Refresh();
            }
            catch (Exception ex)
            {
                DetectButton1.Text = "Detect";
                cbInputA.Enabled = true;
                pnlEncoderAxisConfig.Refresh();
                MessageBox.Show(ex.Message, "Error detecting input",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DetectButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DetectButton2.Text = "Listening...";
                cbInputB.Enabled = false;
                pnlEncoderAxisConfig.Refresh();
                if (MappingBindingSource.Current is MappingViewModel mappingViewModel
                    && mappingViewModel.Deactivated)
                {
                    mappingViewModel.DetectInputB();
                }
                DetectButton2.Text = "Detect";
                cbInputB.Enabled = true;
                pnlEncoderAxisConfig.Refresh();
            }
            catch (Exception ex)
            {
                DetectButton2.Text = "Detect";
                cbInputB.Enabled = true;
                pnlEncoderAxisConfig.Refresh();
                MessageBox.Show(ex.Message, "Error detecting input",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
