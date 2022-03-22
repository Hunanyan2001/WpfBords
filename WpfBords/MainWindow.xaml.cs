using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfBords
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        public List<Ball> Balls = new List<Ball>();
        public MainWindow()
        {
            InitializeComponent();
            Ball ball = new Ball();
            ball.Ellipse = CreateAnEllipse(null);
            ball.Direction = Directions.Right;// GetRandomDirection(Directions.RightTop);
            ball.Speed = 10;
            Balls.Add(ball);
            BallCanvas.Children.Add(ball.Ellipse);
            Canvas.SetLeft(ball.Ellipse, 150);
            Canvas.SetTop(ball.Ellipse, 100);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.05); //Set the interval period here.
            timer.Tick += timerTick;
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {

            var ellipses = BallCanvas.Children.OfType<Ellipse>();
            var mockEllipses = new List<Ellipse>();
            mockEllipses.AddRange(ellipses);
            //Remove the previous ellipse from the paint canvas.
            foreach (var ellipse in mockEllipses)
            {
                var ball = Balls.Single(m => m.Ellipse == ellipse);
                BallCanvas.Children.Remove(ball.Ellipse);
                var left = Canvas.GetLeft(ball.Ellipse);
                var top = Canvas.GetTop(ball.Ellipse);
                var newEllipse = CreateAnEllipse(ball.Ellipse.Fill);
                BallCanvas.Children.Add(newEllipse);
                ball.Ellipse = newEllipse;

                switch (ball.Direction)
                {
                    case Directions.Right:
                        Canvas.SetLeft(ball.Ellipse, left + ball.Speed);
                        Canvas.SetTop(ball.Ellipse, top);
                        if (left >= BallCanvas.Width - ball.Ellipse.Width)
                        {
                            if (BallCanvas.Children.Count <= 50)
                            {
                                AddNewBall(left, top, GetRandomDirection(ball.Direction));
                            }
                            ball.Direction = GetRandomDirection(ball.Direction);
                        }
                        break;
                    case Directions.Left:
                        Canvas.SetLeft(ball.Ellipse, left - ball.Speed);
                        Canvas.SetTop(ball.Ellipse, top);
                        if (left <= 0)
                        {
                            if (BallCanvas.Children.Count <= 50)
                            {
                                AddNewBall(left, top, GetRandomDirection(ball.Direction));
                            }
                            ball.Direction = GetRandomDirection(ball.Direction);
                        }
                        break;
                    case Directions.Top:
                        Canvas.SetLeft(ball.Ellipse, left);
                        Canvas.SetTop(ball.Ellipse, top - ball.Speed);
                        if (top <= 0)
                        {
                            if (BallCanvas.Children.Count <= 50)
                            {
                                AddNewBall(left, top, GetRandomDirection(ball.Direction));
                            }
                            ball.Direction = GetRandomDirection(ball.Direction);
                        }
                        break;
                    case Directions.LeftTop:
                        Canvas.SetLeft(ball.Ellipse, left - ball.Speed);
                        Canvas.SetTop(ball.Ellipse, top - ball.Speed);
                        if (left <= 0 || top <= 0)
                        {
                            if (BallCanvas.Children.Count <= 50)
                            {
                                AddNewBall(left, top, GetRandomDirection(ball.Direction));
                            }
                            ball.Direction = GetRandomDirection(ball.Direction);
                        }
                        break;
                    case Directions.RightTop:
                        if (top < 0)
                        {
                            Canvas.SetTop(ball.Ellipse, 0);
                        }
                        else
                        {
                            Canvas.SetLeft(ball.Ellipse, left + ball.Speed);
                            Canvas.SetTop(ball.Ellipse, top - ball.Speed);
                        }

                        if (left >= BallCanvas.Width - ball.Ellipse.Width || top - ball.Speed <= 0)
                        {
                            if (BallCanvas.Children.Count <= 50)
                            {
                                AddNewBall(left, top, GetRandomDirection(ball.Direction));
                            }
                            ball.Direction = GetRandomDirection(ball.Direction);
                        }
                        break;
                    case Directions.Buttom:
                        Canvas.SetLeft(ball.Ellipse, left);
                        Canvas.SetTop(ball.Ellipse, top + ball.Speed);
                        if (top >= BallCanvas.Height - ball.Ellipse.Height - ball.Speed)
                        {
                            if (BallCanvas.Children.Count <= 50)
                            {
                                AddNewBall(left, top, GetRandomDirection(ball.Direction));
                            }
                            ball.Direction = GetRandomDirection(ball.Direction);
                        }
                        break;
                    case Directions.LeftButtom:
                        Canvas.SetLeft(ball.Ellipse, left - ball.Speed);
                        Canvas.SetTop(ball.Ellipse, top + ball.Speed);
                        if (left < 0 || top >= BallCanvas.Height - ball.Ellipse.Height)
                        {
                            if (BallCanvas.Children.Count <= 50)
                            {
                                AddNewBall(left, top, GetRandomDirection(ball.Direction));
                            }
                            ball.Direction = GetRandomDirection(ball.Direction);
                        }
                        break;
                    case Directions.RightButtom:
                        Canvas.SetLeft(ball.Ellipse, left + ball.Speed);
                        Canvas.SetTop(ball.Ellipse, top + ball.Speed);
                        if (left >= BallCanvas.Width - ball.Ellipse.Width || top >= BallCanvas.Height - ball.Ellipse.Height)
                        {
                            if (BallCanvas.Children.Count <= 50)
                            {
                                AddNewBall(left, top, GetRandomDirection(ball.Direction));
                            }
                            ball.Direction = GetRandomDirection(ball.Direction);
                        }
                        break;
                }

            }
        }

        private List<int> DetectOppositeDirection(Directions direction)
        {
            var names = Enum.GetNames(typeof(Directions));
            string directionName = Enum.GetName(typeof(Directions), direction);
            var splitedByCamelCase = directionName.SplitCamelCase();

            var possibleDirections = new List<int>();
            foreach (var name in names)
            {
                bool contains = false;
                foreach (var dirName in splitedByCamelCase)
                {
                    if (name.Contains(dirName))
                    {
                        contains = true;
                        break;
                    }
                }
                if (contains != true)
                {
                    var possibleDirection = (int)Enum.Parse(typeof(Directions), name);

                    possibleDirections.Add(possibleDirection);
                }
            }

            return possibleDirections;
        }

        private Directions GetRandomDirection(Directions direction)
        {
            Random random = new Random();
            var values = DetectOppositeDirection(direction);
            int index = random.Next(values.Count);

            return (Directions)values[index];
        }

        static private Brush RandomBrush()
        {
            Brush[] brushes =
            {
              new SolidColorBrush(){ Color = Colors.Red },
              new SolidColorBrush(){ Color = Colors.Blue},
              new SolidColorBrush(){ Color = Colors.Green},
              new SolidColorBrush(){ Color = Colors.Purple},
              new SolidColorBrush(){ Color = Colors.Brown},
              new SolidColorBrush(){ Color = Colors.DarkBlue},
              new SolidColorBrush(){ Color = Colors.BlueViolet },
            };
            Random color = new Random();
            return brushes[color.Next(0, brushes.Length)];
        }

        private void AddNewBall(double left, double top, Directions directions)
        {
            Ball newBall = new Ball();
            newBall.Direction = directions;
            newBall.Ellipse = CreateAnEllipse(null);
            newBall.Speed = 5;
            Balls.Add(newBall);
            BallCanvas.Children.Add(newBall.Ellipse);
            Canvas.SetTop(newBall.Ellipse, top);
            Canvas.SetLeft(newBall.Ellipse, left);
        }

        // Customize your ellipse in this method
        public Ellipse CreateAnEllipse(Brush color)
        {
            return new Ellipse()
            {
                Height = 60,
                Width = 60,
                StrokeThickness = 1,
                Fill = color == null ? RandomBrush() : color
            };
        }
    }
}
