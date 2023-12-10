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
        private PlantModel? _plant; //Plant that is shown details of
        public PlantDetailsWindow(int plantId)
        {
            InitializeComponent();
            DisableEdit();
            using (GreenThumbDbContext context = new())
            {
                GreenThumbRepository<PlantModel> plantRepo = new(context);
                _plant = plantRepo.GetAllInclude("Instructions").FirstOrDefault(p => p.PlantId == plantId); //Get plant with instructions
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

        //Enables the edit functionallity in UI
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
            txtImgUrl.IsReadOnly = false;
        }

        //Disables the edit functionallity in UI
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
            txtImgUrl.IsReadOnly = true;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        //Creates a new plant to add to the garden or adds one more to the exisiting plant
        private void btnAddToGarden_Click(object sender, RoutedEventArgs e)
        {
            using (GreenThumbDbContext context = new())
            {
                GreenThumbRepository<GardenPlant> gpRepo = new(context);
                GreenThumbRepository<GardenModel> gardenRepo = new(context);

                int gardenId = UserManager.currentUser.Garden.GardenId;
                int plantId = _plant.PlantId;
                GardenPlant? relation = gpRepo.GetAll().FirstOrDefault(r => r.GardenId == gardenId && r.PlantId == plantId);

                if (relation == null)
                {
                    //Create new relation with 1 quantity
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
                    //Relation already exists, add one to the quantity property
                    relation.Quanity = relation.Quanity + 1;
                    gpRepo.Update(relation);
                }
                gpRepo.Complete();
                MessageBox.Show($"One {_plant.Name} added to your garden!\n\nTIPS: If you want more of the same plant\n its easier to handle in the garden! :)");
            }
        }

        //Saves the new changes to the active plant (_plant)
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            using (GreenThumbDbContext context = new())
            {
                GreenThumbRepository<PlantModel> plantRepo = new(context);
                GreenThumbRepository<InstructionModel> instructionRepo = new(context);

                string plantName = txtPlantName.Text.Trim();
                string description = txtDescription.Text.Trim();
                string imgUrl = txtImgUrl.Text.Trim();

                _plant.Name = plantName;
                _plant.Description = description;
                _plant.ImgUrl = imgUrl;

                if (!Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute)) //Can be a website URL that is not an image
                {
                    MessageBox.Show("Image URL is not a valid Url");
                    return;
                }

                foreach (var instuction in _plant.Instructions)
                {
                    //Remove all instructions
                    instructionRepo.Delete(instuction);
                }
                _plant.Instructions.Clear();
                foreach (ListViewItem item in lstInstructions.Items)
                {
                    //Add all the new instructions
                    _plant.Instructions.Add(new InstructionModel()
                    {
                        InstructionText = (string)item.Content,
                        PlantId = _plant.PlantId
                    });
                }
                plantRepo.Update(_plant);
                plantRepo.Complete();
            }
            DisableEdit();
            MessageBox.Show("Plant updated!");
        }

        //Goes back to the PlantWindow
        private void GoBack()
        {
            PlantWindow plantWindow = new PlantWindow();
            plantWindow.Show();
            Close();
        }

        //Sets the textBoxes to correct information
        private void SetDetails(PlantModel plant)
        {
            txtDescription.Text = plant.Description;
            txtPlantName.Text = plant.Name;
            txtImgUrl.Text = plant.ImgUrl;
            foreach (InstructionModel instruction in plant.Instructions)
            {
                ListViewItem item = new ListViewItem();
                item.Content = instruction.InstructionText;
                lstInstructions.Items.Add(item);
            }
        }

        //Removes selected item from ListView
        private void btnRemoveInstruction_Click(object sender, RoutedEventArgs e)
        {
            if (lstInstructions.SelectedItem == null)
            {
                MessageBox.Show("No instruction selected!");
                return;
            }
            lstInstructions.Items.Remove(lstInstructions.SelectedItem);
        }

        //Adds instruction to the ListView as a string in .Content
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
