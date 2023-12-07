﻿using GreenThumb.Database;
using GreenThumb.Managers;
using GreenThumb.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GreenThumb.Windows;

/// <summary>
/// Interaction logic for MyGardenWindow.xaml
/// </summary>
public partial class MyGardenWindow : Window
{
    private PlantModel selectedPlant;
    private List<TextBlock> selectedTextBlocks = new();
    public MyGardenWindow()
    {
        InitializeComponent();

        DisplayPlants();

    }

    internal MyGardenWindow(PlantModel plant)
    {
        selectedPlant = plant;

        InitializeComponent();

        DisplayPlants();
        SetSelectedText();
    }

    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
        PlantWindow plantWindow = new PlantWindow();
        plantWindow.Show();
        Close();
    }

    private void DisplayPlants()
    {
        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<GardenPlant> gpRepo = new(context);
            GreenThumbRepository<UserModel> userRepo = new(context);

            //Update user;
            UserManager.currentUser = userRepo.GetAllInclude("Garden.Plants").FirstOrDefault(u => u.UserId == UserManager.currentUser.UserId);
            userRepo.Update(UserManager.currentUser);

            List<PlantModel> plants = UserManager.currentUser.Garden.Plants;

            if (plants.Count == 0)
            {
                return;
            }

            int counter = 0; //Used to know which cell to put image / plant

            foreach (PlantModel plant in plants)
            {
                string imgUrlString = plant.ImgUrl;

                Grid cellGrid = new Grid();

                //Calculate and set where in the grid to set flowers
                int row = counter / 5;
                int col = counter % 5;

                cellGrid.SetValue(Grid.ColumnProperty, col);
                cellGrid.SetValue(Grid.RowProperty, row);
                cellGrid.MouseLeftButtonDown += CellGrid_Clicked; //Assign method "CellGrid_Clicked to eventhandler
                cellGrid.Tag = plant;

                if (!string.IsNullOrEmpty(imgUrlString))
                {
                    //Create Image with url from plant
                    Image img = new Image { };
                    BitmapImage bitmapImage = new BitmapImage(new Uri(imgUrlString));
                    img.Source = bitmapImage;
                    img.Stretch = Stretch.UniformToFill;
                    cellGrid.Children.Add(img);
                }
                else
                {
                    //Create Image with standard image
                    Image img = new Image { };
                    BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/GreenThumb;component/assets/SadPlant.jpg"));
                    img.Source = bitmapImage;
                    img.Stretch = Stretch.Fill;
                    cellGrid.Children.Add(img);
                }


                //Create textblocks

                TextBlock nameTextBlock = new TextBlock
                {
                    Text = plant.Name, // Use the name property of the plant
                    Foreground = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontWeight = FontWeights.Bold,
                    Background = new SolidColorBrush(Color.FromArgb(120, 255, 255, 255)), //Set semi transparent background
                };

                GardenPlant relation = gpRepo.GetAll().FirstOrDefault(gp => gp.PlantId == plant.PlantId); //No need to check GardenId as we only have plants in the garden in plants list
                TextBlock numberTextBlock = new TextBlock
                {
                    Text = relation.Quanity.ToString(), // Set Quantity of the plant
                    Foreground = Brushes.Black,
                    Background = Brushes.White,
                    FontSize = 14,
                    Margin = new Thickness(0, 0, 2, 0),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                TextBlock selectionTextBlock = new TextBlock
                {
                    Text = "Selected",
                    Foreground = Brushes.Green,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Visibility = Visibility.Hidden,
                    Tag = plant,
                };

                selectedTextBlocks.Add(selectionTextBlock);

                //Add them to the cell grid
                cellGrid.Children.Add(nameTextBlock);
                cellGrid.Children.Add(numberTextBlock);
                cellGrid.Children.Add(selectionTextBlock);

                //Add cellgrid to main one
                gridGarden.Children.Add(cellGrid);

                counter++;
            }
        }
    }
    private void CellGrid_Clicked(object sender, MouseButtonEventArgs e)
    {
        if (sender is Grid cellgrid && cellgrid.Tag is PlantModel)
        {
            Grid clickedCell = (Grid)sender;
            selectedPlant = (PlantModel)clickedCell.Tag;
            SetSelectedText();
        }
    }

    private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
    {
        if (selectedPlant == null)
        {
            return;
        }

        if (MessageBox.Show("Are you sure you want to remove all?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No)
        {
            return;
        }

        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<GardenPlant> gpRepo = new(context);
            GardenPlant? relation = gpRepo.GetAll().FirstOrDefault(gp => gp.GardenId == UserManager.currentUser.Garden.GardenId && gp.PlantId == selectedPlant.PlantId);
            if (relation != null)
            {
                gpRepo.Delete(relation);
                gpRepo.Complete();

                MyGardenWindow myGardenWindow = new MyGardenWindow();
                myGardenWindow.Show();
                Close();
            }
        }
        SetSelectedText();
    }

    private void btnRemoveOne_Click(object sender, RoutedEventArgs e)
    {
        if (selectedPlant == null)
        {
            return;
        }

        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<GardenPlant> gpRepo = new(context);
            GardenPlant? relation = gpRepo.GetAll().FirstOrDefault(gp => gp.GardenId == UserManager.currentUser.Garden.GardenId && gp.PlantId == selectedPlant.PlantId);
            if (relation != null)
            {
                relation.Quanity--;

                gpRepo.Update(relation);
                gpRepo.Complete();

                if (relation.Quanity == 0)
                {
                    gpRepo.Delete(relation);
                    gpRepo.Complete();
                }

                MyGardenWindow myGardenWindow = new MyGardenWindow(selectedPlant);
                myGardenWindow.Show();
                Close();
            }
        }
    }
    private void btnAddOne_Click(object sender, RoutedEventArgs e)
    {
        if (selectedPlant == null)
        {
            return;
        }

        using (GreenThumbDbContext context = new())
        {
            GreenThumbRepository<GardenPlant> gpRepo = new(context);
            GardenPlant? relation = gpRepo.GetAll().FirstOrDefault(gp => gp.GardenId == UserManager.currentUser.Garden.GardenId && gp.PlantId == selectedPlant.PlantId);
            if (relation != null)
            {
                relation.Quanity++;

                gpRepo.Update(relation);
                gpRepo.Complete();

                MyGardenWindow myGardenWindow = new MyGardenWindow(selectedPlant);
                myGardenWindow.Show();
                Close();
            }
        }
    }

    private void SetSelectedText()
    {
        foreach (var textBlock in selectedTextBlocks)
        {
            PlantModel plant = (PlantModel)textBlock.Tag;
            if (plant.PlantId == selectedPlant.PlantId)
            {
                textBlock.Visibility = Visibility.Visible;
            }
            else
            {
                textBlock.Visibility = Visibility.Hidden;
            }
        }
    }

}
