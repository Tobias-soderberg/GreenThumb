﻿using GreenThumb.Database;
using GreenThumb.Managers;
using GreenThumb.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GreenThumb.Windows
{
    /// <summary>
    /// Interaction logic for MyGardenWindow.xaml
    /// </summary>
    public partial class MyGardenWindow : Window
    {
        public MyGardenWindow()
        {
            InitializeComponent();

            DisplayPlants();
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


                    //Create textblock
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

                    //Add to the cell grid

                    cellGrid.Children.Add(numberTextBlock);

                    //Add cellgrid to main one
                    gridGarden.Children.Add(cellGrid);

                    counter++;
                }
            }
        }
    }
}
