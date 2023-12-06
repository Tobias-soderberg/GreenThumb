using GreenThumb.Database;
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

            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //Create grid that is for each cell
                    Grid cellGrid = new Grid();
                    cellGrid.SetValue(Grid.ColumnProperty, i);
                    cellGrid.SetValue(Grid.RowProperty, j);

                    //Set image source and stretch it out to fill cell
                    Image img = new Image { };
                    BitmapImage bitmapImage = new BitmapImage(new Uri(@"https://media.discordapp.net/attachments/1181964626632638598/1181964689824030850/Flower2.jpg?ex=6582f965&is=65708465&hm=5f233a079b12525ad8405bdd13f8b0ea865c24eba89968b356dc2666eb76bae3&=&format=webp&width=909&height=909"));
                    img.Source = bitmapImage;

                    //Set Values on image
                    img.Stretch = System.Windows.Media.Stretch.UniformToFill;

                    //Create textblock
                    TextBlock numberTextBlock = new TextBlock
                    {
                        Text = 1.ToString(), // Change 1 to Quantity of the flower!
                        Foreground = Brushes.Black,
                        Background = Brushes.White,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Bottom
                    };

                    //Add to the cell grid
                    cellGrid.Children.Add(img);
                    cellGrid.Children.Add(numberTextBlock);

                    //Add cellgrid to main one
                    gridGarden.Children.Add(cellGrid);
                }

            }
        }
    }
}
