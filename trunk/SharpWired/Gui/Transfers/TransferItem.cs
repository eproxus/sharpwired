﻿using System;
using System.Windows.Forms;
using SharpWired.Gui.Resources.Icons;
using SharpWired.Model.Transfers;

namespace SharpWired.Gui.Transfers {
    public partial class TransferItem : SharpWiredGuiBase {
        private ITransfer transfer;
        public bool Selected { get; set; }
        public Status Status { get { return transfer.Status; } }

        public delegate void ClickedArgs(TransferItem ti, bool control);

        public event ClickedArgs Clicked;

        private bool frozen = true; //Stops repainting a paused/idle transfer

        public TransferItem() {
            InitializeComponent();
        }

        public void Init(ITransfer t) {
            transfer = t;
            var icons = IconHandler.Instance;
            pauseButton.Image = icons.MediaPlaybackPause;
            deleteButton.Image = icons.ProcessStop;
            fileName.Text = t.Source.Name;
            info.Text = "";
            Anchor = AnchorStyles.Left | AnchorStyles.Right;
        }

        private void OnClicked(object sender, EventArgs e) {
            var control = false;

            // TODO: Handle shift selecting.
            if ((ModifierKeys & Keys.Control) == Keys.Control) {
                control = true;
            }

            if (Clicked != null) {
                Clicked(this, control);
            }
        }

        private void pauseButton_Click(object sender, EventArgs e) {
            if (transfer.Status == Status.Idle) {
                //TODO
            } else {
                Controller.FileTransferController.PauseDownload(transfer);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e) {
            pauseButton_Click(sender, e);
            Controller.FileTransferController.RemoveDownload(transfer);
        }

        internal void Repaint() {
            if (transfer.Status == Status.Idle) {
                if (!frozen) {
                    pauseButton.Image = IconHandler.Instance.MediaPlaybackStart;
                    pauseButton.Enabled = false;

                    info.Text = "Paused — " + GuiUtil.FormatByte(transfer.Received)
                                + " of " + GuiUtil.FormatByte(transfer.Size);
                    frozen = true;
                }
            } else {
                progressBar.Value = (int) (transfer.Progress*1000.0);

                string timeLeft;
                var estimateTimeLeft = transfer.EstimatedTimeLeft;
                if (estimateTimeLeft == null) {
                    timeLeft = "∞";
                } else {
                    timeLeft = GuiUtil.FormatTimeSpan((TimeSpan) estimateTimeLeft);
                }

                info.Text = timeLeft + " remaining — " +
                            GuiUtil.FormatByte(transfer.Received) + " of " + GuiUtil.FormatByte(transfer.Size) +
                            " (" + GuiUtil.FormatByte(transfer.Speed) + "/s" + ")";

                frozen = false;
            }
        }
    }
}