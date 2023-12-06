using GreenThumb.Database;
using GreenThumb.Managers;
using GreenThumb.Models;
using System.Windows;
using System.Windows.Controls;

namespace GreenThumb.Windows;

/// <summary>
/// Interaction logic for PlantWindow.xaml
/// </summary>
public partial class PlantWindow : Window
{
    public PlantWindow()
    {
        InitializeComponent();
        UpdatePlantList();
    }

    private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        UpdatePlantList();
    }

    private void UpdatePlantList()
    {
        string searchInput = txtSearch.Text;
        lstPlants.Items.Clear();
        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<PlantModel> plantRepo = new(context);
            List<PlantModel>? plantList = plantRepo.GetAllInclude("Instructions");
            if (plantList == null)
            {
                return;
            }
            foreach (var plant in plantList)
            {
                if (plant.Name.ToLower().Contains(searchInput.ToLower()))
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = plant;
                    item.Content = plant.Name;
                    lstPlants.Items.Add(item);
                }
            }
        }
    }

    private void btnAddPlant_Click(object sender, RoutedEventArgs e)
    {
        AddPlantWindow addPlantWindow = new AddPlantWindow();
        addPlantWindow.Show();
        Close();
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (lstPlants.SelectedItem == null)
        {
            return;
        }
        ListViewItem selectedItem = (ListViewItem)lstPlants.SelectedItem;
        PlantModel plantToRemove = (PlantModel)selectedItem.Tag;
        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<PlantModel> plantRepo = new(context);
            try
            {
                plantRepo.Delete(plantToRemove.PlantId);
                plantRepo.Complete();
                UpdatePlantList();
            }
            catch
            {
                MessageBox.Show($"Could not delete that plant with id: {plantToRemove.PlantId}, please try again! If this keeps comming up please send to IT department :)");
            }

        }
    }

    private void btnDetails_Click(object sender, RoutedEventArgs e)
    {
        ListViewItem selectedItem = (ListViewItem)lstPlants.SelectedItem;
        if (selectedItem == null)
        {
            MessageBox.Show("No plant selected!");
            return;
        }
        PlantModel plant = (PlantModel)selectedItem.Tag;
        PlantDetailsWindow plantDetailsWindow = new(plant.PlantId);
        plantDetailsWindow.Show();
        Close();
    }

    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
        UserManager.currentUser = null;
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void btnMyGarden_Click(object sender, RoutedEventArgs e)
    {
        MyGardenWindow mainWindow = new MyGardenWindow();
        mainWindow.Show();
        Close();
    }
}
