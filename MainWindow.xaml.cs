using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Пример_практика.Models;

namespace Пример_практика
{
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context;
        private TableType _currentTable = TableType.Users;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                _context = new AppDbContext();

                // Проверка подключения к базе данных
                if (_context.Database.CanConnect())
                {
                    // Загружаем пользователей при старте
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Не удалось подключиться к базе данных", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private enum TableType
        {
            Users,
            Products,
            Locations,
            Inventory,
            Tasks,
            OperationsLog
        }

        private void LoadUsers()
        {
            try
            {
                var users = _context.Users.ToList();
                UsersGrid.ItemsSource = users;
                TitleText.Text = "Пользователи";
                CountText.Text = $"Всего записей: {users.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                var products = _context.Products.ToList();
                ProductsGrid.ItemsSource = products;
                TitleText.Text = "Товары";
                CountText.Text = $"Всего записей: {products.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadLocations()
        {
            try
            {
                var locations = _context.Locations.ToList();
                LocationsGrid.ItemsSource = locations;
                TitleText.Text = "Ячейки хранения";
                CountText.Text = $"Всего записей: {locations.Count}";
                UpdateLocationStats(locations);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ячеек: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadInventory()
        {
            try
            {
                var inventory = _context.Inventories
                    .Include(i => i.Product)
                    .Include(i => i.Location)
                    .Select(i => new
                    {
                        i.ID,
                        ProductName = i.Product.Name,
                        ProductArticle = i.Product.Article,
                        LocationCode = i.Location.FullCode,
                        i.Quantity
                    })
                    .ToList();

                InventoryGrid.ItemsSource = inventory;
                TitleText.Text = "Остатки";
                CountText.Text = $"Всего записей: {inventory.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки остатков: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTasks()
        {
            try
            {
                var tasks = _context.Tasks
                    .Include(t => t.AssignedUser)
                    .Select(t => new
                    {
                        t.ID,
                        t.TaskType,
                        t.Status,
                        AssignedTo = t.AssignedUser.FullName,
                        DueDate = t.DueDate.HasValue ? t.DueDate.Value.ToString("dd.MM.yyyy") : ""
                    })
                    .ToList();

                TasksGrid.ItemsSource = tasks;
                TitleText.Text = "Задания";
                CountText.Text = $"Всего записей: {tasks.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заданий: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadOperationsLog()
        {
            try
            {
                var operations = _context.OperationsLogs
                    .Include(o => o.User)
                    .Select(o => new
                    {
                        o.ID,
                        OperationDate = o.OperationDate.ToString("dd.MM.yyyy HH:mm"),
                        UserName = o.User.FullName,
                        o.Action,
                        o.Details
                    })
                    .ToList();

                OperationsLogGrid.ItemsSource = operations;
                TitleText.Text = "Журнал операций";
                CountText.Text = $"Всего записей: {operations.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки журнала: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateLocationStats(List<Location> locations)
        {
            if (locations != null && locations.Any())
            {
                StatsPanel.Visibility = Visibility.Visible;
                TotalLocationsStat.Text = locations.Count.ToString();
                FreeLocationsStat.Text = locations.Count(l => !l.IsOccupied).ToString();
                OccupiedLocationsStat.Text = locations.Count(l => l.IsOccupied).ToString();
            }
            else
            {
                StatsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void HideAllGrids()
        {
            UsersGrid.Visibility = Visibility.Collapsed;
            ProductsGrid.Visibility = Visibility.Collapsed;
            LocationsGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            TasksGrid.Visibility = Visibility.Collapsed;
            OperationsLogGrid.Visibility = Visibility.Collapsed;
            StatsPanel.Visibility = Visibility.Collapsed;
        }

        // Обработчики навигации
        private void BtnUsers_Click(object sender, RoutedEventArgs e)
        {
            _currentTable = TableType.Users;
            HideAllGrids();
            UsersGrid.Visibility = Visibility.Visible;
            LoadUsers();
        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            _currentTable = TableType.Products;
            HideAllGrids();
            ProductsGrid.Visibility = Visibility.Visible;
            LoadProducts();
        }

        private void BtnLocations_Click(object sender, RoutedEventArgs e)
        {
            _currentTable = TableType.Locations;
            HideAllGrids();
            LocationsGrid.Visibility = Visibility.Visible;
            LoadLocations();
        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            _currentTable = TableType.Inventory;
            HideAllGrids();
            InventoryGrid.Visibility = Visibility.Visible;
            LoadInventory();
        }

        private void BtnTasks_Click(object sender, RoutedEventArgs e)
        {
            _currentTable = TableType.Tasks;
            HideAllGrids();
            TasksGrid.Visibility = Visibility.Visible;
            LoadTasks();
        }

        private void BtnOperationsLog_Click(object sender, RoutedEventArgs e)
        {
            _currentTable = TableType.OperationsLog;
            HideAllGrids();
            OperationsLogGrid.Visibility = Visibility.Visible;
            LoadOperationsLog();
        }

        // Обработчики операций
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (_currentTable)
                {
                    case TableType.Users:
                        var newUser = new User
                        {
                            FullName = "Новый пользователь",
                            Login = "newuser",
                            Password = "123",
                            Role = "Оператор"
                        };
                        _context.Users.Add(newUser);
                        _context.SaveChanges();
                        LoadUsers();
                        MessageBox.Show("Новый пользователь успешно добавлен", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    case TableType.Products:
                        var newProduct = new Product
                        {
                            Article = "NEW-001",
                            Name = "Новый товар",
                            Barcode = "123456789"
                        };
                        _context.Products.Add(newProduct);
                        _context.SaveChanges();
                        LoadProducts();
                        MessageBox.Show("Новый товар успешно добавлен", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    case TableType.Locations:
                        var newLocation = new Location
                        {
                            Row = "A",
                            Rack = "99",
                            Shelf = "99",
                            FullCode = "A-99-99",
                            IsOccupied = false
                        };
                        _context.Locations.Add(newLocation);
                        _context.SaveChanges();
                        LoadLocations();
                        MessageBox.Show("Новая ячейка успешно добавлена", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    default:
                        MessageBox.Show($"Добавление в раздел \"{TitleText.Text}\" временно недоступно", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object selectedItem = null;

                switch (_currentTable)
                {
                    case TableType.Users:
                        selectedItem = UsersGrid.SelectedItem as User;
                        if (selectedItem != null)
                        {
                            var user = selectedItem as User;
                            user.FullName += " (изменено)";
                            _context.SaveChanges();
                            LoadUsers();
                            MessageBox.Show("Пользователь успешно изменен", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Выберите пользователя для редактирования", "Предупреждение",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        break;

                    case TableType.Products:
                        selectedItem = ProductsGrid.SelectedItem as Product;
                        if (selectedItem != null)
                        {
                            var product = selectedItem as Product;
                            product.Name += " (ред.)";
                            _context.SaveChanges();
                            LoadProducts();
                            MessageBox.Show("Товар успешно изменен", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Выберите товар для редактирования", "Предупреждение",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        break;

                    case TableType.Locations:
                        selectedItem = LocationsGrid.SelectedItem as Location;
                        if (selectedItem != null)
                        {
                            var location = selectedItem as Location;
                            location.IsOccupied = !location.IsOccupied;
                            _context.SaveChanges();
                            LoadLocations();
                            MessageBox.Show("Статус ячейки изменен", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Выберите ячейку для редактирования", "Предупреждение",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        break;

                    default:
                        MessageBox.Show($"Редактирование в разделе \"{TitleText.Text}\" временно недоступно", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object selectedItem = null;
                string itemName = "";

                switch (_currentTable)
                {
                    case TableType.Users:
                        selectedItem = UsersGrid.SelectedItem as User;
                        itemName = "пользователя";
                        break;
                    case TableType.Products:
                        selectedItem = ProductsGrid.SelectedItem as Product;
                        itemName = "товар";
                        break;
                    case TableType.Locations:
                        selectedItem = LocationsGrid.SelectedItem as Location;
                        itemName = "ячейку";
                        break;
                    case TableType.Inventory:
                        selectedItem = InventoryGrid.SelectedItem;
                        itemName = "остаток";
                        break;
                    case TableType.Tasks:
                        selectedItem = TasksGrid.SelectedItem as WarehouseTask;
                        itemName = "задание";
                        break;
                    case TableType.OperationsLog:
                        MessageBox.Show("Журнал операций только для просмотра", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                }

                if (selectedItem == null)
                {
                    MessageBox.Show($"Выберите {itemName} для удаления", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Вы уверены, что хотите удалить {itemName}?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    switch (_currentTable)
                    {
                        case TableType.Users:
                            _context.Users.Remove(selectedItem as User);
                            break;
                        case TableType.Products:
                            _context.Products.Remove(selectedItem as Product);
                            break;
                        case TableType.Locations:
                            _context.Locations.Remove(selectedItem as Location);
                            break;
                        case TableType.Inventory:
                            dynamic invItem = selectedItem;
                            int invId = invItem.ID;
                            var inventory = _context.Inventories.Find(invId);
                            if (inventory != null)
                                _context.Inventories.Remove(inventory);
                            break;
                        case TableType.Tasks:
                            _context.Tasks.Remove(selectedItem as WarehouseTask);
                            break;
                    }

                    _context.SaveChanges();

                    // Обновляем соответствующую таблицу
                    switch (_currentTable)
                    {
                        case TableType.Users:
                            LoadUsers();
                            break;
                        case TableType.Products:
                            LoadProducts();
                            break;
                        case TableType.Locations:
                            LoadLocations();
                            break;
                        case TableType.Inventory:
                            LoadInventory();
                            break;
                        case TableType.Tasks:
                            LoadTasks();
                            break;
                    }

                    MessageBox.Show($"Запись успешно удалена", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}