using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

// Thank you, Jeroen van Langen - http://stackoverflow.com/a/5175424/218882 and Ivan Leonenko - http://stackoverflow.com/a/12638859/218882

namespace CachedImage
{
    /// <summary>
    /// Represents a control that is a wrapper on System.Windows.Controls.Image for enabling filesystem-based caching
    /// </summary>
    public class Image : System.Windows.Controls.Image
    {
        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register("ImageUrl",
            typeof(string), typeof(Image), new PropertyMetadata("", ImageUrlPropertyChanged));

        public static readonly DependencyProperty LoadAsyncProperty = DependencyProperty.Register("LoadAsync",
            typeof(bool), typeof(Image), new PropertyMetadata(false));

        static Image()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Image),
                new FrameworkPropertyMetadata(typeof(Image)));
        }

        /// <summary>
        /// Gets or sets the url for the cached image
        /// </summary>
        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        public bool LoadAsync
        {
            get { return (bool)GetValue(LoadAsyncProperty); }
            set { SetValue(LoadAsyncProperty, value); }
        }

        private static async void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var url = (String)e.NewValue;
            var cachedImage = (Image)obj;

            if (String.IsNullOrEmpty(url))
                return;

            if (cachedImage.LoadAsync)
            {
                cachedImage.Source = await LoadImageAsync(url);
            }
            else
            {
                cachedImage.Source = LoadImage(url);
            }
        }

        private static Task<BitmapImage> LoadImageAsync(string url)
        {
            return Task.Run<BitmapImage>(() =>
            {
                return LoadImage(url);
            });
        }

        private static BitmapImage LoadImage(string url)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(FileCache.FromUrl(url));
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }
    }
}