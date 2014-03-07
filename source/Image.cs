using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

// Thank you, Jeroen van Langen - http://stackoverflow.com/a/5175424/218882 and Ivan Leonenko - http://stackoverflow.com/a/12638859/218882

namespace CachedImage
{
    /// <summary>
    /// Represents a control that is a wrapper on System.Windows.Controls.Image for enabling filesystem-based caching
    /// </summary>
    public class Image : System.Windows.Controls.Image
    {
        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register("ImageUrl",
            typeof (string), typeof (Image), new PropertyMetadata("", ImageUrlPropertyChanged));

        static Image()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (Image),
                new FrameworkPropertyMetadata(typeof (Image)));
        }

        /// <summary>
        /// Gets or sets the url for the cached image
        /// </summary>
        public string ImageUrl
        {
            get { return (string) GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        private static void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var url = (String) e.NewValue;

            if (String.IsNullOrEmpty(url))
                return;

            var cachedImage = (Image) obj;
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(FileCache.FromUrl(url));
            bitmapImage.EndInit();
            cachedImage.Source = bitmapImage;
        }
    }
}