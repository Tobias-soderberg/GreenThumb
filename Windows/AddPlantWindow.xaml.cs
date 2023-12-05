using GreenThumb.Database;
using GreenThumb.Models;
using System.Windows;
using System.Windows.Controls;

namespace GreenThumb.Windows
{
    /// <summary>
    /// Interaction logic for AddPlantWindow.xaml
    /// </summary>
    public partial class AddPlantWindow : Window
    {
        public AddPlantWindow()
        {
            InitializeComponent();
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

        private void btnRemoveInstruction_Click(object sender, RoutedEventArgs e)
        {
            lstInstructions.Items.Remove(lstInstructions.SelectedItem);
        }

        private void btnAddPlant_Click(object sender, RoutedEventArgs e)
        {
            string plantName = txtPlantName.Text.Trim();
            if (string.IsNullOrEmpty(plantName))
            {
                MessageBox.Show("Plant needs to have a name!");
                return;
            }
            string plantDescription = txtDescription.Text.Trim();
            using (GreenThumbDbContext context = new())
            {

                try
                {
                    GreenThumbRepository<PlantModel> plantRepo = new(context);

                    if (plantRepo.GetAll().FirstOrDefault(p => p.Name.ToLower() == plantName.ToLower()) != null)
                    {
                        MessageBox.Show("Plant already exists!");
                        return;
                    }

                    PlantModel newPlant = new PlantModel()
                    {
                        Name = plantName,
                        Description = plantDescription,
                    };
                    plantRepo.Add(newPlant);
                    plantRepo.Complete();

                    foreach (ListViewItem item in lstInstructions.Items)
                    {
                        InstructionModel newInstruction = new InstructionModel()
                        {
                            InstructionText = item.Content.ToString()!,
                            PlantId = newPlant.PlantId

                        };
                        newPlant.Instructions.Add(newInstruction);
                    }
                    plantRepo.Complete();
                    ClearInputs();
                    MessageBox.Show($"{newPlant.Name} added successfully!");
                    PlantWindow plantWindow = new PlantWindow();
                    plantWindow.Show();
                    Close();
                }
                catch
                {
                    MessageBox.Show("Could not add plant!");
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PlantWindow plantWindow = new PlantWindow();
            plantWindow.Show();
            Close();
        }

        private void ClearInputs()
        {
            lstInstructions.Items.Clear();
            txtDescription.Text = string.Empty;
            txtInstruction.Text = string.Empty;
            txtPlantName.Text = string.Empty;
        }
    }
}
