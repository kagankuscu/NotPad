using NotPad.Core;
using NotPad.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NotPad
{
    public partial class Main : Form
    {
        private Note _Note { get; set; }
        public RepositoryBase<Note> _repository { get; set; }

        public Main()
        {
            InitializeComponent();

            LoadValues();
        }

        private void LoadValues()
        {
            _repository = new RepositoryBase<Note>(new DatabaseFactory());

            var list = _repository.GetMany(p => !p.IsDeleted).ToList();
            listMain.DataSource = list;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_Note != null)
            {
                _Note.Content = txtMain.Text;

                _repository.Update(_Note);
                _repository.DataContext.Commit();

                //listMain.();
            }
            else
                MessageBox.Show("Kayit Silinemedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnDelete.Enabled = false;

            if (_Note != null)
            {
                _repository.Delete(_Note.Id);
                _repository.DataContext.Commit();
            }
            else
                MessageBox.Show("Kayit Silinemedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            btnDelete.Enabled = true;
        }

        private void listMain_Click(object sender, EventArgs e)
        {
            var item = listMain.SelectedItem;
            txtMain.Text = (item as Note).Content;
            _Note = (item as Note);

            txtMain.Tag = (item as Note).Id;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            listMain.BeginUpdate();

            var list = _repository.GetMany(p => !p.IsDeleted).ToList();
            listMain.DataSource = list;
            listMain.EndUpdate();
        }
    }
}
