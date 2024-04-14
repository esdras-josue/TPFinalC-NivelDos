using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using bussines;

namespace Commerce_Catalog_Manager
{
    public partial class Form1 : Form
    {
        private List<Item> ItemList; 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load();
            cboFilter.Items.Add("Name");
            cboFilter.Items.Add("Description");
            cboFilter.Items.Add("Price");
        }

        private void dgvItems_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvItems.CurrentRow != null)
            {
                Item selected = (Item)dgvItems.CurrentRow.DataBoundItem;
                LoadImage(selected.UrlImage);
            }      
        }
        private void load()
        {
            BussinesItem bussiness = new BussinesItem();
            try
            {
                ItemList = bussiness.Listart();
                dgvItems.DataSource = ItemList;
                hideColumns();
                LoadImage(ItemList[0].UrlImage);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void hideColumns()
        {
            dgvItems.Columns["UrlImage"].Visible = false;
            dgvItems.Columns["Id"].Visible = false;
        }
        private void LoadImage(string image)
        {
            try
            {
                if(!string.IsNullOrEmpty(image))
                {
                    pbxItem.Load(image);
                }    
            }
            catch (Exception ex)
            {
                pbxItem.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            } 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddItem add = new frmAddItem();
            add.ShowDialog();
            load();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            Item selected;
            selected = (Item)dgvItems.CurrentRow.DataBoundItem;
            frmAddItem modify = new frmAddItem(selected);
            modify.ShowDialog();
            load();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            BussinesItem bussines = new BussinesItem();
            Item selected;
            try
            {
                DialogResult answer = MessageBox.Show("Are you sure you want to delete the item?","Deleting",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (answer == DialogResult.Yes)
                {
                    selected = (Item)dgvItems.CurrentRow.DataBoundItem;
                    bussines.delete(selected.Id);
                    MessageBox.Show("Succesfully Deleted");
                    load();
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool filterValidation()
        {
            if(cboFilter.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a filter");
                return true;
            }
            if(cboFilter.SelectedIndex < 0) 
            {
                MessageBox.Show("Please select a criterio");
                return true;
            }
            if(cboFilter.SelectedIndex.ToString() == "Price")
            {
                if (string.IsNullOrEmpty(cboFilter.Text))
                {
                    MessageBox.Show("Please enter a number");
                }
                if (!(onlyNumberAllowed(txtExplore.Text)))
                {
                    MessageBox.Show("Only numbers allowed");
                    return true;
                }
            }
            return false;
        }
        private bool onlyNumberAllowed(string number)
        {
            foreach(char character in number)
            {
                if (!(char.IsNumber(character)))
                    return false;
            }
              return true;      
        }
      
        private void btnSearch_Click(object sender, EventArgs e)
        {
            BussinesItem bussines = new BussinesItem();
            try
            {
                if (filterValidation())
                    return;

                string filter = cboFilter.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string explore = txtExplore.Text;
                dgvItems.DataSource = bussines.filterBy(filter,criterio,explore);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }   
        }

        private void txbSearch_TextChanged(object sender, EventArgs e)
        {
            List<Item> filteredList;
            string filter = txbSearch.Text;

            if (filter.Length > 3)
            {
                filteredList = ItemList.FindAll(i => i.Name.ToLower().Contains(filter) || i.Description.ToLower().Contains(filter) || i.Price.ToString().Contains(filter));
            }
            else
            {
                filteredList = ItemList;
            }

            dgvItems.DataSource = null;
            dgvItems.DataSource = filteredList;
            hideColumns();
        }

        private void cboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string option = cboFilter.SelectedItem.ToString();
            if(option == "Price")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Bigger than");
                cboCriterio.Items.Add("Less than");
                cboCriterio.Items.Add("Equals");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Starts With");
                cboCriterio.Items.Add("Ends With");
                cboCriterio.Items.Add("Contains");
            }       
        }
    }
}
