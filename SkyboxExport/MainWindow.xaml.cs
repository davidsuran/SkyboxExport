using Microsoft.Win32;
using System.Windows;
using System.Drawing;
using System.IO;
using Rectangle = System.Drawing.Rectangle;
using System;
using System.Drawing.Imaging;

namespace SkyboxExport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int SUPPORTED_RES = 1024;

        public static readonly Rectangle RecZPos = new Rectangle(2 * SUPPORTED_RES, 0, SUPPORTED_RES, SUPPORTED_RES);
        public static readonly Rectangle RecYPos = new Rectangle(0, SUPPORTED_RES, SUPPORTED_RES, SUPPORTED_RES);
        public static readonly Rectangle RecXPos = new Rectangle(SUPPORTED_RES, SUPPORTED_RES, SUPPORTED_RES, SUPPORTED_RES);
        public static readonly Rectangle RecYNeg = new Rectangle(2 * SUPPORTED_RES, SUPPORTED_RES, SUPPORTED_RES, SUPPORTED_RES);
        public static readonly Rectangle RecXNeg = new Rectangle(3 * SUPPORTED_RES, SUPPORTED_RES, SUPPORTED_RES, SUPPORTED_RES);
        public static readonly Rectangle RecZNeg = new Rectangle(2 * SUPPORTED_RES, 2 * SUPPORTED_RES, SUPPORTED_RES, SUPPORTED_RES);

        public readonly Rectangle[] rectangles = new Rectangle[]
        {
            RecZPos,
            RecYPos,
            RecXPos,
            RecYNeg,
            RecXNeg,
            RecZNeg
        };

        public System.Windows.Controls.Image[] ImageControlls = new System.Windows.Controls.Image[6];
        
        private string CurrentFolder { get; set; }

        private Bitmap[] Bitmaps { get; set; } = new Bitmap[6];

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Indicates that the initialization process for the element is complete.
        /// </summary>
        public override void EndInit()
        {
            base.EndInit();

            ImageControlls = new System.Windows.Controls.Image[]
            {
                ImageZPos,
                ImageYPos,
                ImageXPos,
                ImageYNeg,
                ImageXNeg,
                ImageZNeg
            };
        }

        /// <summary>
        /// Button click import.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnClickImport(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".bmp";
            //openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif|BMP Files (*.bmp)|*.bmp";
            openFileDialog.Filter = "BMP Files (*.bmp)|*.bmp";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string fullPath = openFileDialog.FileName;
                CurrentFolder = fullPath.Substring(0, fullPath.LastIndexOf(@"\"));
                               
                SplitImage(openFileDialog.OpenFile());
            }
                    

        }

        /// <summary>
        /// BTNs the click export.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void BtnClickExport(object sender, RoutedEventArgs e)
        {
            if (Bitmaps[0] == null)
            {
                // TODO: show dialog
                return;
            }

            for (int i = 0; i < Bitmaps.Length; i++)
            {
                Bitmaps[i].Save(FormattableString.Invariant($@"{CurrentFolder}\{ImageControlls[i].Name.Remove(0, 5)}.bmp"), ImageFormat.Bmp );
                Bitmaps[i].Dispose();
            }

            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Splits the image.
        /// </summary>
        /// <param name="stream">The stream.</param>
        private void SplitImage(Stream stream)
        {
            using (Bitmap sourceBitmap = new Bitmap(stream))
            {
                sourceBitmap.ValidateImport(SUPPORTED_RES);

                for (int i = 0; i < Bitmaps.Length; i++)
                {
                    Bitmaps[i] = new Bitmap(SUPPORTED_RES, SUPPORTED_RES);

                    using (var graphics = Graphics.FromImage(Bitmaps[i]))
                    {
                        graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, SUPPORTED_RES, SUPPORTED_RES), rectangles[i], GraphicsUnit.Pixel);

                        ImageControlls[i].Source = Bitmaps[i].ToImageSource();
                    }
                }
            }
        }

    }
}
