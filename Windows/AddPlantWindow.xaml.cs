using GreenThumb.Database;
using GreenThumb.Models;
using System.Windows;
using System.Windows.Controls;

namespace GreenThumb.Windows;

/// <summary>
/// Interaction logic for AddPlantWindow.xaml
/// </summary>
public partial class AddPlantWindow : Window
{
    public AddPlantWindow()
    {
        InitializeComponent();
    }

    //Adds the instruction as a string on the content and to ListView
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

    //Removes selected instruction
    private void btnRemoveInstruction_Click(object sender, RoutedEventArgs e)
    {
        if (lstInstructions.SelectedItem == null)
        {
            MessageBox.Show("No instruction selected!");
            return;
        }
        lstInstructions.Items.Remove(lstInstructions.SelectedItem);
    }

    //Adds the plant to the database.
    private void btnAddPlant_Click(object sender, RoutedEventArgs e)
    {
        string plantName = txtPlantName.Text.Trim();
        string imgUrl = txtImgUrl.Text.Trim();

        //Input checks:
        if (string.IsNullOrEmpty(plantName))
        {
            MessageBox.Show("Plant needs to have a name!");
            return;
        }
        else

        if (!string.IsNullOrEmpty(imgUrl) && !Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute)) //This can be a website that is not an image URL, If you want to try make to open image in new tab and copy that URL.
        {
            MessageBox.Show("Image URL is not a valid Url");
            return;
        }

        string plantDescription = txtDescription.Text.Trim();
        using (GreenThumbDbContext context = new())
        {

            try
            {
                GreenThumbRepository<PlantModel> plantRepo = new(context);

                //Check if name exists in database
                if (plantRepo.GetAll().FirstOrDefault(p => p.Name.ToLower() == plantName.ToLower()) != null)
                {
                    MessageBox.Show("Plant already exists!");
                    return;
                }

                //Create the new PlantModel
                PlantModel newPlant = new PlantModel()
                {
                    Name = plantName,
                    Description = plantDescription,
                    ImgUrl = imgUrl,
                };
                plantRepo.Add(newPlant); //Add to database and save.
                plantRepo.Complete();

                //Add all instructions to the PlantModel and then save
                foreach (ListViewItem item in lstInstructions.Items)
                {
                    InstructionModel newInstruction = new InstructionModel()
                    {
                        InstructionText = item.Content.ToString()!,
                        PlantId = newPlant.PlantId,
                    };
                    newPlant.Instructions.Add(newInstruction);
                }
                plantRepo.Complete();
                MessageBox.Show($"{newPlant.Name} added successfully!");

                ClearInputs(); //Clear all inputs to be ready for next plant
            }
            catch
            {
                //Something went wrong with the database
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
        txtImgUrl.Text = string.Empty;
    }
}
