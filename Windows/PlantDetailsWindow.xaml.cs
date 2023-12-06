using GreenThumb.Database;
using GreenThumb.Managers;
using GreenThumb.Models;
using System.Windows;
using System.Windows.Controls;

namespace GreenThumb.Windows
{
    /// <summary>
    /// Interaction logic for PlantDetailsWindow.xaml
    /// </summary>
    public partial class PlantDetailsWindow : Window
    {
        private PlantModel? _plant;
        public PlantDetailsWindow(int plantId)
        {
            InitializeComponent();
            DisableEdit();
            using (GreenThumbDbContext context = new())
            {
                GreenThumbRepository<PlantModel> plantRepo = new(context);
                _plant = plantRepo.GetAllInclude("Instructions").FirstOrDefault(p => p.PlantId == plantId);
                if (_plant == null)
                {
                    MessageBox.Show("The Id that PlantDetailsWindow got does not exist in the database!");
                    GoBack();
                }
                SetDetails(_plant);
            }

        }

        private void btnEditPlant_Click(object sender, RoutedEventArgs e)
        {
            EnableEdit();
        }

        private void EnableEdit()
        {
            lblInstruction.Visibility = Visibility.Visible;
            txtInstruction.Visibility = Visibility.Visible;
            btnAddInstruction.Visibility = Visibility.Visible;
            btnRemoveInstruction.Visibility = Visibility.Visible;
            btnUpdate.Visibility = Visibility.Visible;
            btnAddToGarden.Visibility = Visibility.Hidden;
            btnEditPlant.Visibility = Visibility.Hidden;
            txtDescription.IsReadOnly = false;
            txtPlantName.IsReadOnly = false;
        }

        private void DisableEdit()
        {
            lblInstruction.Visibility = Visibility.Hidden;
            txtInstruction.Visibility = Visibility.Hidden;
            btnAddInstruction.Visibility = Visibility.Hidden;
            btnRemoveInstruction.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Hidden;
            btnAddToGarden.Visibility = Visibility.Visible;
            btnEditPlant.Visibility = Visibility.Visible;
            txtDescription.IsReadOnly = true;
            txtPlantName.IsReadOnly = true;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void btnAddToGarden_Click(object sender, RoutedEventArgs e)
        {
            using (GreenThumbDbContext context = new())
            {
                GreenThumbRepository<GardenPlant> gpRepo = new(context);
                GreenThumbRepository<GardenModel> gardenRepo = new(context);

                GardenModel garden = gardenRepo.GetAllInclude("Plants").FirstOrDefault(g => g.UserId == UserManager.currentUser.UserId);

                int gardenId = UserManager.currentUser.Garden.GardenId;
                int plantId = _plant.PlantId;
                GardenPlant? relation = gpRepo.GetAll().FirstOrDefault(r => r.GardenId == gardenId && r.PlantId == plantId);

                if (relation == null)
                {
                    GardenPlant newRelation = new GardenPlant()
                    {
                        GardenId = UserManager.currentUser.Garden.GardenId,
                        PlantId = _plant.PlantId,
                        Quanity = 1
                    };
                    gpRepo.Add(newRelation);
                }
                else
                {
                    relation.Quanity = relation.Quanity + 1;
                    gpRepo.Update(relation);
                }
                gpRepo.Complete();

            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            DisableEdit();
            using (GreenThumbDbContext context = new())
            {
                GreenThumbRepository<PlantModel> plantRepo = new(context);
                GreenThumbRepository<InstructionModel> instructionRepo = new(context);

                string plantName = txtPlantName.Text;
                string description = txtDescription.Text;

                _plant.Name = plantName;
                _plant.Description = description;

                foreach (var instuction in _plant.Instructions)
                {
                    instructionRepo.Delete(instuction.InstructionId);
                }
                _plant.Instructions.Clear();
                foreach (ListViewItem item in lstInstructions.Items)
                {
                    _plant.Instructions.Add(new InstructionModel()
                    {
                        InstructionText = (string)item.Content,
                        PlantId = _plant.PlantId
                    });
                }
                plantRepo.Update(_plant);
                plantRepo.Complete();
            }
        }

        private void GoBack()
        {
            PlantWindow plantWindow = new PlantWindow();
            plantWindow.Show();
            Close();
        }

        private void SetDetails(PlantModel plant)
        {
            txtDescription.Text = plant.Description;
            txtPlantName.Text = plant.Name;
            foreach (InstructionModel instruction in plant.Instructions)
            {
                ListViewItem item = new ListViewItem();
                item.Content = instruction.InstructionText;
                lstInstructions.Items.Add(item);
            }
        }

        private void btnRemoveInstruction_Click(object sender, RoutedEventArgs e)
        {
            if (lstInstructions.SelectedItem == null)
            {
                MessageBox.Show("No instruction selected!");
                return;
            }
            lstInstructions.Items.Remove(lstInstructions.SelectedItem);
        }

        private void btnAddInstruction_Click(object sender, RoutedEventArgs e)
        {
            string instruction = txtInstruction.Text.Trim();
            if (string.IsNullOrEmpty(instruction))
            {
                MessageBox.Show("No instruction to add! Please make sure your instruction is entered");
                return;
            }
            ListViewItem item = new ListViewItem();
            item.Content = instruction;
            lstInstructions.Items.Add(item);

            txtInstruction.Text = string.Empty;
        }
    }
}
