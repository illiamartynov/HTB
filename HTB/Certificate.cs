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

    public Certificate(int certificateId, DateTime issueDate)
    {
        _certificateId = certificateId;
        _issueDate = issueDate;
    }

    public void GenerateCertificate()
    {
        Console.WriteLine($"Certificate {_certificateId} generated.");
    }

    public void ViewCertificate()
    {
        Console.WriteLine($"Certificate ID: {_certificateId}, Issue Date: {_issueDate}");
    }
}