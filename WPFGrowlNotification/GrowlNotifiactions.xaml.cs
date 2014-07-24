using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFGrowlNotification {
    public partial class GrowlNotifiactions {
        private const byte MaxNotifications = 4;

        private int _count;
        private readonly Notifications _notifications, _notificationsBuffer;

        public GrowlNotifiactions() {
            _notifications = new Notifications();
            _notificationsBuffer = new Notifications();

            InitializeComponent();
            NotificationsControl.DataContext = _notifications;
        }

        public void AddNotification(Notification notification) {
            notification.Id = _count++;
            if (_notifications.Count + 1 > MaxNotifications) {
                _notificationsBuffer.Add(notification);
            }
            else {
                _notifications.Add(notification);
            }

            // Show window if there're notifications
            if (_notifications.Count > 0 && !IsActive) {
                Show();
            }
        }

        public void RemoveNotification(Notification notification) {
            if (_notifications.Contains(notification)) {
                _notifications.Remove(notification);
            }

            if (_notificationsBuffer.Count > 0) {
                _notifications.Add(_notificationsBuffer[0]);
                _notificationsBuffer.RemoveAt(0);
            }

            //Close window if there's nothing to show
            if (_notifications.Count < 1) {
                Hide();
            }
        }

        private void NotificationWindowSizeChanged(object sender, SizeChangedEventArgs e) {
            if (Math.Abs(e.NewSize.Height) < 0.1) {
                var element = sender as Grid;
                if (element != null) {
                    RemoveNotification(_notifications.First(n => n.Id == int.Parse(element.Tag.ToString())));
                }
            }
        }
    }
}
