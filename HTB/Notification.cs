namespace HTB;

using System;


public class Notification
{
    public int NotificationId { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }

    
    public Notification(int notificationId, string message, bool isRead)
    {
        NotificationId = notificationId;
        Message = message;
        IsRead = isRead;
    }

    
    public void SendNotification()
    {
        Console.WriteLine($"Notification sent: {Message}");
    }

    
    public void ViewNotification()
    {
        Console.WriteLine($"Notification ID: {NotificationId}, Message: {Message}, Is Read: {IsRead}");
    }
}
