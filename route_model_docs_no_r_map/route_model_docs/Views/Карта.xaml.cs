using DevExpress.Xpf.Core;
using DevExpress.Xpf.Map;
using DevExpress.Xpf.WindowsUI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextReplasment_class;

namespace route_model_docs.Views
{
    /// <summary>
    /// Логика взаимодействия для Карта.xaml
    /// </summary>
    public partial class Карта : UserControl
    {



        public Карта()
        {
            InitializeComponent();


            //    //waypoints.Add(new RouteWaypoint("Москва", new GeoPoint(55.8025, 37.528611)));
            //    //waypoints.Add(new RouteWaypoint("Владимир", new GeoPoint(56.1446, 40.41787)));
            //    //waypoints.Add(new RouteWaypoint("Нижний Новгород", new GeoPoint(56.1937, 44.0027)));

            //    //routeProvider.CalculateRoute(waypoints);
            //    //double distance = GeoUtils.CalculateDistance(new GeoPoint(55.8025, 37.528611),new GeoPoint(56.1446, 40.41787)); подсчет расстояния между двумя точками
            //    //teResult.Text = distance.ToString();
        }

        #region Класс RouteState и переменные
        class RouteState
        {
            //public Color Color { get; set; }
            public string StartLetter { get; set; }
            public string Pspintx { get; set; }
            public string StartPoint { get; set; }
            public string EndPoint { get; set; }
            public double Distance { get; set; }
            public int Road_lines { get; set; }
            public int Road_categ { get; set; }
            public double Road_price_per_m { get; set; }
            public double Usherb{ get; set; }
            public override string ToString()
            {
                return "По маршруту " + StartPoint + " - " + EndPoint + "\r" + StartLetter + " потому что ущерб " + Usherb + " рублей." + "\r\nХарактеристики маршрута - "  + " Количество полос: " + Road_lines + ", Категория дороги: " + Road_categ + ", Цена за метр: " + Road_price_per_m + ".\r\nДлина маршрута: " +Distance + " метров.";
            }
        }

        class CalcRes
        {
            public double calc_distance { get; set; }
        }

        List<CalcRes> calcReslst = new List<CalcRes>();
        List<RouteWaypoint> waypoints = new List<RouteWaypoint>();

        // Данные о дороге
        int max_mass = 14;

        //uint length = 5000;
        int road_lines = 3;
        int road_categ = 3;
        int road_price_per_m = 3500;

        string name;
        double lonvab = 0;
        double lanvab = 0;
        int nomerwp = 1;
        //bool con = true;

        public dots dot = new dots();//
        public paths path = new paths();//

        CollectionViewSource dvs = new CollectionViewSource();
        CollectionViewSource pvs = new CollectionViewSource();

        Queue<Action> requestQueue = new Queue<Action>();
        Color cl = Colors.Red;
        //string let = " ПРОЕЗД НЕВОЗМОЖЕН ИЗ-ЗА ПРЕВЫШЕНИЯ НАГРУЗКИ НА ОСИ!";

        TestEntities se = new TestEntities();
        #endregion

        //Поиск
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //var token = "ddff11da2528d2fcd08ba5dd256893408609ddd7";
            //var api = new SuggestClientAsync(token);
            //if (rb_a.IsChecked == true)
            //{
                searchDataProvider.Search(teKeywords.Text.ToString());
            //}

            //if (rb_f.IsChecked == true)
            //{
            //    //var result = await api.FindAddress("9120b43f-2fae-4838-a144-85e43c2bfb29"); ФИАС
            //    //var result = await api.FindAddress("77000000000268400"); КЛАДР
            //    var result = await api.FindAddress(teKeywords.Text);
            //    for (int i = 0; i < result.suggestions.Count(); i++)
            //    {
            //        val = result.suggestions[i].unrestricted_value;
            //    }
            //    searchDataProvider.Search(val);
            //}
        }

        //Добавление первого объекта поиска в маршрут
        private void To__route_Click(object sender, RoutedEventArgs e)
        {
            if(lonvab != 0 & lanvab != 0)
            {
                //waypoints.Add(new RouteWaypoint(name, new GeoPoint(lanvab, lonvab), Tag=c));
                if (trafficTextBox.Text != "Кол-во полос" & road_categTextBox.Text != "Категория" & road_priceTextBox.Text != "Цена за метр" & trafficTextBox.Text != "0" & road_categTextBox.Text != "0" & road_priceTextBox.Text != "0")
                {
                    waypoints.Add(new RouteWaypoint(trafficTextBox.Text + ";" + road_categTextBox.Text + ";" + road_priceTextBox.Text, new GeoPoint(lanvab, lonvab), Tag = name));
                    MessageBox.Show(name + " добавлен" + " под " + nomerwp.ToString() + "-м номером " + "в маршрут");
                    nomerwp++;
                }
                else
                {
                    MessageBox.Show("Введите количество полос, категорию дороги и цену за метр");
                }
            }
            else
            {
                MessageBox.Show("Сначала необходимо указать точку!");
            }
           
        }

        //Построение маршрута
        private void Route_Click(object sender, RoutedEventArgs e)
        {
            nomerwp = 1;
            //routeProvider.CalculateRoute(waypoints);
            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                var wp = waypoints[i];
                var wp2 = waypoints[i + 1];
                var sl = "ПРОЕЗД НЕВОЗМОЖЕН";
                var psp = i + 1;
                var col = Colors.Red;

                string[] disc = wp.Description.Split(';');
                int traffic = int.Parse(disc[0]);
                int categ = int.Parse(disc[1]);
                double price = double.Parse(disc[2]);

                //if (Int32.Parse(wp.Description) == 1)
                //{
                //    sl = "ПРОЕЗД ВОЗМОЖЕН";
                //    col = Colors.Green;
                //}

                requestQueue.Enqueue(new Action(() => {
                    routeProvider.CalculateRoute(new List<RouteWaypoint>() { wp, wp2 }, new RouteState()
                    {
                        //Color = col,
                        StartLetter = sl,
                        Pspintx = psp.ToString(),
                        StartPoint = wp.Tag.ToString(),
                        EndPoint = wp2.Tag.ToString(),
                        Road_lines = traffic,
                        Road_categ = categ,
                        Road_price_per_m = price,
                    });;
                }));
            }
            try
            {
                requestQueue.Dequeue().Invoke();
            }
            catch
            {
                MessageBox.Show("Маршрут пуст!");
                return;
            }
            MessageBox.Show("Маршрут построен!");
            waypoints = new List<RouteWaypoint>();
        }


        //Вывод результата поиска
        private void OnSearchCompleted(object sender, BingSearchCompletedEventArgs e)
        {

            if (e.Cancelled) return;
            if (e.RequestResult.ResultCode != RequestResultCode.Success)
            {
                MessageBox.Show("Поиск не дал результатов");
                return;
            }

            StringBuilder resultList = new StringBuilder("");
            int resCounter = 1;
            foreach (LocationInformation resultInfo in e.RequestResult.SearchResults)
            {
                resultList.Append(String.Format("Результат {0}:\r\n", resCounter));
                resultList.Append(String.Format("Название: {0}\r\n", resultInfo.DisplayName));
                resultList.Append(String.Format("Адрес: {0}\r\n", resultInfo.Address.FormattedAddress));
                resultList.Append(String.Format("Широта: {0}", Math.Round(resultInfo.Location.Latitude, 2)));
                resultList.Append(String.Format("  Долгота: {0}\r\n", Math.Round(resultInfo.Location.Longitude, 2)));
                resultList.Append(String.Format("______________________________\r\n"));
                resCounter++;

                lonvab = resultInfo.Location.Longitude;
                lanvab = resultInfo.Location.Latitude;

                name = resultInfo.Address.FormattedAddress;

                //SqlCommand command = 
            }
            teResult.Text = resultList.ToString();

        }


        private void OnLayerItemsGenerating(object sender, LayerItemsGeneratingEventArgs args)
        {
            mapControl.ZoomToFit(args.Items);
        }


        private void routeProvider_LayerItemsGenerating(object sender, LayerItemsGeneratingEventArgs args)
        {
            RouteState routeState = args.UserState as RouteState;
            char letter = Convert.ToChar(routeState.Pspintx);
            foreach (MapItem item in args.Items)
            {
                MapPushpin pushpin = item as MapPushpin;
                if (pushpin != null)
                    pushpin.Text = letter++.ToString();
                MapPolyline line = item as MapPolyline;
                if (line != null)
                {
                    routeState.Distance = Math.Round(GeoUtils.CalculateStrokeLength(line), 2);
                    Road_price_drop test = new Road_price_drop();
                    var res = test.evaluate_one_part(
                        traffic_lanes: routeState.Road_lines, // Указать как [0]
                        length: routeState.Distance,
                        road_categ: routeState.Road_categ, // Указать как [1]
                        max_mass_on_axis: max_mass,
                        road_price_per_meter: routeState.Road_price_per_m // Указать как [2]
                        );

                    if (res < 200000)
                    {
                        line.Stroke = new SolidColorBrush(Colors.Green);
                        routeState.StartLetter = "ПРОЕЗД ВОЗМОЖЕН";
                    }
                    else
                    {
                        line.Stroke = new SolidColorBrush(Colors.Red);
                    }
                    routeState.Usherb = res;
                    line.Tag = routeState;
                }
            }
            mapControl.ZoomToFit(args.Items);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn1 = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
            conn1.Open();
            SqlCommand cmd1 = new SqlCommand("select Id_path, path_name from paths where path_name=@path_name", conn1);
            cmd1.Parameters.AddWithValue("path_name", path_nameTextBox.Text);
            SqlDataReader reader1;
            reader1 = cmd1.ExecuteReader();
            if (reader1.Read())
            {
                if (trafficTextBox.Text != "Кол-во полос" & road_categTextBox.Text != "Категория" & road_priceTextBox.Text != "Цена за метр")
                {
                    dot = new dots
                    {
                        address = name,
                        lan = lanvab,
                        lon = lonvab,
                        traf_categ_price= trafficTextBox.Text + ";" + road_categTextBox.Text + ";" + road_priceTextBox.Text,
                        Path_FK = (int)reader1["Id_path"]
                    };
                    se.dots.Add(dot);
                    se.SaveChanges();
                    MessageBox.Show("Точка сохранена в маршрут: " + reader1["path_name"]);
                }
                else
                {
                    MessageBox.Show("Введите количество полос, категорию дороги и цену за метр");
                }
            }
            else
                MessageBox.Show("Введите существующее имя маршрута или создайте новый!");

            conn1.Close();

        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (rod.Visibility == Visibility.Hidden)
            {
                rod.Visibility = Visibility.Visible;
                hyb.Visibility = Visibility.Hidden;
            }
            else
            {
                hyb.Visibility = Visibility.Visible;
                rod.Visibility = Visibility.Hidden;
            }

        }

        private void Path_from_saved_Click(object sender, RoutedEventArgs e)
        {
            waypoints = new List<RouteWaypoint>();
            //var startLatters = new char[] { '1', '2' };
            //waypoints_r = new List<RouteWaypoint>();
            SqlConnection conn1 = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
            conn1.Open();
            SqlCommand cmd1 = new SqlCommand("Select Id_dot, traf_categ_price, address, lan, lon, Path_FK from dots where Path_FK=@Path_FK", conn1);
            int id_path = id_p();
            cmd1.Parameters.AddWithValue("Path_FK", id_path);

            SqlDataReader reader1;
            reader1 = cmd1.ExecuteReader();
            int countrows = A();
            int path_fk = 0;
            string cango;
            for (int j = 0; j < countrows; j++)
            {
                if (reader1.Read())
                {
                    name = (string)reader1["address"];
                    lanvab = (double)reader1["lan"];
                    lonvab = (double)reader1["lon"];
                    path_fk = (int)reader1["Path_FK"];
                    cango = (string)reader1["traf_categ_price"];
                    waypoints.Add(new RouteWaypoint(cango, new GeoPoint(lanvab, lonvab), name));
                }
                else
                    MessageBox.Show("Данные не найдены!");
            }

            for (int i = 0; i < waypoints.Count - 1; i++)
            {
                var wp = waypoints[i];
                var wp2 = waypoints[i + 1];
                var sl = "ПРОЕЗД НЕВОЗМОЖЕН";
                var psp = i + 1;
                var col = Colors.Red;

                string[] disc = wp.Description.Split(';');
                int traffic = int.Parse(disc[0]);
                int categ = int.Parse(disc[1]);
                int price = int.Parse(disc[2]);

                //if (Int32.Parse(wp.Description) == 1)
                //{
                //    sl = "ПРОЕЗД ВОЗМОЖЕН";
                //    col = Colors.Green;
                //}

                requestQueue.Enqueue(new Action(() => {
                    routeProvider.CalculateRoute(new List<RouteWaypoint>() { wp, wp2 }, new RouteState()
                    {
                        //Color = col,
                        StartLetter = sl,
                        Pspintx = psp.ToString(),
                        StartPoint = wp.Tag.ToString(),
                        EndPoint = wp2.Tag.ToString(),
                        Road_lines = traffic,
                        Road_categ = categ,
                        Road_price_per_m = price,
                    }); ;
                }));
            }
            try
            {
                requestQueue.Dequeue().Invoke();
            }
            catch
            {
                MessageBox.Show("Маршрут под номером " + id_p().ToString() + " пуст!");
                return;
            }

            MessageBox.Show("Сохраненный маршрут под номером " + path_fk.ToString() + " построен!");

            conn1.Close();
        }
        public int A()
        {
            string stmt = "SELECT COUNT(*) FROM dots WHERE Path_FK=@Path_FK";
            int count = 0;

            using (SqlConnection thisConnection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True"))
            {
                using (SqlCommand cmdCount = new SqlCommand(stmt, thisConnection))
                {
                    thisConnection.Open();
                    cmdCount.Parameters.AddWithValue("Path_FK", id_p());
                    count = (int)cmdCount.ExecuteScalar();
                }
            }
            return count;
        }
        public int id_p()
        {
            int id_p = 0;
            SqlConnection conn1 = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True");
            conn1.Open();
            SqlCommand cmd1 = new SqlCommand("select Id_path, path_name from paths where path_name=@path_name", conn1);
            cmd1.Parameters.AddWithValue("path_name", path_nameTextBox.Text);
            SqlDataReader reader1;
            reader1 = cmd1.ExecuteReader();


            if (reader1.Read())
            {
                id_p = (int)reader1["Id_path"];
                return id_p;
            }
            else
                MessageBox.Show("Введите существующее имя маршрута!");

            conn1.Close();
            return id_p;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mapControl.MouseLeftButtonUp += Map_MouseLeftButtonUp;

            var sup = se.Set<paths>().ToList();
            var sap = se.Set<dots>().ToList();

            pvs = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pathsViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            pvs.Source = se.paths.Local;

            //dvs = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pathsdotsViewSource")));
            //Загрузите данные, установив свойство CollectionViewSource.Source:
            //dvs.Source = se.dots.Local;
        }

        private void New_path_Click(object sender, RoutedEventArgs e)
        {
            if (New_path_tb.Text != "" & New_path_tb.Text != "Введите имя")
            {
                path = new paths
                {
                    path_name = New_path_tb.Text
                };
                se.paths.Add(path);
                se.SaveChanges();
                MessageBox.Show("Маршрут под названием " + New_path_tb.Text + " создан");
                this.pathsDataGrid.Items.Refresh();
            }
            else
                MessageBox.Show("Введите название маршрута!");

        }

        private void Del_dot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dt = pvs.View.CurrentItem as dots;
                MessageBoxResult result =
                MessageBox.Show("Вы действительно хотите удалить точку?", "Отмена", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;
                se.dots.Local.Remove(dt);
                se.SaveChanges();
                this.dotsDataGrid.Items.Refresh();
            }
            catch
            {
                MessageBox.Show("Удалили!");
            }
            
        }

        //protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        //{
        //    base.OnClosing(e);
        //    this.se.Dispose();
        //}

        private void New_path_tb_GotFocus(object sender, RoutedEventArgs e)
        {
            New_path_tb.Text = null;
            New_path_tb.Foreground = new SolidColorBrush() { Color = Colors.Black };
        }

        private void trafficTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            trafficTextBox.Text = null;
            trafficTextBox.Foreground = new SolidColorBrush() { Color = Colors.Black };
        }

        // Клик мышкой по карте и получение координат этой точки
        private void map_point_click(object sender, MouseButtonEventArgs e)
        {
            MapControl map = (MapControl)sender;
            MapHitInfo info = map.CalcHitInfo(e.GetPosition(map));
            if (info.InMapPushpin)
            {
                string l = info.MapPushpin.Location.ToString();
                string[] locs = l.Split(';');
                name = info.MapPushpin.Information.ToString();
                lanvab = double.Parse(locs[0]);
                lonvab = double.Parse(locs[1]);
                MessageBox.Show("Координаты этой точки " + l + ". Её можно добавить в маршрут!" /*+ info.MapPushpin.Information.ToString()*/);
                //MessageBoxResult result =
                //MessageBox.Show("Добавить точку с координатами " + l + " в маршрут? ", "Добавить точку", MessageBoxButton.YesNo);
                //if (result == MessageBoxResult.No) return;
                //waypoints.Add(new RouteWaypoint("0", new GeoPoint(lanvab, lonvab), Tag = name));
                //MessageBox.Show(name + " добавлен" + " под " + nomerwp.ToString() + "-м номером " + "в маршрут");
                //nomerwp++;
            }
        }

        // Клик мышкой по маршруту
        private void Map_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MapControl map = (MapControl)sender;
            MapHitInfo info = map.CalcHitInfo(e.GetPosition(map));
            if (info.InMapPolyline)
            {
                //DXDialogWindow dialogWindow = new DXDialogWindow("Title", MessageBoxButton.OKCancel);
                //info.MapPolyline.Tag.ToString();
                //dialogWindow.Show();

                //DXDialog d = new DXDialog("Information", DialogButtons.Ok, true);
                //d.Content = new TextBlock() { Text = info.MapPolyline.Tag.ToString() };
                //d.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
                //d.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                //d.ShowDialog();

                WinUIDialogWindow winuidialog = new WinUIDialogWindow("Информация о маршруте", MessageBoxButton.OK);
                winuidialog.Content = new TextBlock() { Text = info.MapPolyline.Tag.ToString() };
                winuidialog.ShowDialog();
                //MessageBox.Show(info.MapPolyline.Tag.ToString());
            }        
        }

        private void routeProvider_RouteCalculated(object sender, BingRouteCalculatedEventArgs e)
        {
            if (requestQueue.Count > 0)
                requestQueue.Dequeue().Invoke();

            //RouteCalculationResult result = e.CalculationResult;

            //StringBuilder resultList = new StringBuilder("");

            //for (int rnum = 0; rnum < e.CalculationResult.RouteResults.Count; rnum++)
            //{
            //    //resultList.Append(String.Format("_________________________\n"));
            //    //resultList.Append(String.Format("Path {0}:\n", rnum + 1));
            //    //resultList.Append(String.Format(
            //    //    "Distance: {0}\n",
            //    //    e.CalculationResult.RouteResults[rnum].Distance
            //    //));
            //    //resultList.Append(String.Format(
            //    //    "Time: {0}\n",
            //    //    e.CalculationResult.RouteResults[0].Time
            //    //));
            //    calcReslst.Add(new CalcRes()
            //    {
            //        calc_distance = e.CalculationResult.RouteResults[rnum].Distance
            //    }) ;
            //}

            //for(int i = 0; i < calcReslst.Count(); i++)
            //{
            //    resultList.Append(String.Format(i.ToString() + " " + calcReslst[i].calc_distance.ToString()));
            //}

            //teResult.Text = resultList.ToString();

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
            {
                this.se.Dispose();
            }

        private void road_categTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            road_categTextBox.Text = null;
            road_categTextBox.Foreground = new SolidColorBrush() { Color = Colors.Black };
        }

        private void road_priceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            road_priceTextBox.Text = null;
            road_priceTextBox.Foreground = new SolidColorBrush() { Color = Colors.Black };
        }

        private void road_priceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (road_priceTextBox.Text == "")
            {
                road_priceTextBox.Text = "Цена за метр";
                road_priceTextBox.Foreground = new SolidColorBrush() { Color = Colors.Gray };
            }
        }

        private void trafficTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (trafficTextBox.Text == "")
            {
                trafficTextBox.Text = "Кол-во полос";
                trafficTextBox.Foreground = new SolidColorBrush() { Color = Colors.Gray };
            }
        }

        private void road_categTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (road_categTextBox.Text == "")
            {
                road_categTextBox.Text = "Категория";
                road_categTextBox.Foreground = new SolidColorBrush() { Color = Colors.Gray };
            }
        }

        private void New_path_tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (New_path_tb.Text == "")
            {
                New_path_tb.Text = "Введите имя";
                New_path_tb.Foreground = new SolidColorBrush() { Color = Colors.Gray };
            }
        }
    } 
}
