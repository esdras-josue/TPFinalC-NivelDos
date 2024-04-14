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
using System.IO;
using System.Configuration;

namespace Commerce_Catalog_Manager
{
    public partial class frmAddItem : Form
    {
        private Item item = null;
        private OpenFileDialog openFild = null;
        public frmAddItem()
        {
            InitializeComponent();
        }
        public frmAddItem(Item item)
        {
            InitializeComponent();
            this.item = item;
            Text = "Modify Item";
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            BussinesItem bussines = new BussinesItem();

            try
            {
                if(item == null)
                    item = new Item();
                
                item.Code = txtCode.Text;
                item.Name = txtName.Text;
                item.Description = txtDescripcion.Text;
                item.UrlImage = txtImagen.Text;
                item.Brand = (Brand)cboBrand.SelectedItem;
                item.Category = (Category)cboCategory.SelectedItem;
                item.Price = decimal.Parse(txtPrice.Text);

                if(item.Id != 0)
                {
                    bussines.modifyItem(item);
                    MessageBox.Show("successfully modified");
                }
                else
                {
                    bussines.addItem(item);
                    MessageBox.Show("successfully added");
                }

                if(openFild != null && !(txtImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(openFild.FileName, ConfigurationManager.AppSettings["CatalogPro"] + openFild.FileName);

                }

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAddItem_Load(object sender, EventArgs e)
        {
            ItemBrand brand = new ItemBrand();
            ItemCategory category = new ItemCategory(); 
            try
            {
                cboBrand.DataSource = brand.brandList();
                cboBrand.ValueMember = "Id";
                cboBrand.DisplayMember = "Descripcion";
                cboCategory.DataSource = category.categoryList(); 
                cboCategory.ValueMember = "Id";
                cboCategory.DisplayMember = "Descripcion";
                
                if(item != null)
                {
                    txtCode.Text = item.Code;
                    txtName.Text = item.Name;
                    txtDescripcion.Text = item.Description;
                    txtImagen.Text = item.UrlImage;
                    LoadImage(item.UrlImage);
                    cboBrand.SelectedValue = item.Brand.Id;
                    cboCategory.SelectedValue = item.Category.Id;   


                    txtPrice.Text = item.Price.ToString();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            LoadImage(txtImagen.Text);
        }
        private void LoadImage(string image)
        {
            try
            {
                if (!string.IsNullOrEmpty(image))
                {
                    pbxItem.Load(image);
                }
            }
            catch (Exception ex)
            {
                pbxItem.Load("https://developers.elementor.com/docs/assets/img/elementor-placeholder-image.png");
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            openFild = new OpenFileDialog();
            openFild.Filter = "jpg|*.jpg;|png|*.png";
            openFild.ShowDialog();
            if(openFild.ShowDialog() == DialogResult.OK)
            {
                txtImagen.Text = openFild.FileName;
                LoadImage(openFild.FileName);

                //File.Copy(openFild.FileName, ConfigurationManager.AppSettings["CatalogPro"] + openFild.FileName);
            }

        }
    }
}
