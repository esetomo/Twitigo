using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Twitigo.Views
{
    public class TweetPanel : DockPanel
    {
        public TweetPanel()
        {
            Margin = new Thickness(4);
            HorizontalAlignment = HorizontalAlignment.Stretch;

            Children.Add(TweetAvatar());
            Children.Add(Body());
        }

        private Status S { get; set; } // Bindingに設定するPathをインテリセンスで調べる用

        private UIElement Body()
        {
            var panel = new StackPanel { };

            panel.Children.Add(TopLine());

            var text = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
            };
            text.SetBinding(TextBlock.TextProperty, "Text");
            panel.Children.Add(text);

            panel.Children.Add(TweetActions());

            return panel;
        }

        private UIElement TopLine()
        {
            var panel = new DockPanel { };

            var t = TweetTimestamps();
            DockPanel.SetDock(t, Dock.Right);
            panel.Children.Add(t);

            panel.Children.Add(TweetAuthor());

            return panel;
        }

        private UIElement TweetAvatar()
        {
            var image = new Image
            {
                Width = 48,
                Height = 48,
                VerticalAlignment = VerticalAlignment.Top,
                OpacityMask = AuthorImageMask,
            };
            image.SetBinding(Image.SourceProperty, "User.ProfileImageUrl");

            return image;
        }

        private UIElement TweetAuthor() // 1.
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };

            var name = new TextBlock { };
            name.SetBinding(TextBlock.TextProperty, "User.Name");
            panel.Children.Add(name);

            var screenName = new TextBlock { };
            screenName.SetBinding(
                TextBlock.TextProperty,
                new Binding("User.ScreenName")
                {
                    StringFormat = "@{0}",
                }
            );
            panel.Children.Add(screenName);

            return panel;
        }

        private UIElement TweetActions() // 2.
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };

            panel.Children.Add(ActionButton("Reply"));            
            panel.Children.Add(ActionButton("Retweet"));
            panel.Children.Add(ActionButton("Favorite"));

            return panel;
        }

        private UIElement TweetTimestamps() // 3.
        {
            var text = new TextBlock{};

            text.SetBinding(
                TextBlock.TextProperty,
                new Binding("CreatedAt")
                {
                    Converter = TimeConverter,
                }
            );

            // 7. IndividualTweet

            return text;
        }

        private UIElement RetweetIndicator()
        {
            throw new NotImplementedException();
        }

        // 5. については余計なものを追加しなければ良いはず
        // 6. TODO Add Branding to top of timeline

        private UIElement ActionButton(string name)
        {
            var button = new Button
            {
                Content = name,
            };

            button.SetBinding(
                Button.CommandProperty, 
                new Binding("DataContext." + name + "Command")
                {
                    RelativeSource = new RelativeSource
                    {
                        Mode = RelativeSourceMode.FindAncestor,
                        AncestorType = typeof(ItemsControl),
                    },
                }
            );

            button.SetBinding(
                Button.CommandParameterProperty,
                ".");


            return button;
        }

        private static readonly DrawingBrush AuthorImageMask;
        private static readonly IValueConverter TimeConverter;

        static TweetPanel()
        {
            var drawing = new GeometryDrawing
            {
                Brush = Brushes.White,
                Geometry = new RectangleGeometry(new Rect(0, 0, 48, 48), 6, 6),
            };

            AuthorImageMask = new DrawingBrush
            {
                Drawing = drawing,
            };

            TimeConverter = new TweetTimeConverter();
        }

        private class TweetTimeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                DateTime gmTime = (DateTime)value;
                DateTime localTime = gmTime.ToLocalTime();
                TimeSpan ts = DateTime.Now - localTime;

                if (ts < TimeSpan.FromMinutes(1))
                    return string.Format("{0}秒前", ts.Seconds);

                if(ts < TimeSpan.FromHours(1))
                    return string.Format("{0}分前", ts.Minutes);

                if(ts < TimeSpan.FromDays(1))
                    return string.Format("{0}時間前", ts.Hours);

                if(ts < TimeSpan.FromDays(365))
                    return localTime.ToString("M月d日");

                return localTime.ToString("y年M月d日");
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
