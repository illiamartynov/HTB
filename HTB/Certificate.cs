namespace HTB;

using System;
using System.ComponentModel.DataAnnotations;

public class Certificate
{
    private int _certificateId;
    private DateTime _issueDate;

    [Required(ErrorMessage = "Certificate ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Certificate ID must be a positive integer.")]
    public int CertificateId
    {
        get => _certificateId;
        private set => _certificateId = value;
    }

    [Required(ErrorMessage = "Issue Date is required.")]
    [DataType(DataType.Date, ErrorMessage = "Issue Date must be a valid date.")]
    public DateTime IssueDate
    {
        get => _issueDate;
        private set => _issueDate = value;
    }

    [Required]
    public Person Owner { get; private set; }

    // Приватный конструктор, чтобы ограничить прямое создание объектов
    private Certificate(int certificateId, DateTime issueDate, Person owner)
    {
        CertificateId = certificateId;
        IssueDate = issueDate;
        Owner = owner ?? throw new ArgumentNullException(nameof(owner), "Owner cannot be null.");
    }

    // Фабричный метод для создания сертификата
    public static Certificate Create(int certificateId, DateTime issueDate, Person owner)
    {
        return new Certificate(certificateId, issueDate, owner);
    }

    // Удаление ссылки на владельца
    public void UnassignOwner()
    {
        Owner = null;
    }

    public void ViewCertificate()
    {
        Console.WriteLine($"Certificate ID: {CertificateId}, Issue Date: {IssueDate}, Owner: {Owner.Name}");
    }
    // Certificate
    public void AssignOwner(Person owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

   
}
